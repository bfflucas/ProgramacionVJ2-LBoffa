using UnityEngine;

[CreateAssetMenu(fileName = "PerfilBoss", menuName = "Scriptable Objects/PerfilBoss")]
public class PerfilBoss : ScriptableObject
{
    [Header("Atributos")]
    [SerializeField] private int energia = 10;

    [Header("Tiempos")]
    [SerializeField] private float tiempoEntreDisparos = 3f;
    [SerializeField] private float tiempoEntreEmbestidas = 4f;
    [SerializeField] private float tiempoEntreMovimientos = 3f;

    [Header("Ataque")]
    [SerializeField] private float distanciaEmbestida = 10f;
    [SerializeField] private float tiempoEmbestida = 2f;

    [Header("Área de movimiento")]
    [SerializeField] private Vector2 limiteInferior = new Vector2(-10f, -3f);
    [SerializeField] private Vector2 limiteSuperior = new Vector2(10f, 5f);
    [SerializeField] private float velocidadMovimiento = 20f;

    [Header("Audio")]
    [SerializeField] private AudioClip bossTheme;
    [SerializeField] private AudioClip roar;
    [SerializeField] private AudioClip fly;


    public int Energia { get => energia; set => energia = value; }
    public float TiempoEntreDisparos { get => tiempoEntreDisparos; set => tiempoEntreDisparos = value; }
    public float TiempoEntreEmbestidas { get => tiempoEntreEmbestidas; set => tiempoEntreEmbestidas = value; }
    public float TiempoEntreMovimientos { get => tiempoEntreMovimientos; set => tiempoEntreMovimientos = value; }
    public float DistanciaEmbestida { get => distanciaEmbestida; set => distanciaEmbestida = value; }
    public float TiempoEmbestida { get => tiempoEmbestida; set => tiempoEmbestida = value; }
    public Vector2 LimiteInferior { get => limiteInferior; set => limiteInferior = value; }
    public Vector2 LimiteSuperior { get => limiteSuperior; set => limiteSuperior = value; }
    public float VelocidadMovimiento { get => velocidadMovimiento; set => velocidadMovimiento = value; }
    public AudioClip BossTheme { get => bossTheme; set => bossTheme = value; }
    public AudioClip Roar { get => roar; set => roar = value; }
    public AudioClip Fly { get => fly; set => fly = value; }
}
