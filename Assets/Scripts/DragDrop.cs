using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DragDrop : MonoBehaviour
{
    Vector3 mousePositionOffset;
    public bool siendoArrastrado = false;

    private Movimiento movimientoScr;
    private MovimientoCamara movimientoCamaraScr;

    private BoxCollider2D bc2d;

    private NavMeshAgent agent;
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        bc2d = GetComponent<BoxCollider2D>();
        movimientoScr = GetComponent<Movimiento>();
        movimientoCamaraScr = GameObject.Find("Main Camera").GetComponent<MovimientoCamara>();
    }
    private Vector3 GetMouseWorldPosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void OnMouseDown()
    {
        //al apretar el aldeano
        movimientoCamaraScr.arrastrandoAldeano = true;
        agent.enabled = false;
        mousePositionOffset = gameObject.transform.position - GetMouseWorldPosition();
        movimientoScr.StopCoroutine("ActivarAgente");

    }

    private void OnMouseDrag()
    {
        //Al arrastrar el aldeano
        siendoArrastrado = true;
        transform.position = GetMouseWorldPosition() + mousePositionOffset;
    }

    private void OnMouseUp()
    {
        //Al soltar el aldeano
        movimientoCamaraScr.arrastrandoAldeano = false;
        movimientoScr.StartCoroutine("ActivarAgente", 2f);
        siendoArrastrado = false;
    }
}
