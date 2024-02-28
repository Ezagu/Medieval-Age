using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CasaScr : MonoBehaviour
{
    public float tiempoNacer;

    public int hijosCreados;
    public int cantidadAldeanos;

    public bool creandoHijo;

    public GameObject aldeanoPrefab;
    private GameObject aldeanoParent;

    public float tiempoDobleClick;
    private float ultimoTiempoClick;

    private void Start()
    {
        aldeanoParent = GameObject.Find("Aldeanos");
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Aldeano") && !collision.GetComponent<DragDrop>().siendoArrastrado)
        {
            AñadirAldeano(collision.gameObject);
        }
    }

    private void Update()
    {
        cantidadAldeanos = transform.childCount;

        //si hay dos aldeanos y se pueden crear mas hijos entonces crea hijo
        if (!creandoHijo && cantidadAldeanos == 2 && hijosCreados < 2)
        {
            creandoHijo = true;
            Invoke("CrearHijo", tiempoNacer);
        }

        if (Input.GetKeyDown("space"))
        {
            ExpulsarAldeano();
        }
    }

    private void OnMouseDown()
    {
        if (cantidadAldeanos > 0)
        {
            float tiempoDesdeUltimoClick = Time.time - ultimoTiempoClick;

            if (tiempoDesdeUltimoClick < tiempoDobleClick)
            {
                ExpulsarAldeano();
            }

            ultimoTiempoClick = Time.time;
        }
    }

    private void CrearHijo()
    {
        Debug.Log("Hijo Creado");
        hijosCreados++;
        GameObject hijoNuevo;
        hijoNuevo = Instantiate(aldeanoPrefab, transform.position, Quaternion.identity);
        hijoNuevo.transform.SetParent(aldeanoParent.transform);

        for (int i = 0; i < 2; i++)
        {
            Debug.Log("Aldeano Expulsado");
            ExpulsarAldeano();
        }

        creandoHijo = false;
    }

    public void AñadirAldeano(GameObject aldeano)
    {
        if (cantidadAldeanos < 2 && hijosCreados < 2)
        {
            Debug.Log("Se Sumó aldeano");
            aldeano.transform.SetParent(transform);
            aldeano.gameObject.SetActive(false);
        }
    }

    public void ExpulsarAldeano()
    {
        GameObject aldeano;
        aldeano = transform.GetChild(0).gameObject;
        aldeano.GetComponent<Movimiento>().esperando = false;
        aldeano.GetComponent<Movimiento>().agent.enabled = true;
        aldeano.SetActive(true);
        aldeano.transform.SetParent(aldeanoParent.transform);
    }
}
