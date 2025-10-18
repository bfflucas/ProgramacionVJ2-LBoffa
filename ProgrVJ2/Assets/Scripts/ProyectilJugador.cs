using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class ProyectilJugador : MonoBehaviour
{
    private Rigidbody2D rb;
    private float vidaMaxima = 5f; // tiempo máximo antes de desactivarse
    private float tiempoVivo;
    [SerializeField] private float velocidadRotacion = 360f; // grados por segundo
    //private AudioSource audioSource;
    [SerializeField] private AudioClip uhSFX;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        tiempoVivo = 0f;
        //audioSource = GetComponent<AudioSource>();
        //if (audioSource == null)
        //    audioSource = gameObject.AddComponent<AudioSource>();

    }

    private void Update()
    {
        tiempoVivo += Time.deltaTime;
        if (tiempoVivo >= vidaMaxima)
        {
            Desactivar();
        }
        // Hacer que el proyectil gire constantemente
        transform.Rotate(Vector3.forward * velocidadRotacion * Time.deltaTime);
    }

    public void Disparar(Vector2 direccion, float fuerza)
    {
        rb.linearVelocity = Vector2.zero; // resetear velocidad antes de aplicar fuerza
        rb.AddForce(direccion * fuerza, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Si toca un enemigo, lo destruimos
        if (collision.gameObject.CompareTag("Enemigo"))
        {
            if (uhSFX != null)
            {
                // Creamos un AudioSource temporal para que se reproduzca completo
                GameObject tempAudio = new GameObject("TempAudio");
                tempAudio.transform.position = transform.position;
                AudioSource a = tempAudio.AddComponent<AudioSource>();
                a.clip = uhSFX;
                a.Play();
                Destroy(tempAudio, uhSFX.length); // se destruye después de sonar
            }

            collision.gameObject.SetActive(false);

            // AGREGAR EXPERIENCIA AL HUD
            LevelManager.Instance.AgregarExperiencia(10);
        }

        Desactivar();
    }


    private void Desactivar()
    {
        rb.linearVelocity = Vector2.zero;
        gameObject.SetActive(false);
    }
}
