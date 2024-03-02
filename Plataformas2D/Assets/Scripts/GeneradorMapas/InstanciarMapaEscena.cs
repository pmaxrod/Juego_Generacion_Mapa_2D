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

        InstanciarPersonaje();

        InstanciarMonedas();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void InstanciarPersonaje()
    {
        GameObject personajeInst = personaje.objeto;

        personaje.objeto.transform.position = new Vector2(personaje.GetX(), personaje.GetY());

        Instantiate(personaje.objeto, personaje.objeto.transform);
    }

    private void InstanciarMonedas()
    {
        DatosObjeto datos = moneda.datos;

        moneda.objeto.transform.position = new Vector2(moneda.GetX(), moneda.GetY());

        for (int i = 0; i < datos.cantidad; i++)
        {
            Instantiate(moneda.objeto, moneda.objeto.transform);
        }

    }
}
