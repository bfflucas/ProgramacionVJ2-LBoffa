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

    private void HerirJugador(GameObject jugadorObj)
    {
        puedeHerir = false;
        tiempoCooldown = cooldown;

        Jugador jugador = jugadorObj.GetComponent<Jugador>();
        jugador.ModificarVida(-restarVida);
        Debug.Log(" RESTAMOS " + restarVida + " VIDA AL JUGADOR. Cooldown: " + cooldown + "s");
    }
}
