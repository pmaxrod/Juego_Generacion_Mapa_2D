using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjetoFisico : MonoBehaviour
{

    public float normalSueloMinY = .65f;
    public float modificadorGravedad = 1f;

    protected Vector2 velocidadObjetivo;
    protected bool estaSuelo;
    protected Vector2 normalSuelo;
    protected Rigidbody2D rigidbody2D;
    protected Vector2 velocidad;
    protected ContactFilter2D filtroContacto;
    protected RaycastHit2D[] bufferGolpe = new RaycastHit2D[16];
    protected List<RaycastHit2D> listaBufferGolpe = new List<RaycastHit2D>(16);


    protected const float distanciaMinimaMovimiento = 0.001f;
    protected const float radioCapsula = 0.01f;

    void OnEnable()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        filtroContacto.useTriggers = false;
        filtroContacto.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        filtroContacto.useLayerMask = true;
    }

    void Update()
    {
        velocidadObjetivo = Vector2.zero;
        ComputeVelocity();
    }

    protected virtual void ComputeVelocity()
    {

    }

    void FixedUpdate()
    {
        velocidad += modificadorGravedad * Time.deltaTime * Physics2D.gravity;
        velocidad.x = velocidadObjetivo.x;

        estaSuelo = false;

        Vector2 deltaPosition = velocidad * Time.deltaTime;

        Vector2 moveAlongGround = new Vector2(normalSuelo.y, -normalSuelo.x);

        Vector2 move = moveAlongGround * deltaPosition.x;

        Movement(move, false);

        move = Vector2.up * deltaPosition.y;

        Movement(move, true);
    }

    void Movement(Vector2 move, bool yMovement)
    {
        float distance = move.magnitude;

        if (distance > distanciaMinimaMovimiento)
        {
            int count = rigidbody2D.Cast(move, filtroContacto, bufferGolpe, distance + radioCapsula);
            listaBufferGolpe.Clear();

            for (int i = 0; i < count; i++)
            {
                listaBufferGolpe.Add(bufferGolpe[i]);
            }

            for (int i = 0; i < listaBufferGolpe.Count; i++)
            {
                Vector2 currentNormal = listaBufferGolpe[i].normal;
                if (currentNormal.y > normalSueloMinY)
                {
                    estaSuelo = true;
                    if (yMovement)
                    {
                        normalSuelo = currentNormal;
                        currentNormal.x = 0;
                    }
                }

                float projection = Vector2.Dot(velocidad, currentNormal);
                if (projection < 0)
                {
                    velocidad += -projection * currentNormal;
                }

                float modifiedDistance = listaBufferGolpe[i].distance - radioCapsula;
                distance = modifiedDistance < distance ? modifiedDistance : distance;
            }


        }

        rigidbody2D.position += move.normalized * distance;
    }

}
