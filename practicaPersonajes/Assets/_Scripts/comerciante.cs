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
        movimiento = new Movimiento(transform.position, transform.rotation.eulerAngles.z, 8.4f, 0.8f);
        destination = new Vector2(-46.5f, -30.3f);
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
        transform.rotation = movimiento.getRotation();
        transform.position = movimiento.getPosition();
    }
}
