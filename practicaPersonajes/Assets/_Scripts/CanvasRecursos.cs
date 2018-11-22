using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasRecursos
{

    private Text textOro;
    private Text textMadera;
    private Text textTabaco;
    private Text textComida;
    public CanvasRecursos(Canvas canvas)
    {
        textOro = canvas.transform.GetChild(0).GetComponent<Text>();
        textMadera = canvas.transform.GetChild(1).GetComponent<Text>();
        textTabaco = canvas.transform.GetChild(2).GetComponent<Text>();
        textComida = canvas.transform.GetChild(3).GetComponent<Text>();
    }
    public CanvasRecursos(Canvas canvas, int oro, int madera, int tabaco, int comida)
    : this(canvas)
    {
        setOro(oro);
        setMadera(madera);
        setTabaco(tabaco);
        setComida(comida);
    }
    private void setText(Text text, int value)
    {
        text.text = value.ToString();
    }
    public void setOro(int value)
    {
        setText(textOro, value);
    }
    public void setMadera(int value)
    {
        setText(textMadera, value);
    }
    public void setTabaco(int value)
    {
        setText(textTabaco, value);
    }
    public void setComida(int value)
    {
        setText(textComida, value);
    }
}
