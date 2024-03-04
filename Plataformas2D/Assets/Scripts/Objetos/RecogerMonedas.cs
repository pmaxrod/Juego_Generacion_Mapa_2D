using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecogerMonedas : MonoBehaviour
{
    public static int totalMonedas = 0;

    void Awake()
    {
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        //Destroy the coin if Object tagged Player comes in contact with it
        if (collider.CompareTag("Player"))
        {
            //Add coin to counter
            totalMonedas++;
            //Test: Print total number of coins
            Debug.Log("Tienes " + totalMonedas + " monedas");
            //Destroy coin
            Destroy(gameObject);
        }
    }
}
