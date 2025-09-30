using UnityEngine;
using System;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Jugador : MonoBehaviour
{
    [SerializeField] private PerfilJugador perfilJugador;

    public PerfilJugador PerfilJugador { get => perfilJugador; }
    //private bool win;

    public int vida
    {
        get => perfilJugador.Vida;
        set => perfilJugador.Vida = value;
    }

    public int estrellas
    {
        get => perfilJugador.Estrellas;
        set => perfilJugador.Estrellas = value;
    }
    //public bool Win { get => win; set => win = value; }


    // Evento especial para cuando se queda sin vida
    //[SerializeField] public event Action OnGameOver;
    //[SerializeField] public event Action OnGameWon;


    // Evento para cuando gana la partida
    //public event Action OnGameWon;

    [SerializeField] private UnityEvent<int> OnLivesChanged;
    

    private void OnEnable()
    {
        OnLivesChanged.Invoke(vida);
        
    }
    private void Start()
    {
        //Win = false;
    }

    public void ModificarVida(int puntos)
    {
        Debug.Log($"Vida antes: {vida}, Modificando: {puntos}");

        vida += puntos;

        if (vida <= 0)
        {
            vida = 0;

            OnLivesChanged.Invoke(vida);
            //OnGameOver.Invoke();  // Disparar Game Over


            // Avisar al LevelManager
            if (LevelManager.Instance != null)
            {
                LevelManager.Instance.GameOver();  // perdió
            }

            //SceneManager.LoadScene("EscenaFin");
            return;  // Salir del método después de Game Over
        }
        OnLivesChanged.Invoke(vida);
    }




    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Meta"))
        {
            if (LevelManager.Instance != null)
            {
                LevelManager.Instance.GameWin();  // gana
            }

            //Win = true;
            //SceneManager.LoadScene("EscenaFin");
        }
    }

    public int GetVidas()
    {
        return vida;
    }

    public int GetEstrellas()
    {
        return estrellas;
    }
}


