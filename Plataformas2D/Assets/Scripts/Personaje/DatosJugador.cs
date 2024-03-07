using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatosJugador : MonoBehaviour
{
    public int puntuacion = 0;
    public int vidas = 3;
    public int monedas = 0;

    public static DatosJugador instance;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }
}
