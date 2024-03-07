using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinNivel : MonoBehaviour
{
    public static int puntuacion = 0;
    float longitudRayo = 75f;

    void FixedUpdate()
    {
        RaycastHit2D raycast = Physics2D.Raycast(transform.position, new Vector2(0, longitudRayo), longitudRayo);

        // Si el collider del raycast not es nulo
        if (raycast.collider.CompareTag(Constantes.TAG_JUGADOR))
        {
            AcabarNivel();
            Debug.Log("Golpeando: " + raycast.collider.tag);
        }

        // Rayo dibujado en la escena para depuracion
        Debug.DrawRay(transform.position, Vector2.down * longitudRayo, Color.red);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag(Constantes.TAG_JUGADOR))
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
