using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstanciarMapaEscena : MonoBehaviour
{
    [SerializeField] private Generador generador;
    [Tooltip("Personaje")]
    [SerializeField] private GameObject personaje;

    [Tooltip("Moneda")]
    [SerializeField] private GameObject moneda;
    [Tooltip("Cantidad de monedas")]
    [SerializeField] private int cantidadMonedas;

    [Tooltip("Fin del Nivel")]
    [SerializeField] private GameObject finNivel;

    // Start is called before the first frame update
    void Start()
    {
        generador.GenerarMapa();

        InstanciarPersonaje(generador.mapa);

        InstanciarMonedas(generador.mapa);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void InstanciarPersonaje(int[,] _mapa)
    {
        personaje.transform.position = PosicionProceduralPersonaje(_mapa, personaje);

        Instantiate(personaje, personaje.transform);
    }

    private void InstanciarMonedas(int[,] _mapa)
    {
        for (int i = 0; i < cantidadMonedas; i++)
        {
            moneda.transform.position = PosicionProceduralMoneda(_mapa, moneda);

            Instantiate(moneda, moneda.transform);
        }

    }

    private Vector2 PosicionProceduralMoneda(int[,] _mapa, GameObject _objeto)
    {
        int xVector = Random.Range(0, _mapa.GetUpperBound(0));
        int yVector = 0;

        // Para obtener la altura minima del objeto a instanciar
        for (int x = 0; x <= _mapa.GetUpperBound(0); x++)
        {
            for (int y = 0; y <= _mapa.GetUpperBound(1); y++)
            {
                if (_mapa[x, y] == 1 && (x == 0 || y == 0 || x == _mapa.GetUpperBound(0) || y == _mapa.GetUpperBound(1)))
                {
                    yVector = y;
                }

                if (xVector == x && _mapa[x, y] == 1 && (x == 0 || y == 0 || x == _mapa.GetUpperBound(0) || y == _mapa.GetUpperBound(1)))
                {
                    xVector--;
                }
                else if (xVector == x && _mapa[x, y] == 1 && (x == 0 || y == 0 || x != _mapa.GetUpperBound(0) || y != _mapa.GetUpperBound(1)))
                {
                    xVector++;
                }
            }
            yVector += Random.Range(-5, 10);
        }

        Vector2 nuevaPosicion = new Vector2(xVector, yVector);

        return nuevaPosicion;

    }

    private Vector2 PosicionProceduralPersonaje(int[,] _mapa, GameObject _objeto)
    {
        int xVector = _mapa.GetUpperBound(0) / 4;
        int yVector = _mapa.GetUpperBound(0) / 4;
        int alturaMinima = 0;

        for (int x = 0; x <= _mapa.GetUpperBound(0); x++)
        {
            for (int y = 0; y <= _mapa.GetUpperBound(1); y++)
            {
                // Para obtener la altura minima del objeto a instanciar
                if (_mapa[x, y] == 1 && (x == 0 || y == 0 || x == _mapa.GetUpperBound(0) || y == _mapa.GetUpperBound(1)))
                {
                    alturaMinima = y;
                }

                if (x == xVector && generador.configurarMapa.algoritmo == Algoritmo.RANDOMWALK_CUEVA_MODIFICADO && _mapa[x, y] == 1 && (y < alturaMinima))
                {
                    yVector--;
                }

                if (x == xVector && generador.configurarMapa.algoritmo != Algoritmo.RANDOMWALK_CUEVA_MODIFICADO && _mapa[x, y] == 1 && (y < alturaMinima))
                {
                    yVector++;
                }

            }
        }

        Vector2 nuevaPosicion = new Vector2(xVector, yVector);

        return nuevaPosicion;

    }
}
