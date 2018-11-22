using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Movimiento
{
    private NavMeshAgent navMesh;
    private const float extraEndDistance = 7.2f;
    private Transform objectTransform;
    public Movimiento(NavMeshAgent newNavMesh, Transform newTransform)
    {
        navMesh = newNavMesh;
        objectTransform = newTransform;
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
        return navMesh.transform.position;
    }
    public bool updateMovement(Vector2 destination)
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
            objectTransform.position = new Vector3(navMesh.transform.position.x, navMesh.transform.position.y, 0);
            objectTransform.rotation = Quaternion.Euler(0, 0, navMesh.transform.rotation.x * Mathf.Rad2Deg);
            return false;
        }
    }

    public bool huir(Vector2 destination)
    {
        Vector2 distance = 2 * getPos() - destination;
        navMesh.destination = destination;
        navMesh.isStopped = false;
        objectTransform.position = new Vector3(navMesh.transform.position.x, navMesh.transform.position.y, 0);
        objectTransform.rotation = Quaternion.Euler(0, 0, navMesh.transform.rotation.x * Mathf.Rad2Deg);
        return false;
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
