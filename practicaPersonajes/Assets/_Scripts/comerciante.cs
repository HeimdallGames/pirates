using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Comerciante : MonoBehaviour
{
    public Movimiento movimiento;

    private Vector2 destination;

    //FSM
    private delegate void StateUpdate();
    private StateUpdate stateUpdate;

    private Isla islaDestino;
    private Pirata huyendoPirata;
    public int oro = 0;
    public int tabaco = 0;
    public int madera = 0;
    public int comida = 0;

    public enum EstadoComerciante { MOVIENDOSE, EN_DESTINO };
   // public enum EstadoComerciante { ESPERAR_COMERCIO, COMERCIANDO, VIAJANDO_OTRA_LISTA, HUIR };
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
