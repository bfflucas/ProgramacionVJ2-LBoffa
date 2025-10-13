using UnityEngine;

[CreateAssetMenu(fileName = "PerfilJugador", menuName = "Scriptable Objects/PerfilJugador")]
public class PerfilJugador : ScriptableObject
{
    [Header("Configuracion")]
    [SerializeField][Range(5,10)] private int vida = 5;
    [SerializeField] private int estrellas = 3;
    public int Estrellas { get => estrellas; set => estrellas = value; }


    [Header("Configuracion Movimiento")]
    [SerializeField] private float velocidadSuelo = 5f;
    [SerializeField] private float velocidadAire = 2f;
    [SerializeField] private float fuerzaSalto = 5f;

    [Header("Audio")]
    [SerializeField] private AudioClip jumpSFX;
    [SerializeField] private AudioClip shootSFX;
    [SerializeField] private AudioClip coinSFX;
    [SerializeField] private AudioClip lifeSFX;
    [SerializeField] private AudioClip uhSFX;


    //private AudioSource audioSource;

    public int Vida { get => vida; set => vida = value; }
    public float VelocidadSuelo { get => velocidadSuelo; set => velocidadSuelo = value; }
    public float VelocidadAire { get => velocidadAire; set => velocidadAire = value; }
    public float FuerzaSalto { get => fuerzaSalto; set => fuerzaSalto = value; }
    public AudioClip JumpSFX { get => jumpSFX; }
    public AudioClip ShootSFX { get => shootSFX; }
    public AudioClip CoinSFX { get => coinSFX; }
    public AudioClip LifeSFX { get => lifeSFX; }
    public AudioClip UhSFX { get => uhSFX; set => uhSFX = value; }
}
