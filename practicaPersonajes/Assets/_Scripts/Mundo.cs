using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Mundo : MonoBehaviour
{
    [SerializeField] public List<Isla> islas;
    [SerializeField] private List<Transform> respawnListObject;
    [SerializeField] private List<Vector3> respawnListPositions;
    [SerializeField] private List<Quaternion> respawnListRotations;
    [SerializeField] private List<int> respawnListEsperas;


    public Isla obtenerNuevoDestino(Comerciante comerciante_, Isla ultimaLista) 
    {
        
            islas.Sort((x, y) => (x.posibleFelicidad(comerciante_)).CompareTo(y.posibleFelicidad(comerciante_)));
            int randomIsland = Random.Range(0, 1);
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
    public void addRespawn(Transform element, Vector3 pos, Quaternion rot, int espera)
    {
        respawnListPositions.Add(pos);
        respawnListRotations.Add(rot);
        respawnListEsperas.Add(espera);
        respawnListObject.Add(element);
    }
    IEnumerator respawn(Transform element, Vector3 elementPosition, Quaternion elementRotation, int espera)
    {
        yield return new WaitForSeconds(espera);

        Transform aux = Instantiate(element, elementPosition, elementRotation);
        if (aux != null)
        {
            Armada armada = aux.GetComponent<Armada>();
            if (armada != null)
            {
                armada.mundo = this;
            }
            else
            {
                Pirata pirata = aux.GetComponent<Pirata>();
                if (pirata != null)
                {
                    pirata.mundo = this;
                    pirata.prefab = element;
                }
                else
                {
                    Comerciante comerciante = aux.GetComponent<Comerciante>();
                    if (comerciante != null)
                    {
                        comerciante.mundo = this;
                        comerciante.prefab = element;
                    }
                }
            }
        }
        else
        {
            MonoBehaviour.print("ERROR");
        }
    }
    void Update()
    {
        if (respawnListObject.Count != 0)
        {
            Transform element = respawnListObject[0];
            respawnListObject.RemoveAt(0);
            int espera = respawnListEsperas[0];
            respawnListEsperas.RemoveAt(0);
            Quaternion elementRotation = respawnListRotations[0];
            respawnListRotations.RemoveAt(0);
            Vector3 elementPosition = respawnListPositions[0];
            respawnListPositions.RemoveAt(0);
            StartCoroutine(respawn(element, elementPosition, elementRotation, espera));
        }
    }
}