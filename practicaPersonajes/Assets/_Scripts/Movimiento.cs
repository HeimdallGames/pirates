using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;
using UnityEngine.AI;
public class Movimiento
{
    [HideInInspector] private IAstarAI iaAgent;
    [HideInInspector] private float limeteY = 87.0f;
    [HideInInspector] private  float limeteX = 45.0f;
    [HideInInspector] private Vector2 initialPos;
    [HideInInspector] private Vector2 objectiveDestination;
    [SerializeField] private Vector2 nextPatroll;
    [HideInInspector] private float extraEndDistance;
    [HideInInspector] private float extraPatroll;
    [HideInInspector] private float patrolRadius;

    public Movimiento(IAstarAI newIaAgente, float newExtraEndDistance)
    {
        iaAgent = newIaAgente;
        extraEndDistance = newExtraEndDistance;
        objectiveDestination = initialPos = getPos();
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
        nextPatroll = new Vector2(Mathf.Clamp(initialPos.x + newPatrolDistance * Mathf.Cos(randomAngle),-limeteX,limeteX), Mathf.Clamp( initialPos.y + newPatrolDistance * Mathf.Sin(randomAngle),-limeteY,limeteY));
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
            cambiarDestino(nextPatroll);
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
            cambiarDestino(destination);
            return false;
        }
    }

    public float huir(Vector2 huirDe)
    {
        Vector2 actualPos = getPos();
        Vector2 destination = actualPos+actualPos-huirDe;
        destination = new Vector2(Mathf.Clamp(destination.x, -limeteX, limeteX),Mathf.Clamp(actualPos.y, -limeteY, limeteY));
        cambiarDestino(destination);
        return (destination-actualPos).magnitude;
    }

    private void cambiarDestino(Vector2 newObjective)
    {
        if((objectiveDestination-newObjective).magnitude > 0.1f)
        {
            //MonoBehaviour.print("Destino cambiado: "+newObjective+" -> "+objectiveDestination);
            objectiveDestination = newObjective;        
            iaAgent.destination = new Vector3(newObjective.x,newObjective.y,0);
            iaAgent.isStopped = false;
            iaAgent.SearchPath();
        }
        else{
            iaAgent.isStopped = false;
        }
    }
    public Vector2 getInitialPos(){
        return initialPos;
    }
}
