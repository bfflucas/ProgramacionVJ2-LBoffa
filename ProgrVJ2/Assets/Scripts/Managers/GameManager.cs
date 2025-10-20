using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    //TODO  centralizar este singleton
    

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  //el game object se llama GameManagerSingleton y el componente se llama game manager
            //con esto conservamos el game object a lo largo de las escenas
        }
        else
        {
            Destroy(gameObject);
        }
    }


    private void OnEnable()
    {
        GameEvents.OnPause += Pausar;  //con += asignamos el metodo que va a responder (no poner parentesis)
        GameEvents.OnResume += Reanudar;
    }

    private void OnDisable()
    {
        //buena practica: desuscribirse a los eventos
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
}