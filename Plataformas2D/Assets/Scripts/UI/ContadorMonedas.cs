using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ContadorMonedas : MonoBehaviour
{
     [SerializeField] TMP_Text textoContador;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //Set the current number of coins to display
        if (textoContador.text != RecogerMonedas.totalMonedas.ToString())
        {
            textoContador.text = RecogerMonedas.totalMonedas.ToString();
        }
    }
}
