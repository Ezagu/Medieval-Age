using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AserraderoScr : MonoBehaviour
{
    private Recursos recursosScr;

    private GameObject aldeanoParent;

    private int cantidadAldeanos;
    public int maxAldeanos;

    public int maderaPorAldeano;

    public float tiempoDobleClick;
    private float ultimoTiempoClick;
    private void Start()
    {
        StartCoroutine("SumarMadera");
        recursosScr = GameObject.Find("GameManager").GetComponent<Recursos>();
        aldeanoParent = GameObject.Find("Aldeanos");
    }

    private void Update()
    {
        cantidadAldeanos = transform.childCount;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Aldeano") && !collision.GetComponent<DragDrop>().siendoArrastrado)
        {
            AñadirAldeano(collision.gameObject);
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

    public void AñadirAldeano(GameObject aldeano)
    {
        if (cantidadAldeanos < maxAldeanos)
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

    IEnumerator SumarMadera()
    {
        yield return new WaitForSeconds(1f);
        recursosScr.madera += maderaPorAldeano * transform.childCount;
        StartCoroutine("SumarMadera");

    }
}