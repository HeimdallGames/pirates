using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class showToPanel : MonoBehaviour {

	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Clicked! " + gameObject.name);
            GameObject panel = GameObject.Find("/Canvas").transform.GetChild(0).gameObject;
            if(panel.GetComponent<showPanel>().comerciante == gameObject.GetComponent<Comerciante>())
            {
                panel.SetActive(false);
            }
            else
            {
                panel.GetComponent<showPanel>().comerciante = gameObject.GetComponent<Comerciante>();
                panel.SetActive(true);
            }
            
        }
    }
}
