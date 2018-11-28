using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;
using UnityEngine.AI;
public class Movimiento
{
    [HideInInspector] private IAstarAI iaAgent;
    [HideInInspector] private float limeteX = 86.0f;
    [HideInInspector] private float limeteY = 44.0f;
    [HideInInspector] private Vector2 initialPos;
    [HideInInspector] private Vector2 objectiveDestination;
    [HideInInspector] private Vector2 huyendoDe;
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
        nextPatroll = new Vector2(Mathf.Clamp(initialPos.x + newPatrolDistance * Mathf.Cos(randomAngle), -limeteX, limeteX), Mathf.Clamp(initialPos.y + newPatrolDistance * Mathf.Sin(randomAngle), -limeteY, limeteY));
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
        Vector2 distanciaHuida = huirDe - actualPos;
        if ((huyendoDe - huirDe).magnitude > 0.15f)
        {
            huyendoDe = huirDe;
            float factorX = (distanciaHuida.normalized.x > 0)
                ? Mathf.Abs((-limeteX - actualPos.x) / distanciaHuida.normalized.x)
                : Mathf.Abs((limeteX - actualPos.x) / distanciaHuida.normalized.x);
            float factorY = (distanciaHuida.normalized.y > 0)
                ? Mathf.Abs((-limeteY - actualPos.y) / distanciaHuida.normalized.y)
                : Mathf.Abs((limeteY - actualPos.y) / distanciaHuida.normalized.y);
            Vector2 v = actualPos - Mathf.Min(factorX, factorY) * distanciaHuida.normalized;
            cambiarDestino(v);
        }
        return distanciaHuida.magnitude;
    }


    private void cambiarDestino(Vector2 newObjective)
    {
        if ((objectiveDestination - newObjective).magnitude > 0.1f)
        {
            //MonoBehaviour.print("Destino cambiado: "+newObjective+" -> "+objectiveDestination);
            objectiveDestination = newObjective;
            iaAgent.destination = new Vector3(newObjective.x, newObjective.y, 0);
            iaAgent.isStopped = false;
            iaAgent.SearchPath();
        }
        else
        {
            iaAgent.isStopped = false;
        }
    }
    public Vector2 getInitialPos()
    {
        return initialPos;
    }
}
