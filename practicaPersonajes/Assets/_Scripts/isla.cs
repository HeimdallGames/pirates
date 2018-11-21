using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Isla : MonoBehaviour
{
    public Movimiento movimiento;
    public int posibleFelicidad(Comerciante comerciante_){
        float distancia = movimiento.distance(comerciante_.movimiento.getPos());
        MonoBehaviour.print("madera: "+comerciante_.madera+" oro: "+comerciante_.oro+" tabaco: "+comerciante_.tabaco+" comida: "+comerciante_.comida+" distancia: "+distancia);
        return Mathf.CeilToInt(distancia);
    }

    void Start()
    {
        //Rigidbody2D rigidbody = transform.GetComponent<Rigidbody2D>();
        //BoxCollider2D collider = transform.GetComponent<BoxCollider2D>();
        //movimiento = new Movimiento(rigidbody, collider, 18.4f);
    }

    void Update()
    {

    }
}
