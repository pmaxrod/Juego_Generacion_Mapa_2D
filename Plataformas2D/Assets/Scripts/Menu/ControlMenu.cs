using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlMenu : MonoBehaviour
{
    [Header("Menu Inicio")]
    [SerializeField] GameObject panelInicio;
    [SerializeField] GameObject panelCreditos;
    [SerializeField] TMP_Dropdown escenas;
    [Header("Fin de la partida")]
    [SerializeField] TMP_Text puntuacion;
    [SerializeField] TMP_Text monedasRecogidas;
    [SerializeField] TMP_Text vidasRestantes;
    [SerializeField] TMP_Text textoVictoria;

    GameObject[] paneles;

    string escenaElegida;

    void Awake()
    {
        if (panelInicio != null && panelCreditos != null)
            paneles = new GameObject[] { panelInicio, panelCreditos };

        if (escenas != null)
            escenas.interactable = true;

        if (puntuacion != null && monedasRecogidas != null && vidasRestantes != null && textoVictoria != null)
            PantallaFin();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (panelInicio != null)
            ActivarPanel(panelInicio);
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

    public void Inicio()
    {
        SceneManager.LoadScene(Constantes.ESCENA_MENU_PRINCIPAL);
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

    private void PantallaFin()
    {
        puntuacion.text = $"Puntuación: {DatosJugador.instance.puntuacion}";
        monedasRecogidas.text = $"Monedas recogidas: {DatosJugador.instance.monedas}";

        if (DatosJugador.instance.vidas < 1){
            vidasRestantes.gameObject.SetActive(false);
        }

        vidasRestantes.text = $"Vidas restantes: {DatosJugador.instance.vidas}";

        string texto = DatosJugador.instance.vidas >= 1 ? "¡Has ganado!" : "Fin de la partida";

        textoVictoria.text = texto;
    }
    #endregion

}
