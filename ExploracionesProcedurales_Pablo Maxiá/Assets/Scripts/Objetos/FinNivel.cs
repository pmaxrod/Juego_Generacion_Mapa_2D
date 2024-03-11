using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.UI.Image;

public class FinNivel : MonoBehaviour
{
    private float longitudRayo;
    public GameObject indicadorFin;

    void Start()
    {
        longitudRayo = Generador.instance.GetDimensiones().y;
        InstanciarIndicadorFinNivel();
    }

    // Acaba el nivel
    public void AcabarNivel()
    {
        DatosJugador.instance.puntuacion = (DatosJugador.instance.monedas * 100 + DatosJugador.instance.vidas * 10);
        SceneManager.LoadScene(Constantes.ESCENA_FIN_JUEGO);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Constantes.TAG_JUGADOR))
        {
            AcabarNivel();
        }
    }

    // Instancia el Indicador del final del nivel
    private void InstanciarIndicadorFinNivel()
    {
        //indicadorFin.transform.Translate(0, -Mathf.FloorToInt(longitudRayo), 0);

        if (Generador.instance.configurarMapa.algoritmo == Algoritmo.TUNEL_VERTICAL)
        {
            indicadorFin.transform.Rotate(new Vector3(0, 0, 90));
            longitudRayo = Generador.instance.GetDimensiones().x * 1.5f;
        }
        indicadorFin.transform.localScale = new Vector3(0.1f, longitudRayo * 1.5f, 1);
    }

}
