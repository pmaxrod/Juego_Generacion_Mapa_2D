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
	[Tooltip("El mosaico para dibujar(usa un Rule Tile para obtener mejores resultados)")]
	[SerializeField] private TileBase loseta;
	
	[Header("Dimensiones mapa")]
	[Tooltip("Ancho del mapa")]
	[SerializeField] private int ancho = 60;
	[Tooltip("Alto del mapa")]
	[SerializeField] private int alto = 34;

	[Tooltip("La configuracion del mapa")]
	public ConfigurarMapa configurarMapa;

    int[,] mapa;
/*
    [Header("Semilla")]
    [SerializeField] private bool semillaAleatoria = true;
    [SerializeField] private float semilla = 0f;

    [Header("Perlin Noise suavizado")]
    [SerializeField] private int intervalo = 1;

    [Header("Algoritmo - RandomWalk suavizado")]
    [SerializeField] private int minimoAnchoSeccion = 2;

    [Header("Cuevas")]
    [SerializeField] private bool bordesSonMuros = true;

    [Header("PerlinNoise Cuevas")]
    [SerializeField] private float modificador = 0.1f;
    [SerializeField] private float offSetX = 0f;
    [SerializeField] private float offSetY = 0f;


    [Header("RandomWalk Cueva")]
    [Range(0, 1)] // para asegurarnos que esta entre 0 y 1
    [SerializeField] private float porcentajeEliminar = 0.25f; //  25 %
    [SerializeField] private bool movimientoDiagonal = false;

    [Header("Tunel Direccional")]
    [SerializeField] private int anchoMaximo = 4;
    [SerializeField] private int anchoMinimo = 1; // se quitar� 3 bloques

    [Range(0, 1)] // para asegurarnos que esta entre 0 y 1
    [SerializeField] private float aspereza = 0.75f;

    [SerializeField] private int desplazamientoMaximo = 1;

    [Range(0, 1)] // para asegurarnos que esta entre 0 y 1
    [SerializeField] private float desplazamiento = 0.75f;


    [Header("Automata celular")]
    [Range(0, 1)]
    [SerializeField] private float porcentajeRelleno = 0.45f;
    [SerializeField] private int totalPasadas = 1;

    [Header("Elegir Algoritmo")]
    [SerializeField] private Algoritmo algoritmo = Algoritmo.PERLIN_NOISE;
	*/


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            GenerarMapa();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            LimpiarMapa();
        }
    }

	[ExecuteInEditMode]
    public void GenerarMapa()
    {
        Debug.Log("Estoy en generar mapa");
		float semilla = 0f;

        mapa = new int[ancho, alto];

        LimpiarMapa();

		// Generar semilla nueva de forma aleatoria
		if (configurarMapa.semillaAleatoria == true)
		{
			semilla = Random.Range(0f, 1000f);
		}
		else
		{
			semilla = configurarMapa.semilla;
		}

        //mapa = Algoritmos.GenerarArray(ancho, alto, false);

        switch (configurarMapa.algoritmo)
        {
		case Algoritmo.PERLINNOISE:
				mapa = Algoritmos.GenerarArray(ancho, alto, true);
				mapa = Algoritmos.PerlinNoise(mapa, semilla);
				break;

		case Algoritmo.PERLINNOISE_SUAVIZADO:
				mapa = Algoritmos.GenerarArray(ancho, alto, true);
				mapa = Algoritmos.PerlinNoise(mapa, semilla, configurarMapa.intervalo);
				break;

    	case Algoritmo.RANDOMWALK:
				mapa = Algoritmos.GenerarArray(ancho, alto, true);
				mapa = Algoritmos.RandomWalk(mapa, semilla);
				break;

		case Algoritmo.RANDOMWALK_SUAVIZADO:
				mapa = Algoritmos.GenerarArray(ancho, alto, true);
				mapa = Algoritmos.RandomWalk(mapa, semilla, configurarMapa.intervalo);
				break;

		case Algoritmo.PERLINNOISE_CUEVA:
				mapa = Algoritmos.GenerarArray(ancho, alto, false);
				mapa = Algoritmos.PerlinNoise_Cueva(mapa, configurarMapa.modificador, configurarMapa.conBordes);
				break;
					
		case Algoritmo.PERLINNOISE_CUEVA_MODIFICADO:
				mapa = Algoritmos.GenerarArray(ancho, alto, false);
				mapa = Algoritmos.PerlinNoise_Cueva(mapa, configurarMapa.modificador, configurarMapa.conBordes, configurarMapa.desplazamientoX, configurarMapa.desplazamientoY, semilla);
				break;

		case Algoritmo.RANDOMWALK_CUEVA:
				mapa = Algoritmos.GenerarArray(ancho, alto, false);
				mapa = Algoritmos.RandomWalk_Cueva(mapa, semilla, configurarMapa.porcentajeEliminar);
				break;

		case Algoritmo.TUNEL_VERTICAL:
				mapa = Algoritmos.GenerarArray(ancho, alto, false);
				mapa = Algoritmos.TunelDireccional(mapa, semilla, configurarMapa.minAncho, configurarMapa.maxAncho, configurarMapa.aspereza, configurarMapa.desplazamientoMax, configurarMapa.desplazamiento);
				break;

		case Algoritmo.TUNEL_HORIZONTAL:
				mapa = Algoritmos.GenerarArray(ancho, alto, false);
				mapa = Algoritmos.TunelHorizontal(mapa, semilla, configurarMapa.minAncho, configurarMapa.maxAncho, configurarMapa.aspereza, configurarMapa.desplazamientoMax, configurarMapa.desplazamiento);
				break;		

		case Algoritmo.MAPA_ALEATORIO:
				mapa = Algoritmos.GenerarMapaAleatorio(ancho, alto, semilla, configurarMapa.porcentajeRellenoFloat, configurarMapa.conBordes);
				break;

		case Algoritmo.AUTOMATA_CELULAR_MOORE:
				mapa = Algoritmos.GenerarMapaAleatorio(ancho, alto, semilla, configurarMapa.porcentajeRellenoFloat, configurarMapa.conBordes);
				mapa = Algoritmos.AutomataCelularMoore(mapa, configurarMapa.numeroPasadas, configurarMapa.conBordes);
				break;

			
		case Algoritmo.AUTOMATA_CELULAR_VONNEUMAN:
				mapa = Algoritmos.GenerarMapaAleatorio(ancho, alto, semilla, configurarMapa.porcentajeRellenoFloat, configurarMapa.conBordes);
				mapa = Algoritmos.AutomataCelularVonNeuman(mapa, configurarMapa.numeroPasadas, configurarMapa.conBordes);
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
