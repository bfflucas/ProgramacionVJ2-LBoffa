using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class ProyectilEnemigo : MonoBehaviour
{
    private Rigidbody2D rb;
    private float vidaMaxima = 5f;
    private float tiempoVivo;

    [SerializeField] public float velocidad = 30f;
    [SerializeField] private AudioClip uhSFX;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        tiempoVivo = 0f;
    }

    private void Update()
    {
        tiempoVivo += Time.deltaTime;
        if (tiempoVivo >= vidaMaxima)
        {
            Desactivar();
        }
    }

    // Este método controla el movimiento
    public void Disparar(Vector2 direccion)
    {
        rb.linearVelocity = Vector2.zero; // resetear velocidad
        rb.AddForce(direccion.normalized * velocidad, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (uhSFX != null)
            {
                GameObject tempAudio = new GameObject("TempAudio");
                tempAudio.transform.position = transform.position;
                AudioSource a = tempAudio.AddComponent<AudioSource>();
                a.clip = uhSFX;
                a.Play();
                Destroy(tempAudio, uhSFX.length);
            }
        }

        Desactivar();
    }

    private void Desactivar()
    {
        rb.linearVelocity = Vector2.zero;
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        //Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.linearVelocity = Vector2.zero;
    }


}
