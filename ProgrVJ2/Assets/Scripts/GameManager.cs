using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public Vector3[] cameraPositions;
    public Vector3[] doorPositions;
    public Vector3 initialPlayerPosition;

    private int playerLifes = 5;
    public int PlayerLifes
    {
        get => playerLifes;
        private set => playerLifes = Mathf.Max(0, value);
    }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        GameEvents.OnPause += Pausar;
        GameEvents.OnResume += Reanudar;
    }

    private void OnDisable()
    {
        GameEvents.OnPause -= Pausar;
        GameEvents.OnResume -= Reanudar;
    }

    private void Pausar()
    {
        Time.timeScale = 0;
        Debug.Log("PAUSADO");
    }

    private void Reanudar()
    {
        Time.timeScale = 1;
        Debug.Log("REANUDADO");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale != 0)
            {
                GameEvents.TriggerPause();
            }
            else
            {
                GameEvents.TriggerResume();
            }
        }
    }

    public void AddLifes(int amount)
    {
        PlayerLifes += amount;

        Debug.Log($"Vidas actualizadas: {PlayerLifes}");
    }

    public int GetLifes()
    {
        return PlayerLifes;
    }

    public void ResetLifes()
    {
        PlayerLifes = 5;
        Debug.Log("vidas reiniciadas");
    }
}