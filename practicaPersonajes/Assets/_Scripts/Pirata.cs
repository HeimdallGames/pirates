using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Pirata : MonoBehaviour
{
    [HideInInspector] private Movimiento movimiento;
    [SerializeField] private float extraEndDistance = 7.2f;
    [SerializeField] private float extraPatroll = 7.2f;
    [SerializeField] private float patrollRadius = 22.2f;

    public enum EstadoPirata { ESPERAR_BARCO, ATACAR, HUIR, CONSEGUIR_BOTIN };
    [SerializeField] private EstadoPirata estadoActual;

	[SerializeField] private Comerciante target;
    [SerializeField] private List<GameObject> listaColisiones;
    [SerializeField] private GameObject collisionObject;

    //FSM
    private delegate void StateUpdate();
    private StateUpdate stateUpdate;
    void Start()
    {
        NavMeshAgent agent = transform.GetChild(1).GetComponent<NavMeshAgent>();
        movimiento = new Movimiento(agent, transform.GetChild(0), extraEndDistance, patrollRadius, extraPatroll);
        cambiarEstado(EstadoPirata.ESPERAR_BARCO);
    }

    void Update()
    {
        if (listaColisiones.Count > 0)
        {
            collisionObject = listaColisiones[0];
            listaColisiones.RemoveAt(0);
        }
        stateUpdate();
    }

    public void cambiarEstado(EstadoPirata nuevoEstado)
    {
        switch (nuevoEstado)
        {
            case EstadoPirata.ATACAR:
                stateUpdate = updateAtacando;
                break;
            case EstadoPirata.CONSEGUIR_BOTIN:
                stateUpdate = updateConsiguiendoBotin;
                break;
            case EstadoPirata.ESPERAR_BARCO:
                stateUpdate = updateEsperandoBarco;
                break;
            case EstadoPirata.HUIR:
                stateUpdate = updateHuyendo;
                break;
        }
        estadoActual = nuevoEstado;
    }
    public Movimiento getMovimiento()
    {
        return movimiento;
    }

    private void updateAtacando()
    {
		movimiento.updateMovement (target.getMovimiento().getPos());
    }
    private void updateHuyendo()
    {
        target = null;
    }
    private void updateEsperandoBarco()
    {
        if (collisionObject.tag == "Comerciante")
        {
            target = collisionObject.GetComponent<Comerciante>();
            //target.avisarEsPerseguido(this);
            cambiarEstado(EstadoPirata.ATACAR);
        }
        movimiento.patrullar();
    }
    private void updateConsiguiendoBotin()
    {
        //todo
    }
    

    /*COLISIONES*/
	void OnCollisionEnter2D(Collision2D coll)
	{
        listaColisiones.Add(coll.transform.parent.gameObject);
	}
}
