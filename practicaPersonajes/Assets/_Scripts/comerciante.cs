using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Comerciante : MonoBehaviour
{
    [HideInInspector] private Movimiento movimiento;
    [SerializeField] public Mundo mundo;

    //FSM
    private delegate void StateUpdate();
    private StateUpdate stateUpdate;
    public enum EstadoComerciante { ESPERAR_COMERCIO, COMERCIANDO, BUSCANDO_ISLA, VIAJANDO_OTRA_LISTA, HUIR, ATRACADO };
    [SerializeField] private EstadoComerciante estadoActual;

    [SerializeField] private Isla islaDestino;
    [SerializeField] private Pirata huyendoPirata;

    //Recursos
    [SerializeField] public int oro = 0;
    [SerializeField] public int tabaco = 0;
    [SerializeField] public int madera = 0;
    [SerializeField] public int comida = 0;

    void Start()
    {
        cambiarEstado(EstadoComerciante.BUSCANDO_ISLA);
        islaDestino = mundo.islas[0];
        NavMeshAgent agent = transform.GetChild(1).GetComponent<NavMeshAgent>();
        movimiento = new Movimiento( agent, transform.GetChild(0) );
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

    public Movimiento getMovimiento(){
        return movimiento;
    }

    private void updateEsperando(){
    }

    private void updateComerciando(){
        islaDestino.comerciarConBarco(this);
        cambiarEstado(EstadoComerciante.ESPERAR_COMERCIO);
    }
    private void updateBuscando(){
        islaDestino = mundo.obtenerNuevoDestino(this,islaDestino);
        cambiarEstado(EstadoComerciante.VIAJANDO_OTRA_LISTA);
    }
    private void updateViajando(){
        if( movimiento.updateMovement(islaDestino.getActualPos())){
            islaDestino.avisarBarcoEsperando(this);
            cambiarEstado(EstadoComerciante.ESPERAR_COMERCIO);
        }
    }

    private void updateHuir(){
        movimiento.huir(huyendoPirata.getMovimiento().getPos());
    }
    private void updateAtracado(){
    }


    //USAR PARA COMUNICARSE
    public void atracar(){
        oro=0;
        tabaco = Mathf.CeilToInt(0.25f*tabaco);
        madera = Mathf.CeilToInt(0.25f*madera);
        comida = Mathf.CeilToInt(0.25f*comida);
        cambiarEstado(EstadoComerciante.BUSCANDO_ISLA);
    }
    public void avisarEsPerseguido(Pirata pirata){
        huyendoPirata = pirata;
        cambiarEstado(EstadoComerciante.HUIR);
    }
}
