using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Comerciante : MonoBehaviour
{
    Movimiento movimiento;
    Vector2 destination;

    //FSM
    private delegate void StateUpdate();
    private StateUpdate stateUpdate;

    public enum EstadoComerciante { MOVIENDOSE, EN_DESTINO };
    private EstadoComerciante estadoActual;
    void Start()
    {
        cambiarEstado(EstadoComerciante.MOVIENDOSE);
        Rigidbody2D rigidbody = transform.GetComponent<Rigidbody2D>();
        BoxCollider2D collider = transform.GetComponent<BoxCollider2D>();
        movimiento = new Movimiento(rigidbody, collider, 18.4f);
        destination = new Vector2(-32f, -20f);
    }

    void Update()
    {
        stateUpdate();
    }
    public void cambiarEstado(EstadoComerciante nuevoEstado)
    {
        switch (nuevoEstado)
        {
            case EstadoComerciante.MOVIENDOSE:
                stateUpdate = updateMoviendose;
                break;
            case EstadoComerciante.EN_DESTINO:
                stateUpdate = updateEnDestino;
                break;
        }
        estadoActual = nuevoEstado;
    }

    void updateEnDestino()
    {
        MonoBehaviour.print("Estoy en el destino.");
    }
    void updateMoviendose()
    {
        if (movimiento.updateMovement(Time.deltaTime, destination))
        {
            MonoBehaviour.print("LLegado a destino.");
            cambiarEstado(EstadoComerciante.EN_DESTINO);
        }
    }
}
