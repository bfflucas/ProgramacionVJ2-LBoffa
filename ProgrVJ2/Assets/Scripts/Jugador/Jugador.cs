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
    //pruebas
    private HUDController hud;
    public void SetHUD(HUDController h)
    {
        hud = h;
        OnLivesChanged.RemoveAllListeners();
        OnLivesChanged.AddListener(h.ActualizarVidasHUD);
    }

    //----------------
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemigo"))
        {
            // Revisamos si el jugador golpea desde arriba
            // asumimos que el punto de contacto principal es collision.contacts[0]
            ContactPoint2D contacto = collision.contacts[0];

            // Si la normal apunta hacia abajo, significa que el jugador cayó sobre el enemigo
            if (contacto.normal.y > 0.5f)
            {
                // Reproducir sonido
                if (perfilJugador.UhSFX != null)
                {
                    GameObject tempAudio = new GameObject("TempAudio");
                    tempAudio.transform.position = transform.position;
                    AudioSource a = tempAudio.AddComponent<AudioSource>();
                    a.clip = perfilJugador.UhSFX;
                    a.Play();
                    Destroy(tempAudio, perfilJugador.UhSFX.length);
                }

                // Desactivar enemigo
                collision.gameObject.SetActive(false);

                // Agregar experiencia
                if (LevelManager.Instance != null)
                    LevelManager.Instance.AgregarExperiencia(10);

                
            }
            
        }
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

    public void InicializarVida()
    {
        vida = perfilJugador.Vida;
        OnLivesChanged.Invoke(vida);
    }

}


