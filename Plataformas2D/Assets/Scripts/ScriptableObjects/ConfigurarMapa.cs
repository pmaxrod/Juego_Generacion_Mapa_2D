using System.Collections;
using System.Collections.Generic;

using UnityEngine;


#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Este script convierte a la clase Algoritmo en un objeto programable.
/// Para crear un nuevo Algoritmo, haga clic con el boton derecho en la vista del proyecto y
/// seleccione Configuracion de la capa del mapa
/// Luego puede usar este script con Generador para generar su nivel
/// </summary>
///
public enum Algoritmo
{
    PERLINNOISE,
    PERLINNOISE_SUAVIZADO,
    RANDOMWALK,
    RANDOMWALK_SUAVIZADO,

    PERLINNOISE_CUEVA,
    PERLINNOISE_CUEVA_MODIFICADO,
    RANDOMWALK_CUEVA,
    RANDOMWALK_CUEVA_MODIFICADO,

    TUNEL_VERTICAL,
    TUNEL_HORIZONTAL,

    MAPA_ALEATORIO,
    AUTOMATA_CELULAR_MOORE,
    AUTOMATA_CELULAR_VONNEUMAN
}

[System.Serializable]
[CreateAssetMenu(fileName = "NuevoMapa", menuName = "Objetos/Configurar Mapa", order = 0)]


public class ConfigurarMapa : ScriptableObject
{
    public Algoritmo algoritmo;
    public bool semillaAleatoria = false;
    public float semilla;
    public int porcentajeRelleno;
    public int porcentajeSuavizar;
    public int porcentajeEliminar;
    public int intervalo;  // suavizar el algoritmo de Perlin Noise
    public float desplazamientoX, desplazamientoY; // desplazamiento para Perlin Noise Cueva
    public bool diagonal;  // modificar el algoritmo de RandomWalk Cueva

    // algoritmo modificador de tuneles
    public int minAncho, maxAncho, aspereza, desplazamientoMax, desplazamiento;
    public bool conBordes;
    public float modificador;

    public float porcentajeRellenoFloat; // Para los algoritmos de automatas
    public int numeroPasadas; // numero de pasadas para los automatas
}


