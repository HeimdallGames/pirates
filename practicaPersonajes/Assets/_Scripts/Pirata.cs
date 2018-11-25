﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Pirata : MonoBehaviour
{
    [HideInInspector] private Movimiento movimiento;
    [SerializeField] public Mundo mundo;
    [SerializeField] public Transform prefab;


    [SerializeField] private float EndDistance = 15f;
    [SerializeField] private float huyendoDistance = 15f;
    
    [SerializeField] private float Patroll = 5f;
    [SerializeField] private float patrollRadius = 22f;

    public enum EstadoPirata { ESPERAR_BARCO, ATACAR, HUIR, CONSEGUIR_BOTIN, REFRESANDO, DESTRUIDO };
    [SerializeField] private EstadoPirata estadoActual;

	[SerializeField] private Comerciante target;
    [SerializeField] private Armada huyendoDe;
    [SerializeField] private List<GameObject> listaColisiones;
    [SerializeField] private GameObject collisionObject;

    //FSM
    private delegate void StateUpdate();
    private StateUpdate stateUpdate;
    void Start()
    {
        NavMeshAgent agent = transform.GetChild(1).GetComponent<NavMeshAgent>();
        movimiento = new Movimiento(agent, transform.GetChild(0), EndDistance, patrollRadius, Patroll);
        cambiarEstado(EstadoPirata.ESPERAR_BARCO);
        collisionObject = null;
    }

    void Update()
    {
        if (listaColisiones.Count > 0)
        {
            collisionObject = listaColisiones[0];
            listaColisiones.RemoveAt(0);
        }
        stateUpdate();
    }

    public void cambiarEstado(EstadoPirata nuevoEstado)
    {
        switch (nuevoEstado)
        {
            case EstadoPirata.ATACAR:
                stateUpdate = updateAtacando;
                break;
            case EstadoPirata.CONSEGUIR_BOTIN:
                stateUpdate = updateConsiguiendoBotin;
                break;
            case EstadoPirata.ESPERAR_BARCO:
                stateUpdate = updateEsperandoBarco;
                break;
            case EstadoPirata.HUIR:
                stateUpdate = updateHuyendo;
                break;
            case EstadoPirata.REFRESANDO:
                stateUpdate = updateRegresando;
                break;
            case EstadoPirata.DESTRUIDO:
                stateUpdate = updateDestruido;
                break;
        }
        estadoActual = nuevoEstado;
    }
    public Movimiento getMovimiento()
    {
        return movimiento;
    }

    private void updateRegresando()
    {
        if(movimiento.updateMovement(movimiento.getInitialPos())){
            cambiarEstado(EstadoPirata.ESPERAR_BARCO);
        }
    }
     private void updateDestruido()
    {
        mundo.addRespawn(prefab,transform.position,transform.rotation, 4);
        Destroy(gameObject);
    }
    private void updateAtacando()
    {
        if(movimiento.updateMovement (target.getMovimiento().getPos()))
        {
            cambiarEstado(EstadoPirata.CONSEGUIR_BOTIN);
        }
    }
    private void updateHuyendo()
    {
        if(movimiento.huir(huyendoDe.getMovimiento().getPos())<huyendoDistance){
            cambiarEstado(EstadoPirata.REFRESANDO);
        }

    }
    private void updateEsperandoBarco()
    {
        if ( collisionObject != null &&collisionObject.tag == "Comerciante")
        {
            target = collisionObject.GetComponent<Comerciante>();
            target.avisarEsPerseguido(this);
            cambiarEstado(EstadoPirata.ATACAR);
        }
        else{
            movimiento.patrullar();
        }
    }
    private void updateConsiguiendoBotin()
    {
        MonoBehaviour.print("El pirata: "+transform.name+"esta consiguiendo su botin.");
        cambiarEstado(EstadoPirata.REFRESANDO);
        target.atracar();
        
    }
    

    /*COLISIONES*/
	void OnCollisionEnter2D(Collision2D coll)
	{
        if( estadoActual ==  EstadoPirata.ESPERAR_BARCO && coll.transform.parent != null)
        {
        listaColisiones.Add(coll.transform.parent.gameObject);
        }
        //MonoBehaviour.print("collision detectada");
	}

    //COMUNICACION
    public void detectadoPorArmada(Armada armada){
        huyendoDe = armada;
        cambiarEstado(EstadoPirata.HUIR);
        MonoBehaviour.print("El pirata: "+transform.name+" ha sido detectado por la armada.");
    }
}