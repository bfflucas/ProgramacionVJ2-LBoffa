using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class JefeFinal : MonoBehaviour
{

    [SerializeField] private PerfilBoss perfilBoss;
    [SerializeField] Transform puntoSpawnProyectil;
    [SerializeField] ObjectPooler poolProyectiles;


    private float tiempoActualEspera;
    private int estadoActual;
    private Vector3 posicionSpawnOriginal;

    private const int DispararProyectil = 0;
    private const int Embestir = 1;
    private const int Mover = 2;

    private int energia;
    bool vivo = true;

    private AudioSource audioFly;

    [Header("Audio")]
    [SerializeField][Range(0, 1)] private float volumeSFX = 0.7f;



    void Start()
    {
        if (puntoSpawnProyectil != null)
        {
            posicionSpawnOriginal = puntoSpawnProyectil.localPosition;
        }

        estadoActual = DispararProyectil;
        StartCoroutine(ComportamientoJefe());

        energia = perfilBoss.Energia;

        // Inicializar audios
        InicializarAudio();

        ////audioSource para theme
        //audioTheme = gameObject.AddComponent<AudioSource>();
        //audioTheme.clip = perfilBoss.BossTheme;
        //audioTheme.loop = true;
        //audioTheme.playOnAwake = false;
        //audioTheme.spatialBlend = 0f; // 2D
        //audioTheme.volume = 1f;
        //audioTheme.volume = volumeTheme / 100f;
        //audioTheme.Play();

        //// AudioSource para Fly
        //audioFly = gameObject.AddComponent<AudioSource>();
        //audioFly.clip = perfilBoss.Fly;
        //audioFly.loop = true;
        //audioFly.playOnAwake = false;
        //audioFly.spatialBlend = 0f; // 2D
        //audioFly.volume = 1f;

        //audioFly.volume = volumeSFX / 100f;
        
        //audioFly.Play();
        

    }



    private IEnumerator ComportamientoJefe()
    {
        while (true)
        {
            switch (estadoActual)
            {
                case DispararProyectil:
                    yield return StartCoroutine(Disparar());
                    tiempoActualEspera = perfilBoss.TiempoEntreDisparos;
                    break;
                case Embestir:
                    yield return StartCoroutine(Embestida());
                    tiempoActualEspera = perfilBoss.TiempoEntreEmbestidas;
                    break;
                case Mover:
                    yield return StartCoroutine(Movimiento());
                    tiempoActualEspera = perfilBoss.TiempoEntreMovimientos;
                    break;
            }

            yield return new WaitForSeconds(tiempoActualEspera);
            ActualizarEstado();
        }
    }

    private IEnumerator Disparar()
    {
        for (int i = 0; i < 3; i++)
        {
            yield return new WaitForSeconds(0.5f);

            GameObject proyectil = poolProyectiles.GetPooledObject();
            if (proyectil != null)
            {
                proyectil.transform.position = puntoSpawnProyectil.position;
                proyectil.transform.rotation = Quaternion.identity;
                proyectil.SetActive(true);

                ProyectilEnemigo pe = proyectil.GetComponent<ProyectilEnemigo>();
                if (pe != null)
                {
                    pe.Disparar(Vector2.left);
                }
            }
        }
    }

    private IEnumerator Embestida()
    {

        Vector2 posicionInicial = transform.position;
        Vector2 posicionObjetivo = new Vector2(posicionInicial.x - perfilBoss.DistanciaEmbestida , posicionInicial.y);

        float tiempoInicio = Time.time;

        // Avance
        while (Time.time < tiempoInicio + perfilBoss.TiempoEmbestida / 2)
        {
            transform.position = Vector2.Lerp(posicionInicial, posicionObjetivo, (Time.time - tiempoInicio) / (perfilBoss.TiempoEmbestida / 2));
            yield return null;
        }

        // Retroceso
        tiempoInicio = Time.time;
        while (Time.time < tiempoInicio + perfilBoss.TiempoEmbestida / 2)
        {
            transform.position = Vector2.Lerp(posicionObjetivo, posicionInicial, (Time.time - tiempoInicio) / (perfilBoss.TiempoEmbestida / 2));
            yield return null;
        }
    }

    private IEnumerator Movimiento()
    {
        Vector2 destino = new Vector2(
            Random.Range(perfilBoss.LimiteInferior.x, perfilBoss.LimiteSuperior.x),
            Random.Range(perfilBoss.LimiteInferior.y, perfilBoss.LimiteSuperior.y)
        );


        while (Vector2.Distance(transform.position, destino) > 0.1f)
        {
            transform.position = Vector2.MoveTowards(transform.position, destino, perfilBoss.VelocidadMovimiento * Time.deltaTime);
            yield return null;
        }
    }

    private void ActualizarEstado()
    {
        estadoActual = Random.Range(0, 3);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 centro = new Vector3(
            (perfilBoss.LimiteInferior.x + perfilBoss.LimiteSuperior.x) / 2,
            (perfilBoss.LimiteInferior.y + perfilBoss.LimiteSuperior.y) / 2,
            0
        );
        Vector3 tamano = new Vector3(
            Mathf.Abs(perfilBoss.LimiteSuperior.x - perfilBoss.LimiteInferior.x),
            Mathf.Abs(perfilBoss.LimiteSuperior.y - perfilBoss.LimiteInferior.y),
            0
        );
        Gizmos.DrawWireCube(centro, tamano);

    }

    public void ModificarEnergia(int puntos)
    {
        Debug.Log($"Energia jefe: {energia}, Modificando: {puntos}");

        energia -= puntos;

        if (energia <= 0)
        {
            energia = 0;
            vivo = false;

            // Avisar al LevelManager
            if (LevelManager.Instance != null)
            {
                LevelManager.Instance.GameWin();  // gana
            } 
            return;  
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Proyectil"))
        {
            if (perfilBoss.Roar != null)
            {
                // Reproducir roar con configuración de volumen consistente
                GameObject tempAudio = new GameObject("TempAudio");
                tempAudio.transform.position = transform.position;
                AudioSource a = tempAudio.AddComponent<AudioSource>();
                a.clip = perfilBoss.Roar;
                a.volume = volumeSFX; // Usar el mismo volumen SFX


                a.Play();
                Destroy(tempAudio, perfilBoss.Roar.length);

                ModificarEnergia(1);
            }

            // AGREGAR EXPERIENCIA AL HUD
            if (LevelManager.Instance != null)
            {
                LevelManager.Instance.AgregarExperiencia(10);
            }
        }

    }

    public PerfilBoss PerfilBoss { get => perfilBoss; }

    private void InicializarAudio()
    {

        // Configurar AudioSource para sonido de vuelo
        if (perfilBoss.Fly != null)
        {
            audioFly = gameObject.AddComponent<AudioSource>();
            audioFly.clip = perfilBoss.Fly;
            audioFly.loop = true;
            audioFly.playOnAwake = false;
            audioFly.spatialBlend = 0f; // 2D

            // Configurar volumen CORRECTAMENTE (una sola vez)
            audioFly.volume = volumeSFX;

            audioFly.Play();
            Debug.Log("Sonido de vuelo iniciado: " + perfilBoss.Fly.name);
        }
        else
        {
            Debug.LogWarning("No hay clip de Fly asignado en el PerfilBoss");
        }
    }

}