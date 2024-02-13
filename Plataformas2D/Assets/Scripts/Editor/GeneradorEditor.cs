using System.Collections;
using System.Collections.Generic;
using UnityEngine;


#if UNITY_EDITOR
using UnityEditor;
#endif



//-------------------------------------------------------------------------------------------------
// clase EDITORRRR
//-------------------------------------------------------------------------------------------------
// Esta clase es el Editor del componente Generador
[CustomEditor(typeof(Generador))]

public class GeneradorEditor : Editor
{
    public override void  OnInspectorGUI()
    {
        // Dibuja los componentes de las variables públicas de la clase Generador
        base.OnInspectorGUI();

        // de esta forma obtenemos el componente Generador
        Generador generador = (Generador)target;

        if (generador.configurarMapa != null)
        {
            Editor configurarMapaEditor = CreateEditor(generador.configurarMapa);
            configurarMapaEditor.OnInspectorGUI();

            if (GUILayout.Button("Generar Mapa"))
            {
                generador.GenerarMapa();

            }

            if (GUILayout.Button("Limpiar Mapa"))
            {
                generador.LimpiarMapa();
            }

        }
    }
}
