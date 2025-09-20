using UnityEngine;
using TMPro;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private HUDController _hud;
    [SerializeField] private Jugador jugador;

    private bool gameOver;
    private bool gameWon;

    void Start()
    {
        gameOver = false;

        if (jugador != null)
        {
            jugador.OnGameOver += MostrarGameOver;
            jugador.OnGameWon += MostrarGameWon;
            
            _hud.ActualizarVidasHUD(jugador.GetVidas());
        }

    }

    void Update()
    {
        if (gameOver || gameWon)
        {
           
            //if (Input.anyKeyDown) para cualquier tecla

            // Solo reinicia con la tecla R
            if (Input.GetKeyDown(KeyCode.R))
            {
                // Reiniciar escena actual
                Time.timeScale = 1f; // restaurar tiempo
                UnityEngine.SceneManagement.SceneManager.LoadScene(
                    UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex
                );
            }
        }
    }



    private void MostrarGameOver()
    {
        gameOver = true;

        // Congelar el juego
        Time.timeScale = 0f;

        //textoGameOver.gameObject.SetActive(true);
        //textoGameOver.text = "GAME OVER - presiona R para reiniciar...";
        Debug.Log("GAME OVER");
    }

    private void MostrarGameWon()
    {
        gameOver = true;

        // Congelar el juego
        Time.timeScale = 0f;

        //textoGameOver.gameObject.SetActive(true);
        //textoGameOver.text = "Ganaste!! - presiona R para reiniciar...";
    }

}
