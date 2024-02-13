using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class VisualizarMapa
{
    //-----------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Genera el mapa de losetas con la informci�n indicada en "_mapa"
    /// </summary>
    /// <param name="_mapa">Informaci�n que se utilizar� para generar el mapa de losetas. 1.- hay loseta. 0 --> no hay losetas</param>
    /// <param name="_mapaDeLosetas"> referencia al mapa de losetas dondes se generaran las losetas</param>
    /// <param name="_loseta">Loseta que se utilizar� para pintar el suelo en el mapa de losetas</param>
    public static void DibujarMapa(int[,] _mapa,  Tilemap _mapaDeLosetas, TileBase _loseta)
    {
        // limpiar el mapa de casillas para empezar con uno vac�o
        _mapaDeLosetas.ClearAllTiles();

        // Devuelve el �ndice del ultimo elemento X --> _mapa.GetUpperBound(0)
        for (int x = 0; x <= _mapa.GetUpperBound(0); x++)
            // Devuelve el �ndice del ultimo elemento Y --> _mapa.GetUpperBound(0)	
            for (int y = 0; y <= _mapa.GetUpperBound(1); y++)
            {
                // 1 = suelo, 0 = No hay suelo
                if (_mapa[x, y] == 1)
                {
                    //posici�n de la loseta y  la loseta que se va a colocar en dicha posici�n
                    _mapaDeLosetas.SetTile(new Vector3Int(x, y, 0), _loseta);
                }
            }

    }


    //-----------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Genera el mapa de losetas con la informci�n indicada en "_mapa"
    /// </summary>
    /// <param name="_mapa">Informaci�n que se utilizar� para generar el mapa de losetas. 1.- hay loseta. 0 --> no hay losetas</param>
    /// <param name="_mapaDeLosetas"> referencia al mapa de losetas dondes se generaran las losetas</param>
    /// <param name="_loseta">Loseta que se utilizar� para pintar el suelo en el mapa de losetas</param>
    /// <param name="_desplazamiento">El desplazamiento que hay que aplicar para visualizar los distintos mapas</param>
    public static void DibujarMapa(int[,] _mapa, Tilemap _mapaDeLosetas, TileBase _loseta, Vector2Int _desplazamiento)
    {

        // Devuelve el �ndice del ultimo elemento X --> _mapa.GetUpperBound(0)
        for (int x = 0; x <= _mapa.GetUpperBound(0); x++)
            // Devuelve el �ndice del ultimo elemento Y --> _mapa.GetUpperBound(0)	
            for (int y = 0; y <= _mapa.GetUpperBound(1); y++)
            {
                // 1 = suelo, 0 = No hay suelo
                if (_mapa[x, y] == 1)
                {
                    //posici�n de la loseta y  la loseta que se va a colocar en dicha posici�n
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

    //------------------------------------------------------------------------------

    public static void LimpiarMapa(Tilemap _mapaDeLosetas)
    {
        Debug.Log("Limpiando el mapa");

        _mapaDeLosetas.ClearAllTiles();
    }
}
