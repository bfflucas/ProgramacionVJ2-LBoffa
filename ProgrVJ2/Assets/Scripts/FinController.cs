using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class FinController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textoFin;

    private void Start()
    {
        if (LevelManager.Instance != null)
        {
            if(LevelManager.Instance.Won){
                textoFin.text = "Ganaste la partida! Encontraste la salida...";
            }
        }
        if (LevelManager.Instance != null)
        {
            if (LevelManager.Instance.Lose)
            {
                textoFin.text = "Perdiste!! Te quedaste sin vidas...";
            }
        }
    }

    public void ReiniciarJuego()
    {
        Time.timeScale = 1f;
        
        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.ReiniciarExperiencia();
            // resetear flags
            LevelManager.Instance.ResetFlags();

        }
        SceneManager.LoadScene("Nivel1");
    }
}
