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
        List<Isla> islasClon = new List<Isla> (islas);
        
        islasClon.Sort((y,x ) => (x.posibleFelicidad(comerciante_)).CompareTo(y.posibleFelicidad(comerciante_)));

        /*foreach (Isla isla in islasClon)
        {
            Debug.Log("Isla " +  isla.transform.name + " para comerciante " + comerciante_.name + " da " + isla.posibleFelicidad(comerciante_) + " felicidad");
        }*/

        int randomIsland = Random.Range(0, 1);
        Isla returnIsla = islasClon[randomIsland];
            
        if (ultimaLista == returnIsla)
        {
            if (randomIsland == 0)
            {
                returnIsla = islasClon[1];     
            }
            else
            {
                returnIsla = islasClon[0];
            }
        }

       // Debug.Log("Comerciante " + comerciante_.name + " viaja a " + returnIsla.transform.name + " por " + returnIsla.posibleFelicidad(comerciante_) + " felicidad.");

        returnIsla.aumentarCompetencia();
        return returnIsla;
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