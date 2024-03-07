using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecogerMonedas : MonoBehaviour
{
    public static int totalMonedas = 0;

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag(Constantes.TAG_JUGADOR))
        {
            totalMonedas++;
            Debug.Log("Tienes " + totalMonedas + " monedas");
            Destroy(gameObject);
        }
    }
}
