using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlJugador : ObjetoFisico
{

    public float velocidadMaxima = 7;
    public float velocidadSalto = 7;

    private float numeroSaltos = 2;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private DatosJugador datosJugador;

    // Use this for initialization
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        datosJugador = GetComponent<DatosJugador>();
    }

   void Update()
    {
        velocidadObjetivo = Vector2.zero;
        ComputeVelocity();
        if (datosJugador.vidas < 1)
        {
            Morir();
        }

        if (transform.position.y < 0)
        {
            DatosJugador.instance.vidas = 0;
        }
    }

    protected override void ComputeVelocity()
    {
        Vector2 move = Vector2.zero;

        move.x = Input.GetAxis("Horizontal");


        if (Input.GetButtonDown("Jump") && numeroSaltos > 0)
        {
            velocidad.y = velocidadSalto;
        }
        else if (Input.GetButtonUp("Jump"))
        {
            if (velocidad.y > 0)
            {
                velocidad.y = velocidad.y * 0.5f;
            }
            numeroSaltos--;
            Debug.Log(numeroSaltos);
        }

        if (move.x > 0.01f)
        {
            if (spriteRenderer.flipX == true)
            {
                spriteRenderer.flipX = false;
            }
        }
        else if (move.x < -0.01f)
        {
            if (spriteRenderer.flipX == false)
            {
                spriteRenderer.flipX = true;
            }
        }

        if (estaSuelo)
        {
            numeroSaltos = 2;
        }

        animator.SetBool("grounded", estaSuelo);
        animator.SetFloat("velocityX", Mathf.Abs(velocidad.x) / velocidadMaxima);

        velocidadObjetivo = move * velocidadMaxima;
    }

    private void Morir()
    {
        SceneManager.LoadScene(Constantes.ESCENA_FIN_JUEGO);
    }
}
