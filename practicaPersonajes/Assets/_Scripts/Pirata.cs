﻿using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;
using UnityEngine.AI;

public class Pirata : MonoBehaviour
{
    [HideInInspector] private Movimiento movimiento;
    [SerializeField] public Mundo mundo;
    [SerializeField] public Transform prefab;


    [SerializeField] private float EndDistance = 15f;
    [SerializeField] private float huyendoDistance = 59.1f;

    [SerializeField] private float Patroll = 5f;
    [SerializeField] private float patrollRadius = 22f;

    public enum EstadoPirata { ESPERAR_BARCO, ATACAR, HUIR, CONSEGUIR_BOTIN, DESTRUIDO };
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
        IAstarAI agent = transform.GetComponent<IAstarAI>();
        movimiento = new Movimiento(agent, EndDistance, patrollRadius, Patroll);
        cambiarEstado(EstadoPirata.ESPERAR_BARCO);
        collisionObject = null;
    }

    void Update()
    {
        if (listaColisiones.Count > 0)
        {
            collisionObject = listaColisiones[0];
            listaColisiones.RemoveAt(0);
            //MonoBehaviour.print("sacado collision object");
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

    private void updateDestruido()
    {
        MonoBehaviour.print(this.name + " destruido");
        mundo.addRespawn(prefab, transform.position, transform.rotation, 4);
        Destroy(gameObject);
    }
    private void updateAtacando()
    {
        if (movimiento.updateMovement(target.getMovimiento().getPos()))
        {
            collisionObject = null;
            cambiarEstado(EstadoPirata.CONSEGUIR_BOTIN);
        }
    }
    private void updateHuyendo()
    {
        if (movimiento.huir(huyendoDe.getMovimiento().getPos()) > huyendoDistance)
        {
            collisionObject = null;
            cambiarEstado(EstadoPirata.ESPERAR_BARCO);
            huyendoDe.cancelarPersecucion();
        }

    }
    private void updateEsperandoBarco()
    {
        if (collisionObject != null)
        {
            //MonoBehaviour.print("analizado collison object");
            target = collisionObject.GetComponent<Comerciante>();
            if (!target.desvalijado)
            {
                target.avisarEsPerseguido(this);
                cambiarEstado(EstadoPirata.ATACAR);
            }
            else
            {
                movimiento.patrullar();
            }
        }
        else
        {
            movimiento.patrullar();
        }
    }
    private void updateConsiguiendoBotin()
    {
        MonoBehaviour.print("El pirata: " + transform.name + " esta consiguiendo su botin.");
        cambiarEstado(EstadoPirata.ESPERAR_BARCO);
        target.atracar();

    }


    /*COLISIONES*/
    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.transform != null && coll.transform.gameObject.tag == "Comerciante")
        {
            //MonoBehaviour.print("collision detectada por PIRATA no nula");
            listaColisiones.Add(coll.transform.gameObject);
        }
        //MonoBehaviour.print("collision detectada por PIRATA");
    }

    //COMUNICACION
    public void detectadoPorArmada(Armada armada)
    {
        huyendoDe = armada;
        cambiarEstado(EstadoPirata.HUIR);
        MonoBehaviour.print("El pirata: " + transform.name + " ha sido detectado por la armada.");

    }

    public void barcoDestruido()
    {
        cambiarEstado(EstadoPirata.DESTRUIDO);
    }
}
