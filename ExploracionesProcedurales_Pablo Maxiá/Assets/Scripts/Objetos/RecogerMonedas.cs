using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecogerMonedas : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag(Constantes.TAG_JUGADOR))
        {
            DatosJugador.instance.monedas++;
            Debug.Log("Tienes " + DatosJugador.instance.monedas + " monedas");
            Destroy(gameObject);
        }
    }
}
