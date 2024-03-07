using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinNivel : MonoBehaviour
{
    public static int puntuacion = 0;

    void FixedUpdate()
    {
        //Length of the ray
        float laserLength = 50f;

        //Get the first object hit by the ray
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.one, laserLength);

        //If the collider of the object hit is not NUll
        if (hit.collider != null)
        {
            //Hit something, print the tag of the object
            Debug.Log("Hitting: " + hit.collider.tag);
        }

        //Method to draw the ray in scene for debug purpose
        Debug.DrawRay(transform.position, Vector2.right * laserLength, Color.red);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            AcabarNivel();
        }
    }

    public void AcabarNivel()
    {
        puntuacion = 100 + (RecogerMonedas.totalMonedas * 100);
        SceneManager.LoadScene(Constantes.ESCENA_FIN_JUEGO);

    }
}
