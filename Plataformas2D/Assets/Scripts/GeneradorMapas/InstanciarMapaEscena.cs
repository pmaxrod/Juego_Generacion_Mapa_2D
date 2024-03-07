using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

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

    [Tooltip("Fondo")]
    [SerializeField] private GameObject fondo;

    Vector2[] casillasVacias;

    // Start is called before the first frame update
    void Start()
    {
        generador.GenerarMapa();

        casillasVacias = CasillasVacias(generador.mapa);

        InstanciarObjetos();

        InstanciarPersonaje();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void InstanciarPersonaje()
    {
        personaje.transform.position = casillasVacias[0];

        Instantiate(personaje, personaje.transform);
    }

    private void InstanciarObjetos()
    {
        fondo.transform.localScale = generador.GetDimensiones();
        InstanciarMonedas();

        InstanciarFinNivel();
    }

    private void InstanciarMonedas()
    {
        for (int i = 0; i < cantidadMonedas; i++)
        {
            int posicionAleatoria = Random.Range(i, casillasVacias.Length - 1);
            moneda.transform.position = casillasVacias[posicionAleatoria];
            Instantiate(moneda, moneda.transform);
        }

    }

    private void InstanciarFinNivel()
    {
        finNivel.transform.position = generador.configurarMapa.algoritmo != Algoritmo.TUNEL_VERTICAL ? casillasVacias[casillasVacias.Length - 1] : generador.GetDimensiones();

        Instantiate(finNivel, finNivel.transform);
    }


    private Vector2[] CasillasVacias(int[,] _mapa)
    {
        List<Vector2> casillasVacias = new List<Vector2>();

        for (int x = 0; x <= _mapa.GetUpperBound(0); x++)
        {
            for (int y = 0; y <= _mapa.GetUpperBound(1); y++)
            {
                if (_mapa[x, y] == 0)
                {
                    casillasVacias.Add(new Vector2(x, y));
                    Debug.Log("x:" + x + " y:" + y);

                }
            }
        }
        return casillasVacias.ToArray();
    }
}
