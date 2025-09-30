using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [Header("Referencias")]
    [SerializeField] private HUDController _hud;
    [SerializeField] private Jugador jugador;

    // Estado del juego
    //private bool won;
    //public bool Won
    //{
    //    get => won;
    //    set => won = value;
    //}
    //private bool lose;
    //public bool Lose
    //{
    //    get => lose;
    //    set => lose = value;
    //}
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
        DontDestroyOnLoad(gameObject); // mantiene entre escenas

        
    }

    private void Start()
    {
        Won = false;
        Lose = false;

        if (jugador != null)
        {
            _hud.ActualizarVidasHUD(jugador.GetVidas());
            _hud.ActualizarEstrellasHUD(jugador.GetEstrellas());
            _hud.ActualizarExperienciaHUD(experiencia);
        }
    }


    //private void Update()
    //{
    //    //if(won||lose)
    //    if ((won || lose) && SceneManager.GetActiveScene().name != "EscenaFin")
    //    {
    //        SceneManager.LoadScene("EscenaFin");
    //    }
    //}

    //private void ReiniciarJuego()
    //{
        
    //    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    //}
    

    // ----------------------------
    // EXPERIENCIA
    // ----------------------------
    public void AgregarExperiencia(int cantidad)
    {
        experiencia += cantidad;
        Debug.Log("Experiencia total: " + experiencia);

        // Actualizar HUD si existe
        if (_hud != null)
        {
            _hud.ActualizarExperienciaHUD(experiencia);
            _hud.MostrarXPBonus(cantidad, experiencia);
        }
    }

    public int GetExperiencia()
    {
        return experiencia;
    }

    public void ReiniciarExperiencia()
    {
        experiencia = 0;
        if (_hud != null)
            _hud.ActualizarExperienciaHUD(experiencia);
    }

    // ----------------------------
    // METODOS DE FIN DE JUEGO
    // ----------------------------
    public void GameOver()
    {
        Lose = true;
        Won = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene("EscenaFin");
    }

    public void GameWin()
    {
        Won = true;
        Lose = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene("EscenaFin");
    }

    public void ResetFlags()
    {
        Won = false;
        Lose = false;
    }



}
