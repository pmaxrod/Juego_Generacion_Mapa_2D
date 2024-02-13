using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
/// <summary>
/// Vista
/// </summary>
public class VisualizarMapa
{

    public static void LimpiarMapa(Tilemap _mapaDeLosetas)
    {
        _mapaDeLosetas.ClearAllTiles();
    }

    //---------------------------------------------------//

    public static void MostrarMapa(int[,] _mapa, Tilemap _mapaDeLosetas, TileBase _loseta)
    {
        // GetUpperBound(0) - eje x; GetUpperBound(1) - eje y
        for (int x = 0; x <= _mapa.GetUpperBound(0); x++)
        {
            for (int y = 0; y <= _mapa.GetUpperBound(1); y++)
            {
                if (_mapa[x, y] == 1) // 1 tiene suelo, 0 no tiene suelo
                {
                    _mapaDeLosetas.SetTile(new Vector3Int(x, y, 0), _loseta);
                }
            }
        }

    }
	
	 //---------------------------------------------------//

	 public static void DibujarMapa(int[,] _mapa, Tilemap _mapaDeLosetas, TileBase _loseta, Vector2Int _desplazamiento)
    {

        // Devuelve el indice del ultimo elemento X --> _mapa.GetUpperBound(0)
        for (int x = 0; x <= _mapa.GetUpperBound(0); x++)
            // Devuelve el indice del ultimo elemento Y --> _mapa.GetUpperBound(0)	
            for (int y = 0; y <= _mapa.GetUpperBound(1); y++)
            {
                // 1 = suelo, 0 = No hay suelo
                if (_mapa[x, y] == 1)
                {
                    //posicion de la loseta y  la loseta que se va a colocar en dicha posicion
                    _mapaDeLosetas.SetTile(new Vector3Int(x + _desplazamiento.x, y + _desplazamiento.y, 0), _loseta);
                }
            }

    }
	
	//------------------------------------------------------------------------------
    public static void DibujarMapaFondo(int[,] _mapa, Tilemap _mapaDeLosetas, TileBase _loseta)
    {
        Debug.Log("Generando mapa de Fondo");

        bool dibujada = false;

        _mapaDeLosetas.ClearAllTiles();

        for (int x = 0; x <= _mapa.GetUpperBound(0); x++)
        {
            dibujada = false;

            for (int y = _mapa.GetUpperBound(1) - 1; y >= 0 && !dibujada; y--)
            {
                if (_mapa[x, y] == 1 && _mapa[x, y + 1] == 0)
                {
                    _mapaDeLosetas.SetTile(new Vector3Int(x, y + 1, 0), _loseta);
                    dibujada = true;
                }
            }

        }

    }

}
