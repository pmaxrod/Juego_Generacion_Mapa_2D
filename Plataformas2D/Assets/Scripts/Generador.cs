using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

#if UNITY_EDITOR
using UnityEditor;
#endif
//------------------------------------------------------------------------------------------



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

	[Tooltip("La configuración del mapa")]
	public ConfigurarMapa configurarMapa;


	

	int[,] mapa;


	//------------------------------------------------------------------------------------------
	private void Start()
	{
		//GenerarMapa() ;

		
	}

	//------------------------------------------------------------------------------------------

	private void Update()
	{
		if( Input.GetKeyDown( KeyCode.G) || Input.GetMouseButtonDown(0))
		{
			GenerarMapa() ;
		}

		if( Input.GetKeyDown( KeyCode.L) || Input.GetMouseButtonDown(1))
		{
			LimpiarMapa() ;
		}
	}


	//------------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// 
	/// </summary>
	/// <param name="_mapa"></param>
	/// <param name="_mapaDeLosetas"></param>
	/// <param name="_loseta"></param>

	[ExecuteInEditMode]
	public void GenerarMapa( )
	{
		Debug.Log("Generando mapa") ;
		float semilla = 0f;

		// crear el array bidimensional del mapa
		mapa = new int[ ancho, alto];

		// Limpiar mapa de rosetas
		LimpiarMapa();


		// Generar semilla nueva de forma aleatoria
		if (configurarMapa.semillaAleatoria == true)
		{
			semilla = Random.Range(0f, 1000f);
		}
		else
			semilla = configurarMapa.semilla;


		switch (configurarMapa.algoritmo)
        {
		case Algoritmo.PERLINNOISE:
				mapa = Algoritmos.GenerarArray(mapa, ancho, alto, true);
				mapa = Algoritmos.PerlinNoise(mapa, semilla);
				break;

		case Algoritmo.PERLINNOISE_SUAVIZADO:
				mapa = Algoritmos.GenerarArray(mapa, ancho, alto, true);
				mapa = Algoritmos.PerlinNoise_Suavizado(mapa, semilla, configurarMapa.intervalo);
				break;

    	case Algoritmo.RANDOMWALK:
				mapa = Algoritmos.GenerarArray(mapa, ancho, alto, true);
				mapa = Algoritmos.RandomWalk(mapa, semilla);
				break;

		case Algoritmo.RANDOMWALK_SUAVIZADO:
				mapa = Algoritmos.GenerarArray(mapa, ancho, alto, true);
				mapa = Algoritmos.RandomWalk_Suavizado(mapa, semilla, configurarMapa.intervalo);
				break;



		case Algoritmo.PERLINNOISE_CUEVA:
				mapa = Algoritmos.GenerarArray(mapa, ancho, alto, false);
				mapa = Algoritmos.PerlinNoise_Cueva(mapa, semilla, configurarMapa.modificador, configurarMapa.conBordes);
					break;
		case Algoritmo.PERLINNOISE_CUEVA_MODIFICADO:
				mapa = Algoritmos.GenerarArray(mapa, ancho, alto, false);
				mapa = Algoritmos.PerlinNoise_Cueva(mapa, configurarMapa.modificador, configurarMapa.conBordes, configurarMapa.desplazamientoX, configurarMapa.desplazamientoY, semilla);
				break;
	

		case Algoritmo.RANDOMWALK_CUEVA:
				mapa = Algoritmos.GenerarArray(mapa, ancho, alto, false);
				mapa = Algoritmos.RandomWalk_Cueva(mapa, semilla, configurarMapa.porcentajeEliminar);
				break;

				
		case Algoritmo.RANDOMWALK_CUEVA_MODIFICADO:

				mapa = Algoritmos.GenerarArray(mapa, ancho, alto, false);
				mapa = Algoritmos.RandomWalk_Cueva(mapa, semilla, configurarMapa.porcentajeEliminar, configurarMapa.conBordes, configurarMapa.diagonal);
				break;
				
		case Algoritmo.TUNEL_VERTICAL:
				mapa = Algoritmos.GenerarArray(mapa, ancho, alto, false);
				mapa = Algoritmos.TunelDireccional(mapa, semilla, configurarMapa.minAncho, configurarMapa.maxAncho, configurarMapa.aspereza);
				break;
				
		case Algoritmo.TUNEL_VERTICAL_MODIFICADO:
				mapa = Algoritmos.GenerarArray(mapa, ancho, alto, false);
				mapa = Algoritmos.TunelDireccional(mapa, semilla, configurarMapa.minAncho, configurarMapa.maxAncho, configurarMapa.aspereza, configurarMapa.desplazamientoMax, configurarMapa.desplazamiento);
				break;
		
		case Algoritmo.TUNEL_HORIZONTAL:
				mapa = Algoritmos.GenerarArray(mapa, ancho, alto, false);
				mapa = Algoritmos.TunelDireccional_Horizontal(mapa, semilla, configurarMapa.minAncho, configurarMapa.maxAncho, configurarMapa.aspereza);
				break;		
				
		case Algoritmo.TUNEL_HORIZONTAL_MODIFICADO:
				mapa = Algoritmos.GenerarArray(mapa, ancho, alto, false);
				mapa = Algoritmos.TunelDireccional_Horizontal(mapa, semilla, configurarMapa.minAncho, configurarMapa.maxAncho, configurarMapa.aspereza, configurarMapa.desplazamientoMax, configurarMapa.desplazamiento);
				break;
				
		case Algoritmo.MAPA_ALEATORIO:
				mapa = Algoritmos.GenerarMapaAleatorio(ancho, alto, semilla, configurarMapa.porcentajeRellenoFloat, configurarMapa.conBordes);
				break;

		case Algoritmo.AUTOMATA_CELULAR_MOORE:
				mapa = Algoritmos.GenerarMapaAleatorio(ancho, alto, semilla, configurarMapa.porcentajeRellenoFloat, configurarMapa.conBordes);
				mapa = Algoritmos.AutomataCelularMoore(mapa, configurarMapa.numeroPasadas);
				break;

			
		case Algoritmo.AUTOMATA_CELULAR_VONNEUMAN:
				mapa = Algoritmos.GenerarMapaAleatorio(ancho, alto, semilla, configurarMapa.porcentajeRellenoFloat, configurarMapa.conBordes);
				mapa = Algoritmos.AutomataCelularVonNeuman(mapa, configurarMapa.numeroPasadas, configurarMapa.conBordes);
				break;

		}

		VisualizarMapa.DibujarMapa(mapa, mapaDeLosetas, loseta);
		
		

		
		



	}

	
	//------------------------------------------------------------------------------------------
	public void LimpiarMapa()
	{
		Debug.Log("Limpiando mapa") ;

		mapaDeLosetas.ClearAllTiles();

	}

	//------------------------------------------------------------------------------------------
}
