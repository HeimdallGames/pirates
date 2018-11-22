using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mundo : MonoBehaviour
{
    [SerializeField] public List<Isla> islas;
    public Isla obtenerNuevoDestino(Comerciante comerciante_, Isla ultimaLista)
    {
        islas.Sort((x, y) => (x.posibleFelicidad(comerciante_)).CompareTo(y.posibleFelicidad(comerciante_)));
        int randomIsland = Random.Range(0, 2);
        Isla returnIsla = islas[randomIsland];
        if (ultimaLista == returnIsla)
        {
            if (randomIsland == 0)
            {
                return islas[Random.Range(1, 2)];
            }
            else
            {
                return islas[0];
            }
        }
        else
        {
            return returnIsla;
        }
    }
}
