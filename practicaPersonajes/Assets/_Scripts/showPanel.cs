using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class showPanel : MonoBehaviour
{

    [SerializeField] private Comerciante comerciante;
    [HideInInspector] private Text nameText;
    [HideInInspector] private Text oroText;
    [HideInInspector] private Text maderaText;
    [HideInInspector] private Text comidaText;
    [HideInInspector] private Text tabacoText;
    void Start()
    {
        gameObject.SetActive(false);
        nameText = gameObject.transform.GetChild(0).GetComponent<Text>();
        oroText = gameObject.transform.GetChild(2).GetComponent<Text>();
        maderaText = gameObject.transform.GetChild(4).GetComponent<Text>();
        comidaText = gameObject.transform.GetChild(6).GetComponent<Text>();
        tabacoText = gameObject.transform.GetChild(8).GetComponent<Text>();
    }

    public void setComerciante(Comerciante newComerciante)
    {
        if (comerciante == newComerciante)
        {
            gameObject.SetActive(!gameObject.active);
        }
        else
        {
            nameText.text = newComerciante.gameObject.name;
            gameObject.SetActive(true);
        }
        comerciante = newComerciante;
    }


    // Update is called once per frame
    void Update()
    {
        if (comerciante != null)
        {
            oroText.text = comerciante.getOro().ToString();
            maderaText.text = comerciante.getMadera().ToString();
            comidaText.text = comerciante.getComida().ToString();
            tabacoText.text = comerciante.getTabaco().ToString();
        }
    }
}
