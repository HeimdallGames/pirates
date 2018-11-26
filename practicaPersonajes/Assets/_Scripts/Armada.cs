using System.Collections;
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
    public enum EstadoArmada { PATRULLANDO, AYUDA, PERSIGUE, ACOMPANA_COMERCIANTE };
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
            Persiguiendo.detectadoPorArmada(this);
            //cambiarEstado(EstadoArmada.PERSIGUE);
        }
        else
        {
            movimiento.patrullar();
        }
    }
    private void updateAyudando()
    {
        //todo
    }
    private void updatePersiguiendo()
    {
        //todo
    }
    private void updateAcompanando()
    {
        //todo
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (estadoActual == EstadoArmada.PATRULLANDO && coll.transform != null)
        {
            listaColisiones.Add(coll.transform.gameObject);
        }
        MonoBehaviour.print("collision detectada ARMADA");
    }

}