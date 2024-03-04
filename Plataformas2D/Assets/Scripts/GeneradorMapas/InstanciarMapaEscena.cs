using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstanciarMapaEscena : MonoBehaviour
{
    [SerializeField] private Generador generador;
    [Tooltip("Personaje")]
    [SerializeField] private ObjetoMapa personaje;

    [Tooltip("Moneda")]
    [SerializeField] private ObjetoMapa moneda;

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
        GameObject personajeInst = personaje.objeto;

        personaje.objeto.transform.position = new Vector2(personaje.GetX(), personaje.GetY());

        Instantiate(personaje.objeto, personaje.objeto.transform);
    }

    private void InstanciarMonedas(int[,] _mapa)
    {
        DatosObjeto datos = moneda.datos;

        for (int i = 0; i < datos.cantidad; i++)
        {
            float offsetX = Random.Range(0, _mapa.GetUpperBound(0));
            float offsetY = Random.Range(0, _mapa.GetUpperBound(1));

            Vector2 nuevaPosicion = new Vector2(moneda.GetX() + offsetX, moneda.GetY() + offsetY);

            moneda.objeto.transform.position = nuevaPosicion;

            for (int x = 0; x <= _mapa.GetUpperBound(0); x++)
            {
                for (int y = 0; y <= _mapa.GetUpperBound(1); y++)
                {
                    int totalVecinas = Algoritmos.LosetasVecinas(_mapa, x, y, true);

                    if (totalVecinas <= 4)
                    {
                        nuevaPosicion = new Vector2(moneda.GetX() + offsetX + 4, moneda.GetY() + offsetY + 4);
                    }
                    else
                    {
                        nuevaPosicion = new Vector2(moneda.GetX() + offsetX - 4, moneda.GetY() + offsetY - 4);
                    }
/*                    if (_mapa[x, y] == 1 && (x <= _mapa.GetUpperBound(0) || y <= _mapa.GetUpperBound(1)))
                    {
                        nuevaPosicion = new Vector2(moneda.GetX() + offsetX + 1, moneda.GetY() + offsetY + 1);
                    }
                    else if (_mapa[x, y] == 1 && (x == _mapa.GetUpperBound(0) || y == _mapa.GetUpperBound(1)))
                    {
                        nuevaPosicion = new Vector2(moneda.GetX() + offsetX - 1, moneda.GetY() + offsetY - 1);
                    }
*/
                    moneda.objeto.transform.position = nuevaPosicion;
                }
            }
            Instantiate(moneda.objeto, moneda.objeto.transform);
        }

    }
}
