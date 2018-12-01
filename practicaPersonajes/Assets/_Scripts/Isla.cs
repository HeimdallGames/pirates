using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Isla : MonoBehaviour
{
    [HideInInspector] private Vector2 actualPos;
    public Mundo mundo;
    private float tiempo = 0;

    public enum tipos { MADERA, TABACO, COMIDA, COMERCIO };
    public tipos tipoActual;

    public Sprite islaCom;
    public Sprite islaRec;
    [SerializeField] private bool islaComercio;

    private int tipoIslaActual = -1;

    //Recursos
    [SerializeField] private int oro = 1000;
    [SerializeField] private int madera = 400;
    [SerializeField] private int tabaco = 400;
    [SerializeField] private int comida = 400;

    private int valorEstandar = 10;
    private int valorAlto = 15;
    private int valorBajo = 5;

    [SerializeField] private int precioMadera;
    [SerializeField] private int precioComida;
    [SerializeField] private int precioTabaco;

    [SerializeField] private int gastoMadera;
    [SerializeField] private int gastoComida;
    [SerializeField] private int gastoTabaco;

    private int necesidadEstandar = 400;
    private int necesidadAlta = 200;


    IEnumerator esperar(int tiempo, Comerciante comerciante)
    {
        yield return new WaitForSeconds(tiempo);
        comerciante.setEspera(false);
    }

    public int posibleFelicidad(Comerciante comerciante_)
    {
        float felicidad, beneficios = 0;
        float distancia = comerciante_.getMovimiento().distance(actualPos);

        if (comerciante_.getMadera() != 0 || comerciante_.getTabaco() != 0 || comerciante_.getComida() != 0)
            beneficios = comerciante_.getMadera() * precioMadera + comerciante_.getTabaco() * precioTabaco + comerciante_.getComida() * precioComida;
        else
        {
            if (tipoActual != tipos.COMERCIO)
                beneficios = 1000000;
        }
        felicidad = (40 * distancia + 60 * beneficios) / 100;
        return Mathf.CeilToInt(felicidad);
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            MonoBehaviour.print("click" + gameObject.name);
            islaComercio = !islaComercio;
        }
    }

    public void avisarBarcoEsperando(Comerciante comerciante)
    {
        //wait 3 s
        comerciante.setEspera(true);
        StartCoroutine(esperar(3, comerciante));
    }

    public void obtenerRecursos(Comerciante comerciante)
    {
        switch (tipoActual)
        {
            case tipos.COMIDA:
                comida -= 500;
                comerciante.setComida(comerciante.getComida() + 500);
                break;
            case tipos.TABACO:
                tabaco -= 500;
                comerciante.setTabaco(comerciante.getTabaco() + 500);
                break;
            case tipos.MADERA:
                madera -= 500;
                comerciante.setMadera(comerciante.getMadera() + 500);
                break;
        }
        comerciante.desvalijado = false;
    }

    public void comerciarConBarco(Comerciante comerciante)
    {
        if (tipoActual == tipos.COMERCIO)
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
                comerciante.setComida(0);
            }
        }
        else { obtenerRecursos(comerciante); }

    }

    void Update()
    {
        if (islaComercio)
        {
            if (this.tipoActual != tipos.COMERCIO)
            {
                this.gameObject.GetComponent<SpriteRenderer>().sprite = islaCom;
                reasignarIsla();
            }
            UpdateComercio();
        }
        else
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = islaRec;

            if (this.tipoActual == tipos.COMERCIO)
            {
                asignarMaterial(-1);
            }
            UpdateRecursos();
        }
    }

    void Start()
    {
        this.gameObject.GetComponent<SpriteRenderer>().sprite = null;

        if (islaComercio)
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = islaCom;
            reasignarIsla();
        }
        else
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = islaRec;
            asignarMaterial(-1);
        }

        actualPos = new Vector2(transform.position.x, transform.position.y);
        precioMadera = valorEstandar;
        precioComida = valorEstandar;
        precioTabaco = valorEstandar;

        asignarMaterial(-1);
    }



    public void asignarMaterial(int material)
    {
        if (material == -1)
            tipoIslaActual = Random.Range(0, 3);
        else
            tipoIslaActual = material;


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
        oro = 5000;
        madera = 400;
        tabaco = 400;
        comida = 400;
        gastoComida = 20;
        gastoMadera = 20;
        gastoTabaco = 20;
        tipoActual = tipos.COMERCIO;
    }


    private void UpdateComercio()
    {

        if (tiempo > 1)
        {
            oro += 100;

            if (comida - gastoComida >= 0)
                comida -= gastoComida;
            if (tabaco - gastoTabaco >= 0)
                tabaco -= gastoTabaco;
            if (madera - gastoMadera >= 0)
                madera -= gastoMadera;

            //reasignar precio comida en funcion de la necesida
            if (comida < necesidadAlta)
            {
                precioComida = valorAlto;
            }
            else if (comida <= necesidadEstandar)
                precioComida = valorEstandar;
            else
            {
                precioComida = valorBajo;
            }
            //reasignar precio tabaco en funcion de la necesida
            if (tabaco < necesidadAlta)
            {
                precioTabaco = valorAlto;
            }
            else if (tabaco <= necesidadEstandar)
                precioTabaco = valorEstandar;
            else
            {
                precioTabaco = valorBajo;
            }
            //reasignar precio madera en funcion de la necesidad
            if (madera < necesidadAlta)
            {
                precioMadera = valorAlto;
            }
            else if (madera <= necesidadEstandar)
                precioMadera = valorEstandar;
            else
            {
                precioMadera = valorBajo;
            }
            tiempo = 0;
        }


        tiempo += UnityEngine.Time.deltaTime;
    }


    private void UpdateRecursos()
    {
        if (tiempo > 1)
        {
            switch (tipoActual)
            {
                case tipos.COMIDA:
                    if (comida < 3000)
                    {
                        comida += 100;
                    }
                    break;
                case tipos.MADERA:
                    if (madera < 3000)
                    {
                        madera += 100;
                    }
                    break;
                case tipos.TABACO:
                    if (tabaco < 3000)
                    {
                        tabaco += 100;
                    }
                    break;
            }

            tiempo = 0;
        }

        tiempo += UnityEngine.Time.deltaTime;
    }
}
