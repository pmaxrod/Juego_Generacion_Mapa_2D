using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstanciarMapaEscena : MonoBehaviour
{
    [SerializeField] private Generador generador;

    // Start is called before the first frame update
    void Start()
    {
        generador.GenerarMapa();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
