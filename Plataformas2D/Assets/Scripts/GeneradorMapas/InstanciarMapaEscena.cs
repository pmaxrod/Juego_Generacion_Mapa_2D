using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstanciarMapaEscena : MonoBehaviour
{
    [SerializeField] private Generador generador;
    [SerializeField] private GameObject personaje;
    [Header("Coordenadas X e Y")]
    [SerializeField] private float xPersonaje;
    [SerializeField] private float yPersonaje;

    // Start is called before the first frame update
    void Start()
    {
        generador.GenerarMapa();

        InstanciarPersonaje();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void InstanciarPersonaje()
    {
        personaje.transform.position = new Vector2(xPersonaje, yPersonaje);

        Instantiate(personaje, personaje.transform);
    }
}
