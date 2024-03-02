using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjetoMapa
{
    public GameObject objeto;
    public DatosObjeto datos;

    public float GetX()
    {
        return datos.x;
    }

    public float GetY()
    {
        return datos.y;
    }

    public int GetCantidad()
    {
        return datos.cantidad;
    }

}
