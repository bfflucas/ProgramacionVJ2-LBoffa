using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [Header("Referencias")]
    [SerializeField] private HUDController _hud;
    [SerializeField] private Jugador jugador;

    [Header("Configuración de niveles")]
    [SerializeField] private string[] escenasPorNivel; // Lista de escenas en orden de juego
    private int nivelActual = 0;

    [Header("Estado del juego")]
    public bool Won { get; private set; }
    public bool Lose { get; private set; }

    private int experiencia = 0;

    private void Awake()
    {
        // Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Mantener entre escenas
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }



    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Actualizar referencias a HUD y jugador
        _hud = Object.FindFirstObjectByType<HUDController>();
        jugador = Object.FindFirstObjectByType<Jugador>();
        //jugador.InicializarVida();


        // Inicializamos vida después de tener HUD
        if (jugador != null)
        {
            jugador.SetHUD(_hud); // le pasamos el HUD para que sepa a quién notificar
            jugador.InicializarVida();
        }


        // Actualizar nivelActual según la escena
        for (int i = 0; i < escenasPorNivel.Length; i++)
        {
            if (escenasPorNivel[i] == scene.name)
            {
                nivelActual = i;
                break;
            }
        }

        if (jugador != null && _hud != null)
        {
            _hud.ActualizarVidasHUD(jugador.GetVidas());
            _hud.ActualizarEstrellasHUD(jugador.GetEstrellas());
            _hud.ActualizarExperienciaHUD(experiencia);
        }
    }

    private void Start()
    {

        Debug.Log("Nivel actual al iniciar escena: " + nivelActual);

        Won = false;
        Lose = false;

    }

    // ----------------------------
    // EXPERIENCIA
    // ----------------------------
    public void AgregarExperiencia(int cantidad)
    {
        experiencia += cantidad * 100;
        Debug.Log("Experiencia total: " + experiencia);

        if (_hud != null)
        {
            _hud.ActualizarExperienciaHUD(experiencia);
            _hud.MostrarXPBonus(cantidad, experiencia);
        }
    }

    public int GetExperiencia() => experiencia;

    public void ReiniciarExperiencia()
    {
        experiencia = 0;
        if (_hud != null)
            _hud.ActualizarExperienciaHUD(experiencia);
    }

    // ----------------------------
    // CONTROL DE NIVELES
    // ----------------------------
    private void AvanzarNivel()
    {
        nivelActual++;

        if (nivelActual < escenasPorNivel.Length)
        {
            SceneManager.LoadScene(escenasPorNivel[nivelActual]);
        }
        else
        {
            // Si no hay más niveles, ir a la escena final
            SceneManager.LoadScene("EscenaFin");
        }
    }

    public void GameWin()
    {
        Won = true;
        Lose = false;
        Time.timeScale = 1f;

        AvanzarNivel();
    }

    // ----------------------------
    // FIN DEL JUEGO
    // ----------------------------
    public void GameOver()
    {
        Lose = true;
        Won = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene("EscenaFin");
    }

    public void ResetFlags()
    {
        Won = false;
        Lose = false;
    }

}
