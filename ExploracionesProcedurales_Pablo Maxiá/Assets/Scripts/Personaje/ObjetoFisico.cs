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
    protected Rigidbody2D rb2D;
    protected Vector2 velocidad;
    protected ContactFilter2D filtroContacto;
    protected RaycastHit2D[] bufferGolpe = new RaycastHit2D[16];
    protected List<RaycastHit2D> listaBufferGolpe = new List<RaycastHit2D>(16);


    protected const float distanciaMinimaMovimiento = 0.001f;
    protected const float radioCapsula = 0.01f;

    void OnEnable()
    {
        rb2D = GetComponent<Rigidbody2D>();
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

        Vector2 posicionDelta = velocidad * Time.deltaTime;

        Vector2 moverSobreSuelo = new Vector2(normalSuelo.y, -normalSuelo.x);

        Vector2 move = moverSobreSuelo * posicionDelta.x;

        Movimiento(move, false);

        move = Vector2.up * posicionDelta.y;

        Movimiento(move, true);
    }

    void Movimiento(Vector2 vector, bool movimientoY)
    {
        float distancia = vector.magnitude;

        if (distancia > distanciaMinimaMovimiento)
        {
            int count = rb2D.Cast(vector, filtroContacto, bufferGolpe, distancia + radioCapsula);
            listaBufferGolpe.Clear();

            for (int i = 0; i < count; i++)
            {
                listaBufferGolpe.Add(bufferGolpe[i]);
            }

            for (int i = 0; i < listaBufferGolpe.Count; i++)
            {
                Vector2 normalActual = listaBufferGolpe[i].normal;
                if (normalActual.y > normalSueloMinY)
                {
                    estaSuelo = true;
                    if (movimientoY)
                    {
                        normalSuelo = normalActual;
                        normalActual.x = 0;
                    }
                }

                float proyeccion = Vector2.Dot(velocidad, normalActual);
                if (proyeccion < 0)
                {
                    velocidad += -proyeccion * normalActual;
                }

                float distanciaModificada = listaBufferGolpe[i].distance - radioCapsula;
                distancia = distanciaModificada < distancia ? distanciaModificada : distancia;
            }


        }

        rb2D.position += vector.normalized * distancia;
    }

}
