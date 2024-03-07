using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextoInterfaz : MonoBehaviour
{
    [Header("Texto escenas jugables")]
    [SerializeField] private TMP_Text textoContadorMonedas;
    [SerializeField] private TMP_Text textoContadorVidas;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (textoContadorMonedas.text != DatosJugador.instance.monedas.ToString())
        {
            textoContadorMonedas.text = DatosJugador.instance.monedas.ToString();
        }
        if (textoContadorVidas.text != DatosJugador.instance.vidas.ToString())
        {
            textoContadorVidas.text = DatosJugador.instance.vidas.ToString();
        }
    }
}
