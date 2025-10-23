using System.Collections;
using UnityEngine;

public class Herir : MonoBehaviour
{
    [Header("Configuracion")]
    [SerializeField] int restarVida = 1;
    [SerializeField] float cooldown = 1f;

    private bool puedeHerir = true;
    private float tiempoCooldown;

    private void Update()
    {
        if (!puedeHerir)
        {
            tiempoCooldown -= Time.deltaTime;
            if (tiempoCooldown <= 0)
            {
                puedeHerir = true;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && puedeHerir)
        {
            HerirJugador(collision.gameObject);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        // Opcional: también verificar durante la colisión continua
        if (collision.gameObject.CompareTag("Player") && puedeHerir)
        {
            HerirJugador(collision.gameObject);
        }
    }

    public void HerirJugador(GameObject jugadorObj)
    {
        // Obtenemos la posición del jugador y del enemigo
        Vector3 jugadorPos = jugadorObj.transform.position;
        Vector3 enemigoPos = transform.position;

        // Accedemos al componente Jugador
        Jugador jugador = jugadorObj.GetComponent<Jugador>();
        if (jugador == null) return;

        if (!gameObject.CompareTag("Suelo"))
        {
            // Si el jugador está por encima del enemigo (lo pisa), no hacer daño
            if (jugadorPos.y > enemigoPos.y + 0.2f) // margen de 0.2f ajustable
            {
                // Reproducir sonido si tiene
                if (jugador.PerfilJugador != null && jugador.PerfilJugador.UhSFX != null)
                {
                    GameObject tempAudio = new GameObject("TempAudio");
                    tempAudio.transform.position = transform.position;
                    AudioSource a = tempAudio.AddComponent<AudioSource>();
                    a.clip = jugador.PerfilJugador.UhSFX;
                    a.Play();
                    Destroy(tempAudio, jugador.PerfilJugador.UhSFX.length);
                }

                // Si no es jefe, desactivar
                if (!gameObject.CompareTag("Boss"))
                {
                    gameObject.SetActive(false);
                }

                // Agregar experiencia
                if (LevelManager.Instance != null)
                    LevelManager.Instance.AgregarExperiencia(10);

                // Rebote del jugador
                Rigidbody2D rbJugador = jugadorObj.GetComponent<Rigidbody2D>();
                if (rbJugador != null)
                {
                    rbJugador.linearVelocity = new Vector2(rbJugador.linearVelocity.x, 10f);
                }

                return; // salimos sin restar vida
            }
        }
        

        // Si no lo pisa, le hacemos daño
        puedeHerir = false;
        tiempoCooldown = cooldown;

        jugador.ModificarVida(-restarVida);
        Debug.Log("RESTAMOS " + restarVida + " VIDA AL JUGADOR. Cooldown: " + cooldown + "s");
    }
}