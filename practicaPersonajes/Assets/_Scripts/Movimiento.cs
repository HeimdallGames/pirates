using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Movimiento
{
    [HideInInspector] private NavMeshAgent navMesh;
    [HideInInspector] private Vector2 initialPos;
    [HideInInspector] private Vector2 lastPos;
    [SerializeField] private Vector2 nextPatroll;
    [HideInInspector] private float extraEndDistance;
    [HideInInspector] private float extraPatroll;
    [HideInInspector] private float patrolRadius;
    [HideInInspector] private Transform objectTransform;

    public Movimiento(NavMeshAgent newNavMesh, Transform newTransform, float newExtraEndDistance)
    {
        navMesh = newNavMesh;
        objectTransform = newTransform;
        extraEndDistance = newExtraEndDistance;
        lastPos = initialPos = getPos();
    }
    public Movimiento(NavMeshAgent newNavMesh, Transform newTransform, float newExtraEndDistance, float newPatrolRadius, float newExtraPatroll)
    : this(newNavMesh, newTransform, newExtraEndDistance)
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
        navMesh.isStopped = true;
    }

    public float distance(Vector2 destination)
    {
        return (destination - getPos()).magnitude;
    }
    public Vector2 getPos()
    {
        return new Vector2(navMesh.transform.position.x, navMesh.transform.position.y);
    }

    public void patrullar()
    {
        if (distance(nextPatroll) < extraPatroll)
        {
            getNewPatroll();
        }
        else
        {
            navMesh.destination = nextPatroll;
            navMesh.isStopped = false;
            objectTransform.position = new Vector3(navMesh.transform.position.x, navMesh.transform.position.y, 0);
            objectTransform.rotation = Quaternion.Euler(0, 0, getRotation());
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
            navMesh.destination = destination;
            navMesh.isStopped = false;
            objectTransform.position = new Vector3(navMesh.transform.position.x, navMesh.transform.position.y, 0);
            objectTransform.rotation = Quaternion.Euler(0, 0, getRotation());
            return false;
        }
    }

    public float huir(Vector2 destination)
    {
        Vector2 distance = 2 * getPos() - destination;
        navMesh.destination = destination;
        navMesh.isStopped = false;
        objectTransform.position = new Vector3(navMesh.transform.position.x, navMesh.transform.position.y, 0);
        objectTransform.rotation = Quaternion.Euler(0, 0, getRotation());
        return distance.magnitude;
    }

    public static float getAngle(Vector2 v1)
    {
        if (v1.x == 0.0f || v1.y == 0.0f)
        {
            return 0.0f;
        }
        else
        {
            float angle = Mathf.Atan(Mathf.Abs(v1.y / v1.x)) * Mathf.Rad2Deg;
            if (v1.y < 0.0f)
            {
                if (v1.x < 0.0f)
                {
                    return angle + 90;
                }
                else
                {
                    return angle + 180;
                }
            }
            else if (v1.x < 0.0f)
            {
                return angle;
            }
            else
            {
                return angle + 270;
            }

        }
    }

    public Vector2 getInitialPos(){
        return initialPos;
    }
    public float getRotation()
    {
        Vector2 actualPos = getPos();
        Vector2 velocity = lastPos - actualPos;
        float velocityAngle = getAngle(velocity);
        lastPos = actualPos;
        return velocityAngle;
    }
}
