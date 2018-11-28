using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class showPanel : MonoBehaviour {
    
    public Comerciante comerciante;

    // Use this for initialization
    void Start () {
        gameObject.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
        if (comerciante != null)
        {
            gameObject.transform.GetChild(2).GetComponent<UnityEngine.UI.Text>().text = comerciante.getOro().ToString();
            gameObject.transform.GetChild(4).GetComponent<UnityEngine.UI.Text>().text = comerciante.getMadera().ToString();
            gameObject.transform.GetChild(6).GetComponent<UnityEngine.UI.Text>().text = comerciante.getComida().ToString();
            gameObject.transform.GetChild(8).GetComponent<UnityEngine.UI.Text>().text = comerciante.getTabaco().ToString();
        }
    }
}
