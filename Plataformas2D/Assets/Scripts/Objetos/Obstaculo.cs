using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstaculo : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter2D(Collision2D other) {
        if (other.collider.CompareTag(Constantes.TAG_JUGADOR))
        {
            DatosJugador.instance.vidas--;
            Debug.Log("Tienes " + DatosJugador.instance.vidas + " vidas");
        }
    }
}
