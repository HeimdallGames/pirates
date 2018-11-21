using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Isla : MonoBehaviour
{
    public Vector2 actualPos;
    public int posibleFelicidad(Comerciante comerciante_){
        float distancia = comerciante_.movimiento.distance(actualPos);
        MonoBehaviour.print("madera: "+comerciante_.madera+" oro: "+comerciante_.oro+" tabaco: "+comerciante_.tabaco+" comida: "+comerciante_.comida+" distancia: "+distancia);
        return Mathf.CeilToInt(distancia);
    }

    void Start()
    {
        actualPos = new Vector2(transform.position.x,transform.position.y);
    }

    void Update()
    {

    }
}
