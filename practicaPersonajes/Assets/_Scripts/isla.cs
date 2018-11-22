using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Isla : MonoBehaviour
{
    [HideInInspector] private Vector2 actualPos;

    public int posibleFelicidad(Comerciante comerciante_)
    {
        float distancia = comerciante_.getMovimiento().distance(actualPos);
        //MonoBehaviour.print("madera: " + comerciante_.getMadera() + " oro: " + comerciante_.getOro() + " tabaco: " + comerciante_.getTabaco() + " comida: " + comerciante_.getComida() + " distancia: " + distancia);
        return Mathf.CeilToInt(distancia);
    }

    public void avisarBarcoEsperando(Comerciante comerciante)
    {

    }

    public void comerciarConBarco(Comerciante comerciante)
    {

    }

    void Start()
    {
        actualPos = new Vector2(transform.position.x, transform.position.y);
    }
    public Vector2 getActualPos()
    {
        return actualPos;
    }
}
