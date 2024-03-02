using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "NuevosDatosObjeto", menuName = "Objetos/Datos Objeto", order = 2)]
public class DatosObjeto : ScriptableObject
{
    public float x;
    public float y;
    public int cantidad;


}
