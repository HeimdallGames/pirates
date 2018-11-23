using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Isla : MonoBehaviour
{
    [HideInInspector] private Vector2 actualPos;
	public Mundo mundo;

	public int tipoIslaActual = -1;

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
        float distancia = comerciante_.getMovimiento().distance(actualPos);
        //MonoBehaviour.print("madera: " + comerciante_.getMadera() + " oro: " + comerciante_.getOro() + " tabaco: " + comerciante_.getTabaco() + " comida: " + comerciante_.getComida() + " distancia: " + distancia);
        return Mathf.CeilToInt(distancia);
    }

    public void avisarBarcoEsperando(Comerciante comerciante)
    {

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
    }


    void Start()
    {	
        actualPos = new Vector2(transform.position.x, transform.position.y);
		precioMadera = valorEstandar;
		precioComida = valorEstandar;
		precioTabaco = valorEstandar;
		//asignarMaterial ();
    }


	/*
	public void asignarMaterial()
	{
		tipoIslaActual = Random.Range (0, 2);;

		switch (tipoIslaActual) 
		{
		case 0:
			comida = 1000;
			break;
		case 1:
			tabaco = 1000;
			break;
		case 2:
			madera = 1000;
			break;
		}
	}
*/
    public Vector2 getActualPos()
    {
        return actualPos;
    }


}
