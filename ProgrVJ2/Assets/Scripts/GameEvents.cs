using System;
using UnityEngine;

public static class GameEvents
{

    public static event Action OnPause;
    public static event Action OnResume;
    public static event Action OnGameOver;
    public static event Action OnVictory;

    public static void TriggerPause() => OnPause?.Invoke();
    public static void TriggerResume() => OnResume?.Invoke();
    public static void TriggerGameOver() => OnGameOver?.Invoke();
    public static void TriggerVictory() => OnVictory?.Invoke();
}


//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

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