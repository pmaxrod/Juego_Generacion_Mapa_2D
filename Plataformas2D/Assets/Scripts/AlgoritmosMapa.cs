using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "NuevaListaAlgoritmos", menuName = "Objetos/Algoritmos Mapa", order = 1)]

public class AlgoritmosMapa : ScriptableObject
{
    [Tooltip("Algoritmos del mapa")]
    public List<Algoritmo> algoritmos;

    public Algoritmo AlgoritmoAleatorio()
    {
        int aleatorio = Random.Range(0, algoritmos.Count);

        return algoritmos[aleatorio];
    }
}
