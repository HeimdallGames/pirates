using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Pirata : MonoBehaviour
{
    [HideInInspector] private Movimiento movimiento;
    [SerializeField] private float extraEndDistance = 7.2f;
    [SerializeField] private float extraPatroll = 7.2f;
    [SerializeField] private float patrollRadius = 22.2f;

    public enum EstadoPirata { ESPERAR_BARCO, ATACAR, HUIR, CONSEGUIR_BOTIN };
    [SerializeField] private EstadoPirata estadoActual;

    //FSM
    private delegate void StateUpdate();
    private StateUpdate stateUpdate;
    void Start()
    {
        NavMeshAgent agent = transform.GetChild(1).GetComponent<NavMeshAgent>();
        movimiento = new Movimiento(agent, transform.GetChild(0), extraEndDistance, patrollRadius, extraPatroll);
        cambiarEstado(EstadoPirata.ESPERAR_BARCO);
    }

    void Update()
    {
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
        }
        estadoActual = nuevoEstado;
    }
    public Movimiento getMovimiento()
    {
        return movimiento;
    }

    private void updateAtacando()
    {
        //todo
    }
    private void updateHuyendo()
    {
        //todo
    }
    private void updateEsperandoBarco()
    {
        movimiento.patrullar();
    }
    private void updateConsiguiendoBotin()
    {
        //todo
    }
}
