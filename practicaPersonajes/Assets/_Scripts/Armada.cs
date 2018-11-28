﻿using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;
using UnityEngine.AI;

public class Armada : MonoBehaviour
{
    [HideInInspector]
    public Movimiento movimiento;
    [SerializeField]
    public Mundo mundo;
    [SerializeField]
    private float extraEndDistance = 7.2f;
    [SerializeField]
    private float extraPatroll = 7.2f;
    [SerializeField]
    private float patrollRadius = 22.2f;

    [SerializeField]
    private Comerciante Llamada;
    [SerializeField]
    private Pirata Persiguiendo;
    [SerializeField]
    private List<GameObject> listaColisiones;
    [SerializeField]
    private GameObject collisionObject;

    //FSM
    private delegate void StateUpdate();
    private StateUpdate stateUpdate;
    public enum EstadoArmada { PATRULLANDO, AYUDA, PERSIGUE, ATRAPAR, ACOMPANA_COMERCIANTE };
    [SerializeField]
    private EstadoArmada estadoActual;
    void Start()
    {
        IAstarAI agent = transform.GetComponent<IAstarAI>();
        movimiento = new Movimiento(agent, extraEndDistance, patrollRadius, extraPatroll);
        cambiarEstado(EstadoArmada.PATRULLANDO);
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

    public void cambiarEstado(EstadoArmada nuevoEstado)
    {
        switch (nuevoEstado)
        {
            case EstadoArmada.PATRULLANDO:
                stateUpdate = updatePatrullando;
                break;
            case EstadoArmada.AYUDA:
                stateUpdate = updateAyudando;
                break;
            case EstadoArmada.PERSIGUE:
                stateUpdate = updatePersiguiendo;
                break;
            case EstadoArmada.ATRAPAR:
                stateUpdate = updateAtraparPirata;
                break;
            case EstadoArmada.ACOMPANA_COMERCIANTE:
                stateUpdate = updateAcompanando;
                break;
        }
        estadoActual = nuevoEstado;
    }
    public Movimiento getMovimiento()
    {
        return movimiento;
    }

    private void updatePatrullando()
    {

        if (collisionObject != null && collisionObject.tag == "Pirata")
        {
            Persiguiendo = collisionObject.GetComponent<Pirata>();
            if (Persiguiendo.atacando)
            {
                Persiguiendo.detectadoPorArmada(this);
                cambiarEstado(EstadoArmada.PERSIGUE);
            }
            else
            {
                collisionObject = null;
                movimiento.patrullar();
            }
        }
        else
        {
            collisionObject = null;
            movimiento.patrullar();
        }
    }
    private void updateAyudando()
    {
        //todo
    }
    private void updatePersiguiendo()
    {


        if (movimiento.updateMovement(Persiguiendo.getMovimiento().getPos()))
        {
            collisionObject = null;

            cambiarEstado(EstadoArmada.ATRAPAR);
        }
    }
    private void updateAcompanando()
    {

        /* if (collisionObject != null && collisionObject.tag == "Comerciante")
         {
             Llamada = collisionObject.GetComponent<Comerciante>();

         }
         */
        if (movimiento.updateMovement(Llamada.getMovimiento().getPos()))
        {

            collisionObject = null;

            cambiarEstado(EstadoArmada.PATRULLANDO);
        }
    }

    private void updateAtraparPirata()
    {
        MonoBehaviour.print("La armada: " + transform.name + " ha capturado al pirata.");
        cambiarEstado(EstadoArmada.PATRULLANDO);
        Persiguiendo.barcoDestruido();

    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (estadoActual == EstadoArmada.PATRULLANDO && coll.transform != null)
        {
            listaColisiones.Add(coll.transform.gameObject);
        }
        //MonoBehaviour.print("collision detectada por ARMADA");
    }


    public void cancelarPersecucion()
    {

        collisionObject = null;
        Persiguiendo = null;
        if (collisionObject != null && collisionObject.tag == "Comerciante")
        {
            Llamada = collisionObject.GetComponent<Comerciante>();
            MonoBehaviour.print("CANCELADA");
            cambiarEstado(EstadoArmada.ACOMPANA_COMERCIANTE);
        }

    }

}