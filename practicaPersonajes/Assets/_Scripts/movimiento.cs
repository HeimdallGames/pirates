using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Movimiento
{
    private NavMeshAgent navMesh;
    private const float extraEndDistance = 7.2f;
    public Movimiento(NavMeshAgent newNavMesh)
    {
        navMesh = newNavMesh;
    }

    public void stopMovement()
    {
        navMesh.isStopped = true;
    }

    public float distance( Vector2 destination){
        return ( destination - getPos()).magnitude;
    }
    public Vector2 getPos(){
        return navMesh.transform.position;
    }
    public bool updateMovement(float deltaTime, Vector2 destination)
    {
        Vector2 distance = destination - getPos();
        if (distance.magnitude < extraEndDistance)
        {
            stopMovement();
            return true;
        }
        else
        {
            navMesh.destination = destination;
            navMesh.isStopped = false;
            return false;
        }
    }

    public static float getAngle(Vector2 v1)
    {
        if (v1.x == 0.0f || v1.y == 0.0f)
        {
            return 0.0f;
        }
        else if (v1.y < 0.0f)
        {
            return -Mathf.Abs(Mathf.Atan(v1.y / v1.x));
        }
        else
        {
            return Mathf.Abs(Mathf.Atan(v1.y / v1.x));
        }
    }
}
