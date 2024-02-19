using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Empezar la partida
    public void IniciarPartida()
    {
        SceneManager.LoadScene(Constantes.ESCENA_PERLIN);
    }

    // Salir del juego
    public void Salir()
    {
        Application.Quit();
    }

    // Panel de creditos
    public void Creditos()
    {

    }
}
