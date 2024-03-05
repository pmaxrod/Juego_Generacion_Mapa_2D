using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinNivel : MonoBehaviour
{
    public static int puntuacion = 0;

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
