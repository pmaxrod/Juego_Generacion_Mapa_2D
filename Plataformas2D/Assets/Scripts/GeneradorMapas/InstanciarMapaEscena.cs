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
        personaje.transform.position = PosicionProcedural(_mapa, personaje, true);

        Instantiate(personaje, personaje.transform);
    }

    private void InstanciarMonedas(int[,] _mapa)
    {
        for (int i = 0; i < cantidadMonedas; i++)
        {            
            moneda.transform.position = PosicionProcedural(_mapa, moneda);

            Instantiate(moneda, moneda.transform);
        }

    }

    private Vector2 PosicionProcedural(int[,] _mapa, GameObject _objeto, bool _personaje = false)
    {
        int xVector = Random.Range(0, _mapa.GetUpperBound(0));
        int yVector = 0;

        if (_personaje && _mapa[0,0] == 1) { }
        {
            xVector = 2;
            yVector = 2;
        }

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
        }

        yVector += Random.Range(-5, 10);

        Vector2 nuevaPosicion = new Vector2(xVector, yVector);

        return nuevaPosicion;

    }
}
