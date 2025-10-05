using UnityEngine;
using System;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Jugador : MonoBehaviour
{
    [SerializeField] private PerfilJugador perfilJugador;

    public PerfilJugador PerfilJugador { get => perfilJugador; }
    

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
            vida = 0;

            OnLivesChanged.Invoke(vida);


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
        }

        if (collision.gameObject.CompareTag("Health"))
        {
            if (LevelManager.Instance != null)
            {
                collision.gameObject.SetActive(false);
                vida++;
                OnLivesChanged.Invoke(vida);
            }

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


