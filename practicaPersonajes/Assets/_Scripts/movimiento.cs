using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Movimiento
{

    private Vector2 actualPos;
    private Vector2 speed;
    private float celerity;
    private float angularCelerity;
    private const float extraEndDistance = 1.2f;
    public Movimiento(Vector3 initialPos, float initialAngle, float newCelerity, float newAngularCelerity)
    {
        celerity = newCelerity;
        angularCelerity = newAngularCelerity;
        actualPos = new Vector2(initialPos.x, initialPos.y);
        float radAngle = initialAngle * Mathf.Deg2Rad;
        float cos = Mathf.Cos(radAngle);
        float sin = Mathf.Sin(radAngle);
        speed = new Vector2(celerity * (cos - sin), celerity * (cos + sin));
    }

    public float distance(Vector2 posibleDestination)
    {
        return Vector2.Distance(actualPos, posibleDestination);
    }

    public bool updateMovement(float deltaTime, Vector2 destination)
    {
        //todo evitar islas y obstaculos
        Vector2 distance = destination - actualPos;
        if (distance.magnitude < extraEndDistance)
        {
            actualPos = destination;
            return true;
        }
        else
        {
            float angle = getAngle(speed, distance);
            if (angle < 0.0f)
            {
                angle = Mathf.Max(angle, -angularCelerity * deltaTime);
            }
            else if (angle > 0.0f)
            {
                angle = Mathf.Min(angle, angularCelerity * deltaTime);
            }
            float cos = Mathf.Cos(angle);
            float sin = Mathf.Sin(angle);
            speed = new Vector2(speed.x * cos - speed.y * sin, speed.y * cos + speed.x * sin);
            actualPos += speed * deltaTime;
            return false;
        }
    }

    public static float getAngle(Vector2 v1, Vector2 v2)
    {
        return getAngle(v1) - getAngle(v2);
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
    public Quaternion getRotation()
    {
        return Quaternion.AngleAxis(getAngle(speed) * Mathf.Rad2Deg, Vector3.forward);
    }
    public Vector3 getPosition()
    {
        return new Vector3(actualPos.x, actualPos.y, 0.0f);
    }
}
