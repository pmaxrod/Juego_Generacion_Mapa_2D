using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlMenu : MonoBehaviour
{
    [SerializeField] GameObject panelInicio;
    [SerializeField] GameObject panelCreditos;
    [SerializeField] TMP_Dropdown escenas;

    GameObject[] paneles;

    string escenaElegida;

    void Awake()
    {
        paneles = new GameObject[] { panelInicio, panelCreditos };

        escenas.interactable = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        ActivarPanel(panelInicio);
    }

    // Update is called once per frame
    void Update()
    {

    }

    #region METODOS PUBLICOS
    // Empezar la partida
    public void IniciarPartida()
    {
        escenaElegida = EscenaElegida(escenas.value);

        SceneManager.LoadScene(escenaElegida);
    }

    // Salir del juego
    public void Salir()
    {
        Application.Quit();
    }

    // Panel de creditos
    public void Creditos()
    {
        ActivarPanel(panelCreditos);
    }

    // Volver al inicio desde los creditos
    public void Atras()
    {
        ActivarPanel(panelInicio);
    }
    #endregion

    #region METODOS PRIVADOS
    private void ActivarPanel(GameObject _panel)
    {
        foreach (GameObject panel in paneles)
        {
            panel.SetActive(false);
        }

        _panel.SetActive(true);
    }

    private string EscenaElegida(int _escena)
    {

        switch (_escena)
        {
            case 0:
                return Constantes.ESCENA_PERLIN;
            case 1:
                return Constantes.ESCENA_TUNEL;
            case 2:
                return Constantes.ESCENA_AUTOMATA;
            default:
                return "EscenaPrueba";
        }
    }
    #endregion

}
