using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;
using UnityEngine.AI;
public class Movimiento
{
    [HideInInspector] private IAstarAI iaAgent;
    [HideInInspector] private Vector2 initialPos;
    [HideInInspector] private Vector2 lastPos;
    [SerializeField] private Vector2 nextPatroll;
    [HideInInspector] private float extraEndDistance;
    [HideInInspector] private float extraPatroll;
    [HideInInspector] private float patrolRadius;

    public Movimiento(IAstarAI newIaAgente, float newExtraEndDistance)
    {
        iaAgent = newIaAgente;
        extraEndDistance = newExtraEndDistance;
        lastPos = initialPos = getPos();
    }
    public Movimiento(IAstarAI newIaAgente, float newExtraEndDistance, float newPatrolRadius, float newExtraPatroll)
    : this(newIaAgente, newExtraEndDistance)
    {
        patrolRadius = newPatrolRadius;
        extraPatroll = newExtraPatroll;
        getNewPatroll();
    }

    private void getNewPatroll()
    {
        float newPatrolDistance = Random.Range(0.0f, patrolRadius);
        float randomAngle = Random.Range(0.0f, Mathf.PI + Mathf.PI); ;
        nextPatroll = new Vector2(initialPos.x + newPatrolDistance * Mathf.Cos(randomAngle), initialPos.y + newPatrolDistance * Mathf.Sin(randomAngle));
        //MonoBehaviour.print("N:"+nextPatroll);
    }
    public void stopMovement()
    {
        iaAgent.isStopped = true;
    }

    public float distance(Vector2 destination)
    {
        return (destination - getPos()).magnitude;
    }
    public Vector2 getPos()
    {
        return new Vector2(iaAgent.position.x, iaAgent.position.y);
    }

    public void patrullar()
    {
        if (distance(nextPatroll) < extraPatroll)
        {
            getNewPatroll();
        }
        else
        {
            iaAgent.isStopped = false;        
            iaAgent.destination = new Vector3(nextPatroll.x,nextPatroll.y,0);
            iaAgent.SearchPath();
        }
    }
    public bool updateMovement(Vector2 destination)
    {
        if (distance(destination) < extraEndDistance)
        {
            stopMovement();
            return true;
        }
        else
        {
            iaAgent.isStopped = false;        
            iaAgent.destination = new Vector3(destination.x,destination.y,0);
            iaAgent.SearchPath();
            return false;
        }
    }

    public float huir(Vector2 destination)
    {
        Vector2 distance = 2 * getPos() - destination;
        iaAgent.isStopped = false;        
        iaAgent.destination = new Vector3(destination.x,destination.y,0);
        iaAgent.SearchPath();
        return distance.magnitude;
    }
    public Vector2 getInitialPos(){
        return initialPos;
    }
}
