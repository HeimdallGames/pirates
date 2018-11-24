using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Isla : MonoBehaviour
{
    [HideInInspector] private Vector2 actualPos;
	public Mundo mundo;

	public enum tipos {MADERA,TABACO,COMIDA,COMERCIO};
	public tipos tipoActual;

	public Sprite islaCom;
	public Sprite islaRec;
	public bool islaComercio;

	private int tipoIslaActual = -1;

	//Recursos
	public int oro = 1000;
	public int madera = 400;
	public int tabaco = 400;
	public int comida = 400;

	private int valorEstandar = 10;
	private int valorAlto = 15;
	private int valorBajo = 5;

	public int precioMadera;
	public int precioComida;
	public int precioTabaco;

	private int necesidadEstandar = 400;
	private int necesidadAlta = 200;
	private int necesidadBaja = 800;

    public int posibleFelicidad(Comerciante comerciante_)
    {
		float felicidad,beneficios = 0;
        float distancia = comerciante_.getMovimiento().distance(actualPos);
        //MonoBehaviour.print("madera: " + comerciante_.getMadera() + " oro: " + comerciante_.getOro() + " tabaco: " + comerciante_.getTabaco() + " comida: " + comerciante_.getComida() + " distancia: " + distancia);
		beneficios = comerciante_.getMadera() * precioMadera + comerciante_.getTabaco() * precioTabaco + comerciante_.getComida() * precioComida;
		felicidad = (40*distancia + 60*beneficios)/100;
		Debug.Log (felicidad + " felicidad y " + beneficios + " beneficios");
		return Mathf.CeilToInt(distancia);
    }

    public void avisarBarcoEsperando(Comerciante comerciante)
    {
		//wait 3 s
		comerciante.setEspera(false);
    }

    public void comerciarConBarco(Comerciante comerciante)
    {
		//COMERCIO MADERA	
		if (comerciante.getMadera() > 0) 
		{
			this.madera += comerciante.getMadera();
			comerciante.setOro(comerciante.getMadera() * precioMadera);
			this.oro -= comerciante.getMadera() * precioMadera;
			comerciante.setMadera(0); 
		}

		//COMERCIO TABACO
		if (comerciante.getTabaco() > 0) 
		{
			this.tabaco += comerciante.getTabaco();
			comerciante.setOro(comerciante.getTabaco() * precioTabaco);
			this.oro -= comerciante.getTabaco() * precioTabaco;
			comerciante.setTabaco(0);
		}
		//COMERCIO COMIDA ??
		if (comerciante.getComida() > 0) 
		{
			this.comida += comerciante.getComida();
			comerciante.setOro(comerciante.getComida() * precioComida);
			this.oro -= comerciante.getComida() * precioComida;
			comerciante.setComida (0);
		}

		//volver a esperar en otra isla hasta que pueda comerciar
		comerciante.setEspera(true);
    }

	void Update()
	{
		if (islaComercio) {
			this.gameObject.GetComponent<SpriteRenderer> ().sprite = islaCom;
			reasignarIsla ();
		} else {
			this.gameObject.GetComponent<SpriteRenderer> ().sprite = islaRec;

			if (this.tipoActual == tipos.COMERCIO) 
			{
				asignarMaterial ();
			}

		}
	}

    void Start()
    {	
		this.gameObject.GetComponent<SpriteRenderer> ().sprite = null;

		if (islaComercio) {
			this.gameObject.GetComponent<SpriteRenderer> ().sprite = islaCom;
			reasignarIsla ();
		} else {
			this.gameObject.GetComponent<SpriteRenderer> ().sprite = islaRec;
			asignarMaterial ();
		}
			
        actualPos = new Vector2(transform.position.x, transform.position.y);
		precioMadera = valorEstandar;
		precioComida = valorEstandar;
		precioTabaco = valorEstandar;

		asignarMaterial ();
    }



	public void asignarMaterial()
	{
		tipoIslaActual = Random.Range (0, 3);;

		switch (tipoIslaActual) 
		{
		case 0:
			comida = 1000;
			tabaco = 0;
			madera = 0;
			tipoActual = tipos.COMIDA;
			break;
		case 1:
			comida = 0;
			tabaco = 1000;
			madera = 0;
			tipoActual = tipos.TABACO;
			break;
		case 2:
			comida = 0;
			tabaco = 0;
			madera = 1000;
			tipoActual = tipos.MADERA;
			break;
		}
	}

    public Vector2 getActualPos()
    {
        return actualPos;
    }

	private void reasignarIsla()
	{
		oro = 1000;
		madera = 400;
		tabaco = 400;
		comida = 400;
		tipoActual = tipos.COMERCIO;
	}
}
