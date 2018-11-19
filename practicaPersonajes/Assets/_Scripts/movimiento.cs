using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Movimiento
{
    private float celerity;
    Rigidbody2D rigidbody;
    BoxCollider2D collider;
    private const float extraEndDistance = 3.2f;
    public Movimiento(Rigidbody2D newRigidbody, BoxCollider2D newCollider, float newCelerity)
    {
        rigidbody = newRigidbody;
        collider = newCollider;
        celerity = newCelerity;
        rigidbody.velocity = Vector2.zero;
    }

    public void stopMovement()
    {
        rigidbody.velocity = Vector2.zero;
        rigidbody.angularVelocity = 0;
        rigidbody.inertia = 0;
    }

    public bool updateMovement(float deltaTime, Vector2 destination)
    {
        Vector2 distance = destination - rigidbody.position;
        if (distance.magnitude < extraEndDistance)
        {
            stopMovement();
            rigidbody.position = destination;
            return true;
        }
        else
        {
            rigidbody.velocity = distance.normalized * celerity;
            rigidbody.rotation = getAngle(distance) * Mathf.Rad2Deg;
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
