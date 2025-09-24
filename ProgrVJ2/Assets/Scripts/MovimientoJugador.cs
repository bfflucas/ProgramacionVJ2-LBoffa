using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovimientoJugador : MonoBehaviour
{
    [Header("Configuracion Movimiento")]
    [SerializeField] private float velocidad = 5f;
    [SerializeField] private float velocidadAire = 2f;
    [SerializeField] private float fuerzaSalto = 5f;

    [Header("Audio")]
    [SerializeField] private AudioClip sonidoSalto;
    private AudioSource audioSource;

    private Rigidbody2D miRigidbody2D;
    private float moverHorizontal;
    private bool puedoSaltar = true;
    private bool saltando = false;

    // Plataforma bajo el jugador
    private Rigidbody2D plataformaRb;
    private Vector2 deltaPlataforma; // delta de movimiento de la plataforma

    private void OnEnable()
    {
        miRigidbody2D = GetComponent<Rigidbody2D>();

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    private void Update()
    {
        moverHorizontal = Input.GetAxis("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space) && puedoSaltar)
        {
            puedoSaltar = false;
            saltando = true;

            if (sonidoSalto != null)
                audioSource.PlayOneShot(sonidoSalto);
        }
    }

    private void FixedUpdate()
    {
        Vector2 nuevaVel = miRigidbody2D.linearVelocity;

        // Movimiento horizontal normal (suelo o aire)
        if (puedoSaltar)
            nuevaVel.x = moverHorizontal * velocidad;
        else
            nuevaVel.x = moverHorizontal * velocidadAire;

        miRigidbody2D.linearVelocity = nuevaVel;

        // Aplicar salto
        if (saltando)
        {
            Vector2 vel = miRigidbody2D.linearVelocity;
            vel.y = 0f;
            miRigidbody2D.linearVelocity = vel;

            miRigidbody2D.AddForce(Vector2.up * fuerzaSalto, ForceMode2D.Impulse);
            saltando = false;

            // Desvincular de la plataforma al saltar
            plataformaRb = null;
        }

        // Mover al jugador junto con la plataforma
        if (plataformaRb != null)
        {
            Vector2 delta = (Vector2)plataformaRb.position - deltaPlataforma;
            miRigidbody2D.position += delta; // sumamos delta de la plataforma
            deltaPlataforma = plataformaRb.position;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Suelo"))
            puedoSaltar = true;

        if (collision.gameObject.CompareTag("Plataforma"))
        {
            puedoSaltar = true;
            plataformaRb = collision.gameObject.GetComponent<Rigidbody2D>();
            deltaPlataforma = plataformaRb.position; // guardamos posición inicial
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Plataforma"))
        {
            plataformaRb = null;
        }
    }
}
