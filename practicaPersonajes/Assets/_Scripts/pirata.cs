using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Pirata : MonoBehaviour
{
    public Movimiento movimiento;
    private Vector2 destination;

    //FSM
    private delegate void StateUpdate();
    private StateUpdate stateUpdate;
    void Start()
    {
        NavMeshAgent agent = transform.GetChild(1).GetComponent<NavMeshAgent>();
        movimiento = new Movimiento( agent, transform.GetChild(0) );
    }

    void Update()
    {
        stateUpdate();
    }
}
