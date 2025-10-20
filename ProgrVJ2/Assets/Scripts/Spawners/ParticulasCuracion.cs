using UnityEngine;
using System.Collections.Generic;

public class ParticulasCuracion : MonoBehaviour
{
    [SerializeField] private int cantidadCuracion = 1;
    private ParticleSystem ps;
    private AudioSource audioSource;
    private List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();

    private void Awake()
    {
        ps = GetComponent<ParticleSystem>();
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    private void OnParticleCollision(GameObject other)
    {
        Debug.Log("OnParticleCollision con: " + other.name);

        Jugador jugador = other.GetComponent<Jugador>();
        if (jugador == null)
            jugador = other.GetComponentInParent<Jugador>(); 

        if (jugador != null)
        {
            if (jugador.GetVidas() < jugador.PerfilJugador.MaxVida)
            {
                jugador.ModificarVida(cantidadCuracion);
                Debug.Log("Jugador curado por partículas!");

                if (jugador.PerfilJugador.CoinSFX != null && audioSource != null)
                    audioSource.PlayOneShot(jugador.PerfilJugador.CoinSFX);
            }
        }

            
    }

}
