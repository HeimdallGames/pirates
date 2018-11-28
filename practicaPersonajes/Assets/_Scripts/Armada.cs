using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;
using UnityEngine.AI;

public class Armada : MonoBehaviour
{
    [HideInInspector] public Movimiento movimiento;
    [SerializeField] public Mundo mundo;
    [SerializeField] private float extraEndDistance = 7.2f;
    [SerializeField] private float extraPatroll = 7.2f;
    [SerializeField] private float patrollRadius = 29.2f;

    [SerializeField] private Comerciante llamada;
    [SerializeField] private Pirata persiguiendo;
    [SerializeField] private List<GameObject> listaColisiones;
    [SerializeField] private GameObject collisionObject;

    //FSM
    private delegate void StateUpdate();
    private StateUpdate stateUpdate;
    public enum EstadoArmada { PATRULLANDO, PERSIGUE, ATRAPAR, ACOMPANA_COMERCIANTE };
    [SerializeField] private EstadoArmada estadoActual;
    void Start()
    {
        IAstarAI agent = transform.GetComponent<IAstarAI>();
        movimiento = new Movimiento(agent, extraEndDistance, patrollRadius, extraPatroll);
        cambiarEstado(EstadoArmada.PATRULLANDO);
        collisionObject = null;
    }

    void Update()
    {
        stateUpdate();
    }

    public void cambiarEstado(EstadoArmada nuevoEstado)
    {
        switch (nuevoEstado)
        {
            case EstadoArmada.PATRULLANDO:
                stateUpdate = updatePatrullando;
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
        Comerciante newLlamada = comprobarUltimaColision();
        if (newLlamada == null)
        {
            movimiento.patrullar();
        }
        else
        {
            llamada = newLlamada;
        }
    }
    private void updatePersiguiendo()
    {
        if (movimiento.updateMovement(persiguiendo.getMovimiento().getPos()))
        {
            cambiarEstado(EstadoArmada.ATRAPAR);
        }
    }
    private void updateAcompanando()
    {
        Comerciante newComerciante = comprobarUltimaColision();
        if (newComerciante != null)
        {
            llamada = newComerciante;
            cambiarEstado(EstadoArmada.PATRULLANDO);
        }
        else
        {
            movimiento.updateMovement(llamada.getMovimiento().getPos());
        }
    }

    private void updateAtraparPirata()
    {
        MonoBehaviour.print("La armada: " + transform.name + " ha capturado al pirata.");
        persiguiendo.barcoDestruido();
        cancelarPersecucion();
    }

    public Comerciante comprobarUltimaColision()
    {
        if (listaColisiones.Count > 0)
        {
            collisionObject = listaColisiones[0];
            listaColisiones.RemoveAt(0);

            Comerciante newLlamada = collisionObject.GetComponent<Comerciante>();
            persiguiendo = newLlamada.perseguidoPor();
            if (persiguiendo != null)
            {
                newLlamada.avisarEsSalvado(this);
                persiguiendo.detectadoPorArmada(this);
                cambiarEstado(EstadoArmada.PERSIGUE);
                return newLlamada;
            }
            else
            {
                collisionObject = null;
                return null;
            }
        }
        else
        {
            collisionObject = null;
            return null;
        }
    }
    void OnTriggerStay2D(Collider2D coll)
    {
        if (coll.transform != null && coll.transform.gameObject.tag == "Comerciante"
            && !listaColisiones.Contains(coll.transform.gameObject))
        {
            listaColisiones.Add(coll.transform.gameObject);
        }
        //MonoBehaviour.print("collision detectada por ARMADA");
    }

    //COMUNICACION
    public void cancelarPersecucion()
    {
        persiguiendo = null;
        if (llamada != null)
        {
            cambiarEstado(EstadoArmada.ACOMPANA_COMERCIANTE);
        }
        else
        {
            cambiarEstado(EstadoArmada.PATRULLANDO);
        }
    }

    public void dejarDeAcompanar()
    {
        if (estadoActual == EstadoArmada.ACOMPANA_COMERCIANTE)
        {
            cambiarEstado(EstadoArmada.PATRULLANDO);
        }
        llamada = null;
    }
}