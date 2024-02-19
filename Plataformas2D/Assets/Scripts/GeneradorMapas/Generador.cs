using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Controlador
/// </summary>
public class Generador : MonoBehaviour
{
    [Header("Referencias")]
    [Tooltip("El Tilemap para dibujar el mapa")]
    [SerializeField] private Tilemap mapaDeLosetas;
    [Tooltip("El mosaico para dibujar (usa un Rule Tile para obtener mejores resultados)")]
    [SerializeField] private TileBase loseta;

    [Header("Dimensiones mapa")]
    [Tooltip("Ancho del mapa")]
    [SerializeField] private int ancho = 60;
    [Tooltip("Alto del mapa")]
    [SerializeField] private int alto = 40;

    [Tooltip("La configuracion del mapa")]
    public ConfigurarMapa configurarMapa;
    [Tooltip("Los algoritmos a escoger")]
    public AlgoritmosMapa algoritmosMapa;

    [Tooltip("Algoritmo aleatorio")]
    public bool algoritmoAleatorio;
    int[,] mapa;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.G))
        {
            GenerarMapa();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            LimpiarMapa();
        }*/
    }

    [ExecuteInEditMode]
    public void GenerarMapa()
    {
        Debug.Log("Estoy en generar mapa");
        float semilla = 0f;

        mapa = new int[ancho, alto];

        LimpiarMapa();

        if (algoritmoAleatorio)
            configurarMapa.algoritmo = algoritmosMapa.AlgoritmoAleatorio();

        // Generar semilla nueva de forma aleatoria
        semilla = configurarMapa.semillaAleatoria ? Random.Range(0f, 1000f) : configurarMapa.semilla;

        switch (configurarMapa.algoritmo)
        {
            case Algoritmo.PERLINNOISE:
                mapa = Algoritmos.GenerarArray(mapa, ancho, alto, true);
                mapa = Algoritmos.PerlinNoise(mapa, semilla);
                break;

            case Algoritmo.PERLINNOISE_SUAVIZADO:
                mapa = Algoritmos.GenerarArray(mapa, ancho, alto, true);
                mapa = Algoritmos.PerlinNoise(mapa, semilla, configurarMapa.intervalo);
                break;

            case Algoritmo.RANDOMWALK:
                mapa = Algoritmos.GenerarArray(mapa, ancho, alto, true);
                mapa = Algoritmos.RandomWalk(mapa, semilla);
                break;

            case Algoritmo.RANDOMWALK_SUAVIZADO:
                mapa = Algoritmos.GenerarArray(mapa, ancho, alto, true);
                mapa = Algoritmos.RandomWalk(mapa, semilla, configurarMapa.intervalo);
                break;

            case Algoritmo.PERLINNOISE_CUEVA:
                mapa = Algoritmos.GenerarArray(mapa, ancho, alto, false);
                mapa = Algoritmos.PerlinNoiseCueva(mapa, semilla, configurarMapa.modificador, configurarMapa.conBordes);
                break;

            case Algoritmo.PERLINNOISE_CUEVA_MODIFICADO:
                mapa = Algoritmos.GenerarArray(mapa, ancho, alto, false);
                mapa = Algoritmos.PerlinNoiseCueva(mapa, semilla, configurarMapa.modificador, configurarMapa.conBordes, configurarMapa.desplazamientoX, configurarMapa.desplazamientoY);
                break;

            case Algoritmo.RANDOMWALK_CUEVA:
                mapa = Algoritmos.GenerarArray(mapa, ancho, alto, false);
                mapa = Algoritmos.RandomWalkCueva(mapa, semilla, configurarMapa.porcentajeEliminar);
                break;
            case Algoritmo.RANDOMWALK_CUEVA_MODIFICADO:
                mapa = Algoritmos.GenerarArray(mapa, ancho, alto, false);
                mapa = Algoritmos.RandomWalkCueva(mapa, semilla, configurarMapa.porcentajeEliminar, configurarMapa.conBordes, configurarMapa.diagonal);
                break;

            case Algoritmo.TUNEL_VERTICAL:
                mapa = Algoritmos.GenerarArray(mapa, ancho, alto, false);
                mapa = Algoritmos.TunelVertical(mapa, semilla, configurarMapa.minAncho, configurarMapa.maxAncho, configurarMapa.aspereza, configurarMapa.desplazamientoMax, configurarMapa.desplazamiento);
                break;

            case Algoritmo.TUNEL_HORIZONTAL:
                mapa = Algoritmos.GenerarArray(mapa, ancho, alto, false);
                mapa = Algoritmos.TunelHorizontal(mapa, semilla, configurarMapa.minAncho, configurarMapa.maxAncho, configurarMapa.aspereza, configurarMapa.desplazamientoMax, configurarMapa.desplazamiento);
                break;

            case Algoritmo.MAPA_ALEATORIO:
                mapa = Algoritmos.GenerarMapaAleatorio(ancho, alto, semilla, configurarMapa.porcentajeRellenoFloat, configurarMapa.conBordes);
                break;

            case Algoritmo.AUTOMATA_CELULAR_MOORE:
                mapa = Algoritmos.GenerarMapaAleatorio(ancho, alto, semilla, configurarMapa.porcentajeRellenoFloat, configurarMapa.conBordes);
                mapa = Algoritmos.AutomataCelular(mapa, configurarMapa.numeroPasadas, configurarMapa.conBordes, 4, true);
                break;

            case Algoritmo.AUTOMATA_CELULAR_VONNEUMAN:
                mapa = Algoritmos.GenerarMapaAleatorio(ancho, alto, semilla, configurarMapa.porcentajeRellenoFloat, configurarMapa.conBordes);
                mapa = Algoritmos.AutomataCelular(mapa, configurarMapa.numeroPasadas, configurarMapa.conBordes, 2, false);
                break;
        }

        // dibujar el mapa
        VisualizarMapa.MostrarMapa(mapa, mapaDeLosetas, loseta);
    }

    public void LimpiarMapa()
    {
        Debug.Log("Estoy en LIMPIAR mapa");

        VisualizarMapa.LimpiarMapa(mapaDeLosetas);
    }
}
