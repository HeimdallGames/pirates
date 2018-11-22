using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Comerciante : MonoBehaviour
{
    public Movimiento movimiento;

    //FSM
    private delegate void StateUpdate();
    private StateUpdate stateUpdate;

    private Isla islaDestino;
    public Mundo mundo;
    private Pirata huyendoPirata;
    public int oro = 0;
    public int tabaco = 0;
    public int madera = 0;
    public int comida = 0;

    //public enum EstadoComerciante { MOVIENDOSE, EN_DESTINO };
    public enum EstadoComerciante { ESPERAR_COMERCIO, COMERCIANDO, BUSCANDO_ISLA, VIAJANDO_OTRA_LISTA, HUIR, ATRACADO };
    private EstadoComerciante estadoActual;
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

    public void updateEsperando(){
    }

    public void updateComerciando(){
        islaDestino.comerciarConBarco(this);
        cambiarEstado(EstadoComerciante.ESPERAR_COMERCIO);
    }
    public void updateBuscando(){
        islaDestino = mundo.obtenerNuevoDestino(this,islaDestino);
        cambiarEstado(EstadoComerciante.VIAJANDO_OTRA_LISTA);
    }
    public void updateViajando(){
        if( movimiento.updateMovement(islaDestino.actualPos)){
            islaDestino.avisarBarcoEsperando(this);
            cambiarEstado(EstadoComerciante.ESPERAR_COMERCIO);
        }
    }

    public void updateHuir(){
        movimiento.huir(huyendoPirata.movimiento.getPos());
    }
    public void updateAtracado(){
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
