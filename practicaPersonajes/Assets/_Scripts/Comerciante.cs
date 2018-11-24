using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Comerciante : MonoBehaviour
{
    [HideInInspector] private Movimiento movimiento;
    [SerializeField] private float extraEndDistance = 8.2f;
    [SerializeField] public Mundo mundo;

    //FSM
    private delegate void StateUpdate();
    private StateUpdate stateUpdate;
    public enum EstadoComerciante { ESPERAR_COMERCIO, COMERCIANDO, BUSCANDO_ISLA, VIAJANDO_OTRA_LISTA, HUIR, ATRACADO };
    [SerializeField] private EstadoComerciante estadoActual;

    [SerializeField] private Isla islaDestino;
    [SerializeField] private Pirata huyendoPirata;
    private bool esperando;

    //Recursos
    [SerializeField] private int oro = 0;

    [SerializeField] private int madera = 0;
    [SerializeField] private int tabaco = 0;
    [SerializeField] private int comida = 0;
    [HideInInspector] private CanvasRecursos canvasRecursos;

    void Start()
    {
        cambiarEstado(EstadoComerciante.BUSCANDO_ISLA);
        islaDestino = mundo.islas[0];
        NavMeshAgent agent = transform.GetChild(1).GetComponent<NavMeshAgent>();
        movimiento = new Movimiento(agent, transform.GetChild(0), extraEndDistance);
        Canvas canvas = transform.GetChild(0).GetChild(0).GetComponent<Canvas>();
        canvasRecursos = new CanvasRecursos(canvas, oro, madera, tabaco, comida);
        esperando = true;
    }

    void Update()
    {
        stateUpdate();
    }

    public void cambiarEstado(EstadoComerciante nuevoEstado)
    {
        switch (nuevoEstado)
        {
            case EstadoComerciante.ESPERAR_COMERCIO:
                stateUpdate = updateEsperando;
                break;
            case EstadoComerciante.COMERCIANDO:
                stateUpdate = updateComerciando;
                break;
            case EstadoComerciante.BUSCANDO_ISLA:
                stateUpdate = updateBuscando;
                break;
            case EstadoComerciante.VIAJANDO_OTRA_LISTA:
                stateUpdate = updateViajando;
                break;
            case EstadoComerciante.HUIR:
                stateUpdate = updateHuir;
                break;
            case EstadoComerciante.ATRACADO:
                stateUpdate = updateAtracado;
                break;
        }
        estadoActual = nuevoEstado;
    }

    public Movimiento getMovimiento()
    {
        return movimiento;
    }
    public int getComida()
    {
        return comida;
    }
    public int getOro()
    {
        return oro;
    }
    public int getMadera()
    {
        return madera;
    }
    public int getTabaco()
    {
        return tabaco;
    }
    public void setComida(int newValue)
    {
        comida = newValue;
        canvasRecursos.setComida(newValue);
    }
    public void setOro(int newValue)
    {
        oro = newValue;
        canvasRecursos.setOro(newValue);
    }
    public void setMadera(int newValue)
    {
        madera = newValue;
        canvasRecursos.setMadera(newValue);
    }
    public void setTabaco(int newValue)
    {
        tabaco = newValue;
        canvasRecursos.setTabaco(newValue);
    }
    public void setEspera(bool estado)
    {
        this.esperando = estado;
    }
    private void updateEsperando()
    {
        if (esperando == false)
        {
            cambiarEstado(EstadoComerciante.COMERCIANDO);
        }
    }

    private void updateComerciando()
    {
        islaDestino.comerciarConBarco(this);
        cambiarEstado(EstadoComerciante.BUSCANDO_ISLA);
    }
    private void updateBuscando()
    {
        islaDestino = mundo.obtenerNuevoDestino(this, islaDestino);
        cambiarEstado(EstadoComerciante.VIAJANDO_OTRA_LISTA);
    }
    private void updateViajando()
    {
        if (movimiento.updateMovement(islaDestino.getActualPos()))
        {
            islaDestino.avisarBarcoEsperando(this);
            cambiarEstado(EstadoComerciante.ESPERAR_COMERCIO);
        }
    }

    private void updateHuir()
    {
        movimiento.huir(huyendoPirata.getMovimiento().getPos());
    }
    private void updateAtracado()
    {
        setOro(Mathf.CeilToInt(0.01f*oro));
        setMadera(Mathf.CeilToInt(0.01f*madera));
        setComida(Mathf.CeilToInt(0.01f*comida));
        setTabaco(Mathf.CeilToInt(0.01f*tabaco));
        esperarReaunudarViaje(4);
        
    }

    IEnumerator esperarReaunudarViaje(int tiempo)
    {
        yield return new WaitForSeconds(tiempo);
        cambiarEstado(EstadoComerciante.VIAJANDO_OTRA_LISTA);
    }


    //USAR PARA COMUNICARSE
    public void atracar()
    {
        oro = 0;
        tabaco = Mathf.CeilToInt(0.25f * tabaco);
        madera = Mathf.CeilToInt(0.25f * madera);
        comida = Mathf.CeilToInt(0.25f * comida);
        cambiarEstado(EstadoComerciante.BUSCANDO_ISLA);
    }
    public void avisarEsPerseguido(Pirata pirata)
    {
        huyendoPirata = pirata;
        cambiarEstado(EstadoComerciante.HUIR);
    }
}
