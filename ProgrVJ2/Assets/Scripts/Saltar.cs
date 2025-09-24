using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saltar : MonoBehaviour
{
    [Header("Configuracion")]
    [SerializeField] private float fuerzaSalto = 5f;
    [SerializeField] private float velocidadAire = 2f; // velocidad horizontal limitada en el aire

    [Header("Audio")]
    [SerializeField] private AudioClip sonidoSalto;
    private AudioSource audioSource;

    private bool puedoSaltar = true;
    private bool saltando = false;

    private Rigidbody2D miRigidbody2D;

    private void OnEnable()
    {
        miRigidbody2D = GetComponent<Rigidbody2D>();

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && puedoSaltar)
        {
            puedoSaltar = false;
            saltando = true;

            if (sonidoSalto != null)
            {
                audioSource.PlayOneShot(sonidoSalto);
            }
        }
    }

    private void FixedUpdate()
    {
        // Aplicar salto
        if (saltando)
        {
            // Reiniciamos velocidad vertical para que todos los saltos sean iguales
            Vector2 vel = miRigidbody2D.linearVelocity;
            vel.y = 0f;
            miRigidbody2D.linearVelocity = vel;

            miRigidbody2D.AddForce(Vector2.up * fuerzaSalto, ForceMode2D.Impulse);
            saltando = false;
        }

        // Movimiento horizontal limitado en el aire
        float inputX = Input.GetAxisRaw("Horizontal"); // -1, 0, 1
        if (!puedoSaltar) // Solo mientras está en el aire
        {
            Vector2 vel = miRigidbody2D.linearVelocity;
            vel.x = inputX * velocidadAire;
            miRigidbody2D.linearVelocity = vel;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        puedoSaltar = true;
    }
}
