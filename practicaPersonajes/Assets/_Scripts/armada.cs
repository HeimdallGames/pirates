using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Armada : MonoBehaviour
{
    public Movimiento movimiento;

    //FSM
    private delegate void StateUpdate();
    private StateUpdate stateUpdate;
    public enum EstadoArmada { PATRULLANDO, AYUDA, PERSIGUE, ACOMPANA_COMERCIANTE };
    [SerializeField] private EstadoArmada estadoActual;
    void Start()
    {
        NavMeshAgent agent = transform.GetChild(1).GetComponent<NavMeshAgent>();
        movimiento = new Movimiento( agent, transform.GetChild(0) );
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
    public Movimiento getMovimiento(){
        return movimiento;
    }
    
    private void updatePatrullando(){
        //todo
    }
    private void updateAyudando(){
        //todo
    }
    private void updatePersiguiendo(){
        //todo
    }
    private void updateAcompanando(){
        //todo
    }

}
