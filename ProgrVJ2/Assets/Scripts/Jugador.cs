using UnityEngine;
using System;
using UnityEngine.Events;

public class Jugador : MonoBehaviour
{
    [Header("Configuracion")]
    [SerializeField] private int vida = 5;


    // Evento especial para cuando se queda sin vida
    public event Action OnGameOver;

    // Evento para cuando gana la partida
    public event Action OnGameWon;

    [SerializeField] private UnityEvent<int> OnLivesChanged;

    private void OnEnable()
    {
        OnLivesChanged.Invoke(vida);
    }

    public void ModificarVida(int puntos)
    {
        Debug.Log($"Vida antes: {vida}, Modificando: {puntos}");

        vida += puntos;
        
        if (vida <= 0)
        {
            vida=0;
            OnLivesChanged.Invoke(vida);
            OnGameOver?.Invoke();  // Disparar Game Over 
            return;  // Salir del método después de Game Over
        }
        Debug.Log($"Vida después: {vida}");
        OnLivesChanged.Invoke(vida);

        
    }


    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Meta"))
        {
            Debug.Log("¡Llegaste a la meta!"); // para probar que se detecta
            OnGameWon?.Invoke();  // Dispara evento de victoria
        }
    }

    public int GetVidas()
    {
        return vida;
    }
}
