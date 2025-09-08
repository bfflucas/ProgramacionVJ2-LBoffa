using UnityEngine;
using System;

public class Jugador : MonoBehaviour
{
    [Header("Configuracion")]
    [SerializeField] private float vida = 100f;

    // Evento para notificar cambios de vida
    public event Action<float> OnVidaCambiada;

    // Evento especial para cuando se queda sin vida
    public event Action OnGameOver;

    // Evento para cuando gana la partida
    public event Action OnGameWon;

    public void ModificarVida(float puntos)
    {
        vida += puntos;
        if (vida < 0)
        {
            vida=0;
        }
        // Notificar cambio
        OnVidaCambiada?.Invoke(vida);

        // Si la vida es 0 o menos -> disparar Game Over
        if (!EstaVivo())
        {
            OnGameOver?.Invoke();
        }
    }

    private bool EstaVivo()
    {
        return vida > 0;
    }

    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Meta"))
        {
            Debug.Log("¡Llegaste a la meta!"); // para probar que se detecta
            OnGameWon?.Invoke();  // Dispara evento de victoria
        }
    }

    public float GetVidas()
    {
        return vida;
    }
}