//-----------------------------------------------------------------------------------------------------------------------------
//                             EDITORRR
//-----------------------------------------------------------------------------------------------------------------------------
//
[CustomEditor(typeof(ConfigurarMapa))]
public class ConfigurarMapaEditor : Editor
{
    public override void OnInspectorGUI()
    {
        ConfigurarMapa capaMapa = (ConfigurarMapa)target;
        GUI.changed = false;
        EditorGUILayout.LabelField(capaMapa.name, EditorStyles.boldLabel);

        capaMapa.algoritmo = (Algoritmo)EditorGUILayout.EnumPopup(new GUIContent("Metodo a generar", "El metodo de generacion que queremos usar para generar el mapa"), capaMapa.algoritmo);
        capaMapa.semillaAleatoria = EditorGUILayout.Toggle("Semilla Aleatoria", capaMapa.semillaAleatoria);

        //Only appear if we have the random seed set to false
        if (!capaMapa.semillaAleatoria)
        {
            capaMapa.semilla = EditorGUILayout.FloatField("Semilla", capaMapa.semilla);
        }

        //Shows different options depending on what algorithm is selected
        switch (capaMapa.algoritmo)
        {
            case Algoritmo.PERLINNOISE:
                //No additional Variables
                break;

            case Algoritmo.PERLINNOISE_SUAVIZADO:
                capaMapa.intervalo = EditorGUILayout.IntSlider("Intervalo", capaMapa.intervalo, 1, 10);
                break;

            case Algoritmo.PERLINNOISE_CUEVA:
                capaMapa.conBordes = EditorGUILayout.Toggle("Bordes con muros", capaMapa.conBordes);
                capaMapa.modificador = EditorGUILayout.Slider("Modificador", capaMapa.modificador, 0.0001f, 1.0f);
                break;

            case Algoritmo.PERLINNOISE_CUEVA_MODIFICADO:
                capaMapa.conBordes = EditorGUILayout.Toggle("Bordes con muros", capaMapa.conBordes);
                capaMapa.modificador = EditorGUILayout.Slider("Modificador", capaMapa.modificador, 0.0001f, 1.0f);
                capaMapa.desplazamientoX = EditorGUILayout.Slider("Desplazamiento X", capaMapa.desplazamientoX, 0f, 10f);
                capaMapa.desplazamientoY = EditorGUILayout.Slider("Desplazamiento Y", capaMapa.desplazamientoY, 0f, 10f);
                break;

            case Algoritmo.RANDOMWALK:
                //No additional Variables
                break;

            case Algoritmo.RANDOMWALK_SUAVIZADO:
                capaMapa.intervalo = EditorGUILayout.IntSlider("Ancho de la seccion", capaMapa.intervalo, 1, 10);
                break;

            case Algoritmo.RANDOMWALK_CUEVA:
                capaMapa.porcentajeEliminar = EditorGUILayout.IntSlider("Porcentaje a eliminar", capaMapa.porcentajeEliminar, 0, 100);
                break;

            case Algoritmo.RANDOMWALK_CUEVA_MODIFICADO:
                capaMapa.conBordes = EditorGUILayout.Toggle("Bordes con muros", capaMapa.conBordes);
                capaMapa.porcentajeEliminar = EditorGUILayout.IntSlider("Porcentaje a eliminar", capaMapa.porcentajeEliminar, 0, 100);
                capaMapa.diagonal = EditorGUILayout.Toggle("Diagonal", capaMapa.diagonal);
                break;

            case Algoritmo.TUNEL_VERTICAL:
                capaMapa.minAncho = EditorGUILayout.IntField("Ancho minimo", capaMapa.minAncho);
                capaMapa.maxAncho = EditorGUILayout.IntField("Ancho maximo", capaMapa.maxAncho);
                capaMapa.desplazamientoMax = EditorGUILayout.IntField("Maximo cambio", capaMapa.desplazamientoMax);
                capaMapa.aspereza = EditorGUILayout.IntSlider(new GUIContent("Aspereza", "Esto se compara con un numero aleatorio para determinar si podemos cambiar la posicion x actual de la ruta."), capaMapa.aspereza, 0, 1);
                capaMapa.desplazamiento = EditorGUILayout.IntSlider(new GUIContent("Movimiento", "Esto se compara con un numero aleatorio para determinar si podemos cambiar el ancho del tunel."), capaMapa.desplazamiento, 0, 1);
                break;

            case Algoritmo.TUNEL_HORIZONTAL:
                capaMapa.minAncho = EditorGUILayout.IntField("Ancho minimo", capaMapa.minAncho);
                capaMapa.maxAncho = EditorGUILayout.IntField("Ancho maximo", capaMapa.maxAncho);
                capaMapa.desplazamientoMax = EditorGUILayout.IntField("Maximo cambio", capaMapa.desplazamientoMax);
                capaMapa.aspereza = EditorGUILayout.IntSlider(new GUIContent("Aspereza", "Esto se compara con un numero aleatorio para determinar si podemos cambiar la posicion x actual de la ruta."), capaMapa.aspereza, 0, 1);
                capaMapa.desplazamiento = EditorGUILayout.IntSlider(new GUIContent("Movimiento", "Esto se compara con un numero aleatorio para determinar si podemos cambiar el ancho del tunel."), capaMapa.desplazamiento, 0, 1);
                break;

            case Algoritmo.MAPA_ALEATORIO:
                capaMapa.conBordes = EditorGUILayout.Toggle("Bordes con muros", capaMapa.conBordes);
                capaMapa.porcentajeRellenoFloat = EditorGUILayout.Slider("Porcentaje de relleno", capaMapa.porcentajeRellenoFloat, 0.0001f, 1.0f);
                break;

            case Algoritmo.AUTOMATA_CELULAR_MOORE:
                capaMapa.conBordes = EditorGUILayout.Toggle("Bordes con muros", capaMapa.conBordes);
                capaMapa.porcentajeRellenoFloat = EditorGUILayout.Slider("Porcentaje de relleno", capaMapa.porcentajeRellenoFloat, 0.0001f, 1.0f);
                capaMapa.numeroPasadas = EditorGUILayout.IntField("Numero de pasadas", capaMapa.numeroPasadas);
                break;

            case Algoritmo.AUTOMATA_CELULAR_VONNEUMAN:
                capaMapa.conBordes = EditorGUILayout.Toggle("Bordes con muros", capaMapa.conBordes);
                capaMapa.porcentajeRellenoFloat = EditorGUILayout.Slider("Porcentaje de relleno", capaMapa.porcentajeRellenoFloat, 0.0001f, 1.0f);
                capaMapa.numeroPasadas = EditorGUILayout.IntField("Numero de pasadas", capaMapa.numeroPasadas);
                break;
        }

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        AssetDatabase.SaveAssets();

        if (GUI.changed)
        {
            EditorUtility.SetDirty(capaMapa);
        }
    }
}
