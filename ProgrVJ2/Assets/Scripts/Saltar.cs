
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saltar : MonoBehaviour
{
    // Variables a configurar desde el editor
    [Header("Configuracion")]
    [SerializeField] private float fuerzaSalto = 5f;

    // Audio
    [Header("Audio")]
    [SerializeField] private AudioClip sonidoSalto;  // Asignar desde el inspector
    private AudioSource audioSource;

    // Variables de uso interno en el script
    private bool puedoSaltar = true;
    private bool saltando = false;

    // Variable para referenciar otro componente del objeto
    private Rigidbody2D miRigidbody2D;

    // Codigo ejecutado cuando el objeto se activa en el nivel
    private void OnEnable()
    {
        miRigidbody2D = GetComponent<Rigidbody2D>();

        // Inicializar el AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

    }

    // Codigo ejecutado en cada frame del juego (Intervalo variable)
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && puedoSaltar)
        {
            puedoSaltar = false;
        }

        

    }

    //las fuerzas se aplican en fixedUpdate, ya que este utiliza un intervalo fijo
    private void FixedUpdate()
    {
        if(!puedoSaltar && !saltando)
        {
            miRigidbody2D.AddForce(Vector2.up * fuerzaSalto, ForceMode2D.Impulse);
            // por defecto cuando aplicamos fuerza, AddForce lo hace en modo Force, pero en el caso del impulso es distinto

            // Reproducir sonido al presionar salto
            if (sonidoSalto != null)
            {
                audioSource.PlayOneShot(sonidoSalto);
            }

            saltando = true;
        }
    }

    //codigo ejecutado cuando el jugador colisiona con otro objeto
    private void OnCollisionEnter2D(Collision2D collision)
    {
        puedoSaltar = true;
        saltando = false;
    }
}