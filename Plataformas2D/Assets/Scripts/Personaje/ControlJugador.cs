using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlJugador : ObjetoFisico
{

    public float maxSpeed = 7;
    public float jumpTakeOffSpeed = 7;

    private float maxJumps = 2;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    // Use this for initialization
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    protected override void ComputeVelocity()
    {
        Vector2 move = Vector2.zero;

        move.x = Input.GetAxis("Horizontal");


        if (Input.GetButtonDown("Jump") && maxJumps > 0)
        {
            velocidad.y = jumpTakeOffSpeed;
        }
        else if (Input.GetButtonUp("Jump"))
        {
            if (velocidad.y > 0)
            {
                velocidad.y = velocidad.y * 0.5f;
            }
            maxJumps--;
            Debug.Log(maxJumps);
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
            maxJumps = 2;
        }

        animator.SetBool("grounded", estaSuelo);
        animator.SetFloat("velocityX", Mathf.Abs(velocidad.x) / maxSpeed);

        velocidadObjetivo = move * maxSpeed;
    }
}
