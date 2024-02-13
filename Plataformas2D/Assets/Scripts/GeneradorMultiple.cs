using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

#if UNITY_EDITOR
using UnityEditor;
#endif
//------------------------------------------------------------------------------------------



public class GeneradorMultiple : MonoBehaviour
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

	[Tooltip("Lista de configuración del mapa")]	
	public List<ConfigurarMapa> configurarMapa = new List<ConfigurarMapa>();

	List<int[,]> listaMapas = new List<int[,]>();


	//int[,] mapa;


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
		Debug.Log("Generando mapas en pila") ;
			

		// Limpiar mapa de rosetas
		LimpiarMapa();
		listaMapas = new List<int[,]>();

		//Configurar los distintos mapas
		for (int i = 0; i < configurarMapa.Count; i++)
		{
			// crear el array bidimensional del mapa
			int [,]mapa = new int[ancho, alto];
			float semilla;

			// Generar semilla nueva de forma aleatoria
			if (configurarMapa[i].semillaAleatoria == true)
			{
				semilla = Random.Range(0f, 1000f);
			}
			else
				semilla = configurarMapa[i].semilla;


			switch (configurarMapa[i].algoritmo)
			{
				case Algoritmo.PERLINNOISE:
					mapa = Algoritmos.GenerarArray(mapa, ancho, alto, true);
					mapa = Algoritmos.PerlinNoise(mapa, semilla);
					break;

				case Algoritmo.PERLINNOISE_SUAVIZADO:
					mapa = Algoritmos.GenerarArray(mapa, ancho, alto, true);
					mapa = Algoritmos.PerlinNoise_Suavizado(mapa, semilla, configurarMapa[i].intervalo);
					break;

				case Algoritmo.RANDOMWALK:
					mapa = Algoritmos.GenerarArray(mapa, ancho, alto, true);
					mapa = Algoritmos.RandomWalk(mapa, semilla);
					break;

				case Algoritmo.RANDOMWALK_SUAVIZADO:
					mapa = Algoritmos.GenerarArray(mapa, ancho, alto, true);
					mapa = Algoritmos.RandomWalk_Suavizado(mapa, semilla, configurarMapa[i].intervalo);
					break;



				case Algoritmo.PERLINNOISE_CUEVA:
					mapa = Algoritmos.GenerarArray(mapa, ancho, alto, false);
					mapa = Algoritmos.PerlinNoise_Cueva(mapa, semilla, configurarMapa[i].modificador, configurarMapa[i].conBordes);
					break;
				case Algoritmo.PERLINNOISE_CUEVA_MODIFICADO:
					mapa = Algoritmos.GenerarArray(mapa, ancho, alto, false);
					mapa = Algoritmos.PerlinNoise_Cueva(mapa, configurarMapa[i].modificador, configurarMapa[i].conBordes, configurarMapa[i].desplazamientoX, configurarMapa[i].desplazamientoY, semilla);
					break;


				case Algoritmo.RANDOMWALK_CUEVA:
					mapa = Algoritmos.GenerarArray(mapa, ancho, alto, false);
					mapa = Algoritmos.RandomWalk_Cueva(mapa, semilla, configurarMapa[i].porcentajeEliminar);
					break;


				case Algoritmo.RANDOMWALK_CUEVA_MODIFICADO:

					mapa = Algoritmos.GenerarArray(mapa, ancho, alto, false);
					mapa = Algoritmos.RandomWalk_Cueva(mapa, semilla, configurarMapa[i].porcentajeEliminar, configurarMapa[i].conBordes, configurarMapa[i].diagonal);
					break;

				case Algoritmo.TUNEL_VERTICAL:
					mapa = Algoritmos.GenerarArray(mapa, ancho, alto, false);
					mapa = Algoritmos.TunelDireccional(mapa, semilla, configurarMapa[i].minAncho, configurarMapa[i].maxAncho, configurarMapa[i].aspereza);
					break;

				case Algoritmo.TUNEL_VERTICAL_MODIFICADO:
					mapa = Algoritmos.GenerarArray(mapa, ancho, alto, false);
					mapa = Algoritmos.TunelDireccional(mapa, semilla, configurarMapa[i].minAncho, configurarMapa[i].maxAncho, configurarMapa[i].aspereza, configurarMapa[i].desplazamientoMax, configurarMapa[i].desplazamiento);
					break;

				case Algoritmo.TUNEL_HORIZONTAL:
					mapa = Algoritmos.GenerarArray(mapa, ancho, alto, false);
					mapa = Algoritmos.TunelDireccional_Horizontal(mapa, semilla, configurarMapa[i].minAncho, configurarMapa[i].maxAncho, configurarMapa[i].aspereza);
					break;

				case Algoritmo.TUNEL_HORIZONTAL_MODIFICADO:
					mapa = Algoritmos.GenerarArray(mapa, ancho, alto, false);
					mapa = Algoritmos.TunelDireccional_Horizontal(mapa, semilla, configurarMapa[i].minAncho, configurarMapa[i].maxAncho, configurarMapa[i].aspereza, configurarMapa[i].desplazamientoMax, configurarMapa[i].desplazamiento);
					break;

				case Algoritmo.MAPA_ALEATORIO:
					mapa = Algoritmos.GenerarMapaAleatorio(ancho, alto, semilla, configurarMapa[i].porcentajeRellenoFloat, configurarMapa[i].conBordes);
					break;

				case Algoritmo.AUTOMATA_CELULAR_MOORE:
					mapa = Algoritmos.GenerarMapaAleatorio(ancho, alto, semilla, configurarMapa[i].porcentajeRellenoFloat, configurarMapa[i].conBordes);
					mapa = Algoritmos.AutomataCelularMoore(mapa, configurarMapa[i].numeroPasadas);
					break;


				case Algoritmo.AUTOMATA_CELULAR_VONNEUMAN:
					mapa = Algoritmos.GenerarMapaAleatorio(ancho, alto, semilla, configurarMapa[i].porcentajeRellenoFloat, configurarMapa[i].conBordes);
					mapa = Algoritmos.AutomataCelularVonNeuman(mapa, configurarMapa[i].numeroPasadas, configurarMapa[i].conBordes);
					break;

			}


			//Añadir el mapa a la lista de mapas
			listaMapas.Add(mapa);
		}


		//Permite que los mapas esten en el mismo Mapa de Losetas sin superponerses
		// Hay que calcular el desplazamiento a aplicar
		Vector2Int desplazamiento = new Vector2Int(-ancho / 2, (-alto / 2) - 1);


		//Work through the list to generate all maps
		foreach (int[,] map in listaMapas)
		{
			VisualizarMapa.DibujarMapa(map, mapaDeLosetas, loseta, desplazamiento);
			desplazamiento.y += -alto + 1;
		}




	}

	
	//------------------------------------------------------------------------------------------
	public void LimpiarMapa()
	{
		Debug.Log("Limpiando mapa") ;

		mapaDeLosetas.ClearAllTiles();

	}

	//------------------------------------------------------------------------------------------
}


//-------------------------------------------------------------------------------------------------
// clase EDITORRRR
//-------------------------------------------------------------------------------------------------

[CustomEditor(typeof(GeneradorMultiple))]
public class GeneradorMultipleEditor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		//Create a reference to our script
		GeneradorMultiple generadorMultiple = (GeneradorMultiple)target;

		//List of editors to only show if we have elements in the map settings list
		List<Editor> editorMapa = new List<Editor>();

		for (int i = 0; i < generadorMultiple.configurarMapa.Count; i++)
		{
			if (generadorMultiple.configurarMapa[i] != null)
			{
				Editor editorMapaMarco = CreateEditor(generadorMultiple.configurarMapa[i]);
				editorMapa.Add(editorMapaMarco);
			}
		}
		//If we have more than one editor in our editor list, draw them out. Also draw the buttons
		if (editorMapa.Count > 0)
		{
			EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
			for (int i = 0; i < editorMapa.Count; i++)
			{
				editorMapa[i].OnInspectorGUI();
			}

			if (GUILayout.Button("Generar mapa"))
			{
				generadorMultiple.GenerarMapa();
			}


			if (GUILayout.Button("Limpiar mapa"))
			{
				generadorMultiple.LimpiarMapa();
			}
		}
	}
}
