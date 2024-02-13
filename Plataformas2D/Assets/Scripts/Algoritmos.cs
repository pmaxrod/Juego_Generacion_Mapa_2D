using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Algoritmos 
{
    //------------------------------------------------------------------------------
    /// <summary>
    /// Genera un array bidimensional
    /// </summary>
    /// <param name="_ancho"> ancho del mapa  2D</param>
    /// <param name="_alto"> al to del mapa 2D</param>
    /// <param name="_vacio"> Verdadero si queremos iniciarlizarlo a cero. Si no a uno.</param>
    /// <returns> El mapa 2D generado</returns>
    public static int[,] GenerarArray( int [,] _mapa, int _ancho, int _alto, bool _vacio)
    {
        

        for( int x = 0; x < _ancho; x++)
            for( int y = 0; y < _alto; y++)
            {
                _mapa[x, y] = _vacio ? 0 : 1;
                /*
                if (_vacio)             
                    _mapa[x, y] = 0;
                
                else
                    _mapa[x, y] = 1;
                */
            }

        return _mapa;
    }







    //-------------------------------------------------------------------------------------------------------------------------------------
    //           ALGORITMOS BÁSICOS
    //-------------------------------------------------------------------------------------------------------------------------------------

    /// <summary>
    /// Genera el terreno basándose en el Perlin Noise
    /// </summary>
    /// <param name="_mapa">El array a modificar donde se guardará el terreno generado</param>
    /// <param name="_semilla">La semilla que se utiliza para generar el mapa</param>
    /// <returns>El array modificado con el terreno generado</returns>
    public static int[,] PerlinNoise( int[,] _mapa, float _semilla)
    {
        // Altura a la se va a poner el suelo
        int nuevaAltura;

        for( int x = 0; x <= _mapa.GetUpperBound(0); x++)
        {
            // conseguimos la altura para cada X del mapa
            // redondea 
            nuevaAltura = Mathf.FloorToInt(Mathf.PerlinNoise(x, _semilla) * _mapa.GetUpperBound(1));

            for( int y = nuevaAltura; y>=0; y--)
            {
                _mapa[x, y] = 1;
            }
        }

        return _mapa;
    }



    //------------------------------------------------------------------------------
    public static int[,] PerlinNoise_Suavizado(int[,] _mapa, float _semilla, int _intervalo)
    {
        if (_intervalo > 1)
        {
            // Utilizados en el proceso de suavizado
            Vector2Int posActual, posAnterior;

            // los puntos correspondientes para el suavizado. Una lista para cada eje.
            List<int> ruidoX = new List<int>();
            List<int> ruidoY = new List<int>();

            int nuevoPunto, puntos;

            // Genera el ruido
            // dimension de la x --> _mapa.GetUpperBound(0)
            for (int x = 0; x <= _mapa.GetUpperBound(0) + _intervalo; x += _intervalo)
            {
                // conseguimos la altura
                nuevoPunto = Mathf.FloorToInt((Mathf.PerlinNoise(x, _semilla)) * _mapa.GetUpperBound(1));

                ruidoY.Add(nuevoPunto);
                ruidoX.Add(x);

            }

            puntos = ruidoY.Count;

            // empezamos en la primera posición para así tener disponible una posición 
            for (int i = 1; i < puntos; i++)
            {
                posActual = new Vector2Int(ruidoX[i], ruidoY[i]);
                posAnterior = new Vector2Int(ruidoX[i - 1], ruidoY[i - 1]);

                // calcular la recta que une a los 2 puntos
                Vector2 diferencia = posActual - posAnterior;

                // cuanto habría que bajar en cada paso, en cada x
                float cambioEnAltura = diferencia.y / _intervalo;

                // Guardamos la altura actual
                float alturaActual = posAnterior.y;


                // no nos podemos pasar de la posición actual y de la dismensiones del mapa
                // Generar los bloques del intervalo de la x anterior a la x actual
                for (int x = posAnterior.x; x < posActual.x && x <= _mapa.GetUpperBound(0); x++)
                {
                    // empezamos desde la altura actual
                    for (int y = Mathf.FloorToInt(alturaActual); y >= 0; y--)
                    {
                        _mapa[x, y] = 1;
                    }

                    alturaActual += cambioEnAltura;

                }

            }
        }
        else
        {
            // no podemos suavisar, así  que devolvemos el resultado del mapa Perlinnoise
            _mapa = PerlinNoise(_mapa, _semilla);
        }

        return _mapa;

    }


    //------------------------------------------------------------------------------
    /// <summary>
    /// Generar tereno usando el algoritno RandomWalk( paseo aleatorio)
    /// <param name="_mapa"> El mapa que vamos a editar</param>
    /// <param name="_semilla">La semilla para la generación RandomWalk</param>
    /// <returns>Mapa modificado</returns>
    /// </summary>
    public static int[,] RandomWalk(int[,] _mapa, float _semilla)
    {
        // La semilla de nuestro random
        Random.InitState(_semilla.GetHashCode()); // obtiene el valor entero del valor real

        // Altura desde la cual vamos a dibujar el mapa
        int ultimaAltura = Random.Range(0, _mapa.GetUpperBound(1));

        // Recorremos el mapa a lo ancho - X
        for (int x = 0; x <= _mapa.GetUpperBound(0); x++)
        {
            // 0 Sube, 1 Baja, 2 igual
            int sigMovimiento = Random.Range(0, 3);

            // SUBIR
            if (sigMovimiento == 0 && ultimaAltura < _mapa.GetUpperBound(1))
            {
                ultimaAltura++;
            }

            // BAJAR
            else if (sigMovimiento == 1 && ultimaAltura > 0)
            {
                ultimaAltura--;
            }

            // no cambia la altura si sigMoviento == 2


            for (int y = ultimaAltura; y >= 0; y--)
            {
                _mapa[x, y] = 1;
            }


        }

        return _mapa;
    }


    //------------------------------------------------------------------------------

    public static int[,] RandomWalk_Suavizado(int[,] _mapa, float _semilla, int _minAnchoSeccion)
    {
        // La semilla de nuestro random
        Random.InitState(_semilla.GetHashCode()); // obtiene el valor entero del valor real

        // Altura desde la cual vamos a dibujar el mapa
        int ultimaAltura = Random.Range(0, _mapa.GetUpperBound(1));

        // llevar la cuenta del ancho de la sección actual
        int anchoSeccion = 0;

        // Recorremos el mapa a lo ancho --> X
        for (int x = 0; x <= _mapa.GetUpperBound(0); x++)
        {
            // solo subiremos o bajaremos, cuando terminemos de dibujar la sección actual
            if (anchoSeccion >= _minAnchoSeccion)
            {
                // 0 Sube, 1 Baja, 2 igual
                int sigMovimiento = Random.Range(0, 3);

                // SUBIR
                if (sigMovimiento == 0 && ultimaAltura < _mapa.GetUpperBound(1))
                {
                    ultimaAltura++;
                }
                // BAJAR
                else if (sigMovimiento == 1 && ultimaAltura > 0)
                {
                    ultimaAltura--;
                }

                // empezamos una nueva sección de suelo con la misma altura
                anchoSeccion = 0; // volver a poner a cero la sección
            }

            //Hemos procesado otro bloque de la seccion actual
            anchoSeccion++;

            // rellenar LA COLUMNA X desde la ultimaAltura hasta abajo
            for (int y = ultimaAltura; y >= 0; y--)
            {
                _mapa[x, y] = 1;
            }

        }
        return _mapa;
    }

    #region CUEVAS

    //-------------------------------------------------------------------------------------------------------------------------------------
    //           ALGORITMOS DE CUEVAS
    //-------------------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 
    /// </summary>
    /// <param name="_mapa"> El mapa a modificar</param>
    /// <param name="_modificador">El valor por el cual multiplicamos la posición para obtener un valor del Perlin Noise. Mapa más grande o mas pequeño</param>
    /// <param name="_bordesSonMuros">Si es verdadero, los bordes serían muros</param>
    /// <returns>Mapa modificado</returns>

    public static int[,] PerlinNoise_Cueva(int[,] _mapa, float _semilla, float _modificador, bool _bordesSonMuros)
    {
        // almacena si hay que poner hueco o suelo

        int nuevoPunto;

        for (int x = 0; x <= _mapa.GetUpperBound(0); x++)
        {
            for (int y = 0; y <= _mapa.GetUpperBound(1); y++)
            {
                // comprobar si los bordes son muros o no

                if (_bordesSonMuros && (x == 0 || y == 0 || x == _mapa.GetUpperBound(0) || y == _mapa.GetUpperBound(1)))
                {
                    _mapa[x, y] = 1;
                }
                else
                {
                    // se redonde el resultado de la funcion PerlinNoise
                    nuevoPunto = Mathf.RoundToInt(Mathf.PerlinNoise(x * _modificador + _semilla, y *  _modificador+_semilla));
                    _mapa[x, y] = nuevoPunto;
                }
            }
        }

        return _mapa;
    }

    //-------------------------------------------------------------------------------------------------------------------------------------
    // desplazar el mapa según los valores de los offSet
    /// <summary>
    /// Crea una cueva usando el Perlin Noise para el proceso de generación
    /// </summary>
    /// <param name="_mapa"> El mapa que se va a modificar</param>
    /// <param name="_modificador">El valor por el cual multiplicamos la posición para obtener un valor del Perlin Noise. Mapa más grande o mas pequeño</param>
    /// <param name="_bordesSonMuros">Si es verdadero, los bordes serían muros</param>
    /// <param name="_offSetX">Desplazamiento en X para el Perlin Noise</param>
    /// <param name="_offSetY">Desplazamiento en Y para el Perlin Noise</param>
    /// <param name="_semilla">Se usará para situarnos en un X,Y  (X = Y = semilla) en el Perlin Noise</param>
    /// <returns>El mapa con la cueva generada con el Perlin Noise</returns>
    public static int[,] PerlinNoise_Cueva(int[,] _mapa,  float _modificador, bool _bordesSonMuros, float _offSetX = 0f, float _offSetY = 0f, float _semilla = 0f)
    {
        // almacena si hay que poner hueco o suelo

        int nuevoPunto;

        for (int x = 0; x <= _mapa.GetUpperBound(0); x++)
        {
            for (int y = 0; y <= _mapa.GetUpperBound(1); y++)
            {
                // comprobar si los bordes son muros o no

                if (_bordesSonMuros && (x == 0 || y == 0 || x == _mapa.GetUpperBound(0) || y == _mapa.GetUpperBound(1)))
                {
                    _mapa[x, y] = 1;
                }
                else
                {
                    // se redondea el resultado de la funcion PerlinNoise
                    nuevoPunto = Mathf.RoundToInt(Mathf.PerlinNoise(x * _modificador + _offSetX + _semilla,
                                                                     y * _modificador + _offSetY + _semilla));
                    _mapa[x, y] = nuevoPunto;
                }
            }
        }

        return _mapa;
    }

    //-------------------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Crear una nueva cueva usando el algoritmo Random Walk
    /// </summary>
    /// <param name="_mapa"> El mapa a modificar</param>
    /// <param name="_semilla">La semilla para los números aleatorios</param>
    /// <param name="_porcentajeSueloEliminar">La cantidad de suelo que queremos quitar</param>
    /// <returns> El mapa modificado</returns>
    public static int[,] RandomWalk_Cueva(int[,] _mapa, float _semilla, float _porcentajeSueloEliminar)
    {
        // Las semilla de nuestro Random
        Random.InitState(_semilla.GetHashCode());


        // Definimos los límites
        int vMin = 0;
        int vMaxX = _mapa.GetUpperBound(0);
        int vMaxY = _mapa.GetUpperBound(1);
        int ancho = _mapa.GetUpperBound(0) + 1;
        int alto = _mapa.GetUpperBound(1) + 1;

        // Definir la posición de inicio en X y en Y
        int posX = Random.Range(vMin, vMaxX);
        int posY = Random.Range(vMin, vMaxY);

        // redondeamos el valor
        int cantidadLosetasEliminar = Mathf.FloorToInt(ancho * alto * _porcentajeSueloEliminar/100);


        // para contar las losetas que llevamos elimnadas
        int losetasEliminadas = 0;


        while (losetasEliminadas < cantidadLosetasEliminar)
        {
            // si hay suelo
            if (_mapa[posX, posY] == 1)
            {
                _mapa[posX, posY] = 0; // elimnamos loseta
                losetasEliminadas++;
            }

            // dirección aleatoria a seguir
            int dirAleatoria = Random.Range(0, 4);
            switch (dirAleatoria)
            {
                case 0:
                    // Arriba
                    posY++;
                    break;
                case 1:
                    // Abajo
                    posY--;
                    break;
                case 2:
                    // Izquierda
                    posX--;
                    break;
                case 3:
                    // derecha
                    posX++;
                    break;
            }

            // ver los límites del mapa
            // para asegurarnos que NO nos salimos del área de trabajo
            posX = Mathf.Clamp(posX, vMin, vMaxX);
            posY = Mathf.Clamp(posY, vMin, vMaxY);

        } // fin del while       

        return _mapa;
    }

    //-------------------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Crear una nueva cueva usando el algoritmo Random Walk
    /// </summary>
    /// <param name="_mapa"> El mapa a modificar</param>
    /// <param name="_semilla">La semilla para los números aleatorios</param>
    /// <param name="_porcentajeSueloEliminar">La cantidad de suelo que quermos quitar</param>
    /// <param name="_bordesSonMuros">Si hay que mantener los bordes siempre con suelo</param>
    /// <param name="_movDiagonal">Permite el movimiento en diagonal</param>
    /// <returns> El mapa modificado</returns>
    public static int[,] RandomWalk_Cueva(int[,] _mapa, float _semilla, float _porcentajeSueloEliminar, bool _bordesSonMuros = true, bool _movDiagonal = false)
    {
        // Las semilla de nuestro Random
        Random.InitState(_semilla.GetHashCode());


        // Definimos los límites
        int vMin = 0;
        int vMaxX = _mapa.GetUpperBound(0);
        int vMaxY = _mapa.GetUpperBound(1);
        int ancho = _mapa.GetUpperBound(0) + 1;
        int alto = _mapa.GetUpperBound(1) + 1;

        // mejora - bordes son muros
        // excluimos los bordes del radio de acción
        if (_bordesSonMuros)
        {
            vMin++;
            vMaxX--;
            vMaxY--;

            ancho -= 2;
            alto -= 2;
        }

        // Definir la posición de inicio en X y en Y
        int posX = Random.Range(vMin, vMaxX);
        int posY = Random.Range(vMin, vMaxY);

        // redondeamos el valor
        int cantidadLosetasEliminar = Mathf.FloorToInt(ancho * alto * _porcentajeSueloEliminar/100);


        // para contar las losetas que llevamos elimnadas
        int losetasEliminadas = 0;


        while (losetasEliminadas < cantidadLosetasEliminar)
        {
            // si hay suelo
            if (_mapa[posX, posY] == 1)
            {
                _mapa[posX, posY] = 0; // elimnamos loseta
                losetasEliminadas++;
            }

            if (_movDiagonal)
            {
                // si nos podemos mover en diagonal
                int dirAleatoriaX = Random.Range(-1, 2); // devuelve -1, 0 o 1
                int dirAleatoriaY = Random.Range(-1, 2); // devuelve -1, 0 o 1

                posX += dirAleatoriaX;
                posY += dirAleatoriaY;

            }
            else
            {
                // dirección aleatoria a seguir
                int dirAleatoria = Random.Range(0, 4);
                switch (dirAleatoria)
                {
                    case 0:
                        // Arriba
                        posY++;
                        break;
                    case 1:
                        // Abajo
                        posY--;
                        break;
                    case 2:
                        // Izquierda
                        posX--;
                        break;
                    case 3:
                        // derecha
                        posX++;
                        break;
                }
            }
            // ver los limites del mapa
            // para asegurarnos que no salimos del area de trabajo
            posX = Mathf.Clamp(posX, vMin, vMaxX);
            posY = Mathf.Clamp(posY, vMin, vMaxY);

        } // fin del while



        return _mapa;
    }


    #endregion

    #region TUNELES
    //-------------------------------------------------------------------------------------------------------------------------------------
    //           ALGORITMOS DE TÚNELES
    //-------------------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Crea un túnel vertical de longitud el alto. Toma en cuenta la aspereza para ambiar el acnho del túnel
    /// </summary>
    /// <param name="_mapa"> El mapa a modificar</param>
    /// <param name="_semilla">La semilla para los números aleatorios</param>
    /// <param name="_anchoMin">Ancho mínimo del túnel</param>
    /// <param name="_anchoMax">Ancho máximo del túnel</param>
    /// <param name="_aspereza">La probabilidad que cambie el ancho cada paso de Y</param>
    public static int[,] TunelDireccional(int[,] _mapa, float _semilla, int _anchoMin, int _anchoMax, float _aspereza)
    {
        // este valor va desde su valor en negativo hasta el valor positivo
        // en este caso, con el valor 1, el ancho del túnel es 3 ( -1, 0, 1)
        int anchoTunel = 1;

        // posición de comienzo del mapa
        int x = _mapa.GetUpperBound(0) / 2; // la mitad del mapa

        // la semilla de nuestro random
        Random.InitState(_semilla.GetHashCode());

        // recorremos la Y, ya que el túnel es vertical
        for (int y = 0; y <= _mapa.GetUpperBound(1); y++)
        {
            // Generamos esta parte del túnel
            for (int i = -anchoTunel; i <= anchoTunel; i++)
            {
                _mapa[x + i, y] = 0; // quitar el suelo
            }

            // una _aspereza del 0, significa que no queremos que cambie el ancho del túnel
            // le damos la vuelta a _aspereza, para que cambie 1- 0, nunca cambia ya que 
            // el valor de Random.value nunca va a ser mayor que 1
            if (Random.value > 1 - _aspereza)
            {
                // cambio del ancho
                // Obtener aleatoriamente la cantidad que cambiaremos el ancho
                int cambioAncho = Random.Range(-_anchoMax, _anchoMax);
                anchoTunel += cambioAncho;

                // limitar el valor, para que este entre los límites establecidos
                anchoTunel = Mathf.Clamp(anchoTunel, _anchoMin, _anchoMax);
            }
        }
        return _mapa;
    }

    //-------------------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Crea un túnel vertical de longitud el alto. Toma en cuenta la aspereza para ambiar el acnho del túnel
    /// </summary>
    /// <param name="_mapa"> El mapa a modificar</param>
    /// <param name="_semilla">La semilla para los números aleatorios</param>
    /// <param name="_anchoMin">Ancho mínimo del túnel</param>
    /// <param name="_anchoMax">Ancho máximo del túnel</param>
    /// <param name="_aspereza">La probabilidad que cambie el ancho cada paso de Y</param>
    /// <param name="_desplazamientoMax">La cantidad máxima que podemos cambiar el punto central del túnel </param>
    /// <param name="_desplazamiento">La probabilidad de que cambie el punto central del túnel</param>
    /// <returns> El mapa conel tunel direccional vertical</returns>
    public static int[,] TunelDireccional(int[,] _mapa, float _semilla, int _anchoMin, int _anchoMax, float _aspereza, int _desplazamientoMax, float _desplazamiento)
    {
        // este valor va desde su valor en negativo hasta el valor positivo
        // en este caso, con el valor 1, el cncho del túnel es 3 ( -1, 0, 1)
        int anchoTunel = 1;

        // posición de comienzo del mapa
        int x = _mapa.GetUpperBound(0) / 2; // la mitad del mapa

        // la semilla de nuestro random
        Random.InitState(_semilla.GetHashCode());

        // recorremos la Y, ya que el túnel es vertical
        for (int y = 0; y <= _mapa.GetUpperBound(1); y++)
        {
            // Generamos esta parte del túnel
            for (int i = -anchoTunel; i <= anchoTunel; i++)
            {
                _mapa[x + i, y] = 0; // quitar el suelo
            }

            // una _aspereza del 0, significa que no queremos que cambie el ancho del túnel
            // le damos la vuelta a _aspereza, para que cambie 1- 0, nunca cambia ya que 
            // el valor de Random.value nunca va aser mayor que 1
            if (Random.value > 1 - _aspereza)
            {
                // cambio del ancho
                // Obtener aleatoriamente la cantidad que cambiaremos el ancho
                int cambioAncho = Random.Range(-_anchoMax, _anchoMax);
                anchoTunel += cambioAncho;

                // limitar el valor, para que este entre los límites establecidos
                anchoTunel = Mathf.Clamp(anchoTunel, _anchoMin, _anchoMax);
            }

            // Comprobar si cambiar la posición central del túnel
            if (Random.value > 1 - _desplazamiento)
            {
                // valor aleatorio de desplazamiento de la posición central
                int cambioDesplazamiento = Random.Range(-_desplazamientoMax, _desplazamientoMax);

                x += cambioDesplazamiento;

                // la x no puede salir de los límites del mapa
                x = Mathf.Clamp(x, _anchoMax + 1, _mapa.GetUpperBound(0) - _anchoMax);
            }

        }
        return _mapa;
    }


    //-------------------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Crea un túnel hortizontal de longitud el alto. Toma en cuenta la aspereza para ambiar el acnho del túnel
    /// </summary>
    /// <param name="_mapa"> El mapa a modificar</param>
    /// <param name="_semilla">La semilla para los números aleatorios</param>
    /// <param name="_anchoMin">Ancho mínimo del túnel</param>
    /// <param name="_anchoMax">Ancho máximo del túnel</param>
    /// <param name="_aspereza">La probabilidad que cambie el ancho cada paso de Y</param>
    /// <returns> El mapa con el tunel direccional horizontal</returns>

    public static int[,] TunelDireccional_Horizontal(int[,] _mapa,  float _semilla, int _anchoMin, int _anchoMax, float _aspereza)
    {
        // este valor va desde su valor en negativo hasta el valor positivo
        // en este caso, con el valor 1, el cncho del túnel es 3 ( -1, 0, 1)
        int anchoTunel = 1;

        // posición de comienzo del mapa
        int y = _mapa.GetUpperBound(1) / 2; // la mitad del mapa

        // la semilla de nuestro random
        Random.InitState(_semilla.GetHashCode());

        // recorremos la X, ya que el túnel es horizontal
        for (int x = 0; x <= _mapa.GetUpperBound(0); x++)
        {
            // Generamos esta parte del túnel
            for (int i = -anchoTunel; i <= anchoTunel; i++)
            {
                _mapa[x, y + i] = 0; // quitar el suelo
            }

            // una _aspereza del 0, significa que no queremos que cambie el ancho del túnel
            // le damos la vuelta a _aspereza, para que cambie 1- 0, nunca cambia ya que 
            // el valor de Random.value nunca va aser mayor que 1
            if (Random.value > 1 - _aspereza)
            {
                // cambio del ancho
                // Obtener aleatoriamente la cantidad que cambiaremos el ancho
                int cambioAncho = Random.Range(-_anchoMax, _anchoMax);
                anchoTunel += cambioAncho;

                // limitar el valor, para que este entre los límites establecidos
                anchoTunel = Mathf.Clamp(anchoTunel, _anchoMin, _anchoMax);
            }
        }


        return _mapa;
    }


    //-------------------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Crea un túnel hortizontal de longitud el alto. Toma en cuenta la aspereza para ambiar el acnho del túnel
    /// </summary>
    /// <param name="_mapa"> El mapa a modificar</param>
    /// <param name="_semilla">La semilla para los números aleatorios</param>
    /// <param name="_anchoMin">Ancho mínimo del túnel</param>
    /// <param name="_anchoMax">Ancho máximo del túnel</param>
    /// <param name="_aspereza">La probabilidad que cambie el ancho cada paso de Y</param>
    /// <param name="_desplazamientoMax">La cantidad máxima que podemos cambiar el punto central del túnel </param>
    /// <param name="_desplazamiento">La probabilidad de que cambie el punto central del túnel</param>
    /// <returns> El mapa conel tunel direccional horizontal</returns>
    public static int[,] TunelDireccional_Horizontal(int[,] _mapa, float _semilla, int _anchoMin, int _anchoMax, float _aspereza, int _desplazamientoMax, float _desplazamiento)
    {
        // este valor va desde su valor en negativo hasta el valor positivo
        // en este caso, con el valor 1, el cncho del túnel es 3 ( -1, 0, 1)
        int anchoTunel = 1;

        // posición de comienzo del mapa
        int y = _mapa.GetUpperBound(1) / 2; // la mitad del mapa

        // la semilla de nuestro random
        Random.InitState(_semilla.GetHashCode());

        // recorremos la X, ya que el túnel es horizontal
        for (int x = 0; x <= _mapa.GetUpperBound(0); x++)
        {
            // Generamos esta parte del túnel
            for (int i = -anchoTunel; i <= anchoTunel; i++)
            {
                _mapa[x, y + i] = 0; // quitar el suelo
            }

            // una _aspereza del 0, significa que no queremos que cambie el ancho del túnel
            // le damos la vuelta a _aspereza, para que cambie 1- 0, nunca cambia ya que 
            // el valor de Random.value nunca va aser mayor que 1
            if (Random.value > 1 - _aspereza)
            {
                // cambio del ancho
                // Obtener aleatoriamente la cantidad que cambiaremos el ancho
                int cambioAncho = Random.Range(-_anchoMax, _anchoMax);
                anchoTunel += cambioAncho;

                // limitar el valor, para que este entre los límites establecidos
                anchoTunel = Mathf.Clamp(anchoTunel, _anchoMin, _anchoMax);
            }

            // Comprobar si cambiar la posición central del túnel
            if (Random.value > 1 - _desplazamiento)
            {
                // valor aleatorio de desplazamiento de la posición central
                int cambioDesplazamiento = Random.Range(-_desplazamientoMax, _desplazamientoMax);

                y += cambioDesplazamiento;

                // la x no puede salir de los límites del mapa
                y = Mathf.Clamp(y, _anchoMax + 1, _mapa.GetUpperBound(1) - _anchoMax);
            }
        }


        return _mapa;
    }

    //-------------------------------------------------------------------------------------------------------------------------------------




    #endregion
    //-------------------------------------------------------------------------------------------------------------------------------------
    #region AUTOMATAS_CELULARES

    //-------------------------------------------------------------------------------------------------------------------------------------
    //    Algoritmos para automatas celulares
    //-------------------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Crea la base para las funciones avanzadas de autómatas celulares
    /// Usaremos el mapa en distintos métodos dependiendo del tipo de vecindario que queramos.
    /// </summary>
    /// <param name="_ancho">Ancho del mapa</param>
    /// <param name="_alto">Alto del mapa</param>
    /// <param name="_semilla">La semilla de los números aleatorios</param>
    /// <param name="_porcentajeRelleno">La cantidad que queremos que se llene el mapa. Valor entre 0 y 1.</param>
    /// <param name="_bordesSonMuros">Si es verdadero, los bordes serían muros</param>
    /// <returns>El mapa con el contenido aleatorio</returns>
    public static int[,] GenerarMapaAleatorio(int _ancho, int _alto, float _semilla, float _porcentajeRelleno, bool _bordesSonMuros)
    {
        // Establecer la semilla para los numeros aleatorios
        Random.InitState(_semilla.GetHashCode());

        int[,] mapa = new int[_ancho, _alto];

        // recorremos todas las posiciones del mapa
        for (int x = 0; x <= mapa.GetUpperBound(0); x++)
        {
            for (int y = 0; y <= mapa.GetUpperBound(1); y++)
            {
                if (_bordesSonMuros && (x == 0 || y == 0 || x == mapa.GetUpperBound(0) || y == mapa.GetUpperBound(1)))
                {
                    mapa[x, y] = 1;
                }
                else
                {
                    // ponemos suelo si el resultado del random es inferior que el procentaje
                    mapa[x, y] = (Random.value < _porcentajeRelleno) ? 1 : 0;
                }
            }
        }

        return mapa;
    }

    //-------------------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Calcular el total de losetas vecinas
    /// </summary>
    /// <param name="_mapa">El mapa donde comprobar las losetas</param>
    /// <param name="_x">La posición X de la loseta que se va a comprobar</param>
    /// <param name="_y">La posición Y de la loseta que se va a comprobar</param>
    /// <param name="_incluirDiagonal">Si hay que tener en cuenta las posiciones vecinas en diagonal</param>
    /// <returns>Total de losetas vecinas con suelo</returns>
    public static int LosetasVecinas(int[,] _mapa, int _x, int _y, bool _incluirDiagonales)
    {
        // contar las losetas vecinas
        int totalVecinas = 0;

        // recorrer las losetas vecinas de esta posición
        for (int vecinoX = _x - 1; vecinoX <= _x + 1; vecinoX++)
        {
            for (int vecinoY = _y - 1; vecinoY <= _y + 1; vecinoY++)
            {
                // comprorbar que estamos dentro del mapa
                if (vecinoX >= 0 && vecinoX <= _mapa.GetUpperBound(0) && vecinoY >= 0 && vecinoY <= _mapa.GetUpperBound(1))
                {
                    // ignorar la posición de central (_x,_y)
                    // sin incluir las diagonales
                    //
                    //   N
                    // N T N
                    //   N
                    //
                    // incluyendo las diagonales --> incluirDiagonales = true
                    //
                    // N N N
                    // N T N
                    // N N N
                    // 

                    if ((vecinoX != _x || vecinoY != _y) && (_incluirDiagonales || (vecinoX == _x || vecinoY == _y)))
                    {
                        // sumar las casillas que tienen 1, y así sabremos las casillas vecinas que tienen vecinas
                        totalVecinas += _mapa[vecinoX, vecinoY];
                    }
                }
            }
        }

        return totalVecinas;
    }


    //-------------------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Suaviza un mapa usando las reglas de vecindario de Moore
    /// Se tienen en cuenta todas las losetas vecinas (incluidas las diagonales)
    /// </summary>
    /// <param name="_mapa">El mapa a suavizar</param>
    /// <param name="totalDePasadas">la cantidad de pasadas que se hara</param>
    /// <returns>Devuelve el mapa suavizado</returns>
    public static int[,] AutomataCelularMoore(int[,] _mapa, int totalDePasadas)
    {
        for (int i = 0; i < totalDePasadas; i++)
        {
            for (int x = 0; x <= _mapa.GetUpperBound(0); x++)
            {
                for (int y = 0; y <= _mapa.GetUpperBound(1); y++)
                {
                    // Incluye las diagonales
                    int totalVecinas = LosetasVecinas(_mapa, x, y, true);

                    // Si tenemos mas de 4 vecinos, ponemos suelo - VIVE
                    if (totalVecinas > 4)
                    {
                        _mapa[x, y] = 1;
                    }

                    // Si tenemos menos de 4 vecinos, dejamos un hueco - MUERE
                    else if (totalVecinas < 4)
                    {
                        _mapa[x, y] = 0;
                    }

                    // si tenemos exactamente 4 vecino, no cambiamos nada
                }
            }
        }

        return _mapa;
    }


    //-------------------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 
    /// </summary>
    /// <param name="_mapa"></param>
    /// <param name="totalDePasadas"></param>
    /// <param name="_bordesSonMuros"></param>
    /// <returns></returns>  
     
    public static int[,] AutomataCelularVonNeuman(int[,] _mapa, int totalDePasadas, bool _bordesSonMuros)
    {
        for (int i = 0; i < totalDePasadas; i++)
        {
            for (int x = 0; x <= _mapa.GetUpperBound(0); x++)
            {
                for (int y = 0; y <= _mapa.GetUpperBound(1); y++)
                {
                    // NO Incluye las diagonales
                    int totalVecinas = LosetasVecinas(_mapa, x, y, false);

                    // si respetamos los bordes esta activado
                    if (_bordesSonMuros && (x == 0 || x == _mapa.GetUpperBound(0) || y == 0 || y == _mapa.GetUpperBound(1)))
                    {
                        _mapa[x, y] = 1;
                    }

                    // Si tenemos mas de 2 vecinos, ponemos suelo - VIVE
                    else if (totalVecinas > 2)
                    {
                        _mapa[x, y] = 1;
                    }

                    // Si tenemos menos de 2 vecinos, dejamos un hueco - MUERE
                    else if (totalVecinas < 2)
                    {
                        _mapa[x, y] = 0;
                    }

                    // si tenemos exactamente  vecino, no cambiamos nada
                }
            }
        }

        return _mapa;
    }


    #endregion
}
