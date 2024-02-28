using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIScr : MonoBehaviour
{
    public TMP_Text textMadera;
    public TMP_Text textComida;

    Recursos recursosScr;
    void Start()
    {
        recursosScr = GameObject.Find("GameManager").GetComponent<Recursos>();
    }

    // Update is called once per frame
    void Update()
    {
        textMadera.text = "M: " + recursosScr.madera.ToString();
        textComida.text = "C: " + recursosScr.comida.ToString();
    }
}
