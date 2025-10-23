using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
public class MovimientoJugador : MonoBehaviour
{
    private Jugador jugador;


    // EVENTOS de estrellas
    [SerializeField] private UnityEvent<int> OnEstrellasChanged;

    private Rigidbody2D miRigidbody2D;
    private Animator miAnimator;
    private SpriteRenderer miSprite;

    private float moverHorizontal;
    private bool puedoSaltar = true;
    private bool saltando = false;

    // Plataforma bajo el jugador
    private Rigidbody2D plataformaRb;
    private Vector2 deltaPlataforma;

    // Detección de paredes
    private bool tocandoParedIzquierda = false;
    private bool tocandoParedDerecha = false;

    private AudioSource audioSource;

    [SerializeField] private GameObject prefabEstrellaDisparo; // prefab de la estrella como proyectil
    [SerializeField] private float fuerzaDisparo = 10f;

    // Direccion de disparo: 1 = derecha, -1 = izquierda
    private int direccionDisparo = 1;


    private Queue<GameObject> colaEstrellas = new Queue<GameObject>();


    private void Awake()
    {
        jugador = GetComponent<Jugador>();
    }

    private void OnEnable()
    {
        miRigidbody2D = GetComponent<Rigidbody2D>();
        miAnimator = GetComponent<Animator>();
        miSprite = GetComponent<SpriteRenderer>();

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        OnEstrellasChanged.Invoke(colaEstrellas.Count);


        for (int i = 0; i < jugador.GetEstrellas(); i++)
        {
            GameObject estrella = Instantiate(prefabEstrellaDisparo);
            estrella.SetActive(false);
            estrella.transform.SetParent(transform);
            colaEstrellas.Enqueue(estrella);
        }


        // Notificar HUD
        OnEstrellasChanged?.Invoke(colaEstrellas.Count);

    }

    private void Update()
    {
        moverHorizontal = Input.GetAxis("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space) && puedoSaltar)
        {
            saltando = true;
            puedoSaltar = false;

            if (jugador.PerfilJugador.JumpSFX != null)
                audioSource.PlayOneShot(jugador.PerfilJugador.JumpSFX);
        }

        // Direccion automatica segun movimiento
        if (moverHorizontal > 0.1f)
            direccionDisparo = 1;
        else if (moverHorizontal < -0.1f)
            direccionDisparo = -1;

        // Disparar con F
        if (Input.GetKeyDown(KeyCode.F) && colaEstrellas.Count > 0)
        {
            LanzarEstrella();
        }
    }

    private void FixedUpdate()
    {
        float velX = 0f;

        // Movimiento horizontal en suelo o aire
        if (puedoSaltar)
            velX = moverHorizontal * jugador.PerfilJugador.VelocidadSuelo;
        else
            velX = Mathf.Lerp(miRigidbody2D.linearVelocity.x, moverHorizontal * jugador.PerfilJugador.VelocidadAire, 0.1f);

        // Limitar avance si hay pared lateral
        if ((tocandoParedDerecha && velX > 0) || (tocandoParedIzquierda && velX < 0))
            velX = 0f;

        Vector2 nuevaVel = new Vector2(velX, miRigidbody2D.linearVelocity.y);
        miRigidbody2D.linearVelocity = nuevaVel;

        // --- ANIMACION SOLO EN EL SUELO ---
        if (puedoSaltar)
        {
            int velocidadX = (int)miRigidbody2D.linearVelocity.x;
            miSprite.flipX = miRigidbody2D.linearVelocity.x < 0;
            miAnimator.SetInteger("Velocidad", velocidadX);

            miAnimator.SetBool("EnAire", false);

        }
        else
        {
            miAnimator.SetInteger("Velocidad", 0);
            miAnimator.SetBool("EnAire", miRigidbody2D.linearVelocityY != 0);
        }

        // Aplicar salto
        if (saltando)
        {
            Vector2 vel = miRigidbody2D.linearVelocity;
            vel.y = 0f; // reset vertical antes del salto
            miRigidbody2D.linearVelocity = vel;

            miRigidbody2D.AddForce(Vector2.up * jugador.PerfilJugador.FuerzaSalto, ForceMode2D.Impulse);
            saltando = false;
            plataformaRb = null;
        }

        // Mover al jugador junto con la plataforma
        if (plataformaRb != null)
        {
            Vector2 delta = (Vector2)plataformaRb.position - deltaPlataforma;
            miRigidbody2D.position += delta;
            deltaPlataforma = plataformaRb.position;
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Ignorar colisión con enemigos y bosses para que no se quede pegado
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Boss"))
        {
            Collider2D playerCollider = GetComponent<Collider2D>();
            Collider2D enemyCollider = collision.collider;
            Physics2D.IgnoreCollision(playerCollider, enemyCollider, true);


        }

        foreach (ContactPoint2D contact in collision.contacts)
        {
            // Salto solo si contacto viene desde arriba
            if (contact.normal.y > 0.5f)
            {
                if (collision.gameObject.CompareTag("Suelo"))
                    puedoSaltar = true;

                if (collision.gameObject.CompareTag("Plataforma"))
                {
                    puedoSaltar = true;
                    plataformaRb = collision.gameObject.GetComponent<Rigidbody2D>();
                    deltaPlataforma = plataformaRb.position;
                }


            }

            // Deteccion de paredes
            if (contact.normal.x > 0.5f)
                tocandoParedIzquierda = true;
            if (contact.normal.x < -0.5f)
                tocandoParedDerecha = true;
        }
    }




    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Boss"))
        {
            Collider2D playerCollider = GetComponent<Collider2D>();
            Collider2D enemyCollider = collision.collider;
            Physics2D.IgnoreCollision(playerCollider, enemyCollider, false); // restauramos colisión
            return;
        }

        // Mantener la deteccion de paredes mientras este tocando
        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.x > 0.5f)
                tocandoParedIzquierda = true;
            if (contact.normal.x < -0.5f)
                tocandoParedDerecha = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Boss"))
        {
            Collider2D playerCollider = GetComponent<Collider2D>();
            Collider2D enemyCollider = collision.collider;
            Physics2D.IgnoreCollision(playerCollider, enemyCollider, false); // restauramos colisión
            return;
        }

        // Si dejo de tocar cualquier objeto con tag "Suelo" o "Plataforma" lateral
        if (collision.gameObject.CompareTag("Suelo") || collision.gameObject.CompareTag("Plataforma"))
        {
            // Reiniciamos detección de paredes
            tocandoParedIzquierda = false;
            tocandoParedDerecha = false;
        }

        // Plataforma movil
        if (collision.gameObject.CompareTag("Plataforma"))
            plataformaRb = null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Star"))
        {
            if (jugador.PerfilJugador.CoinSFX != null)
                audioSource.PlayOneShot(jugador.PerfilJugador.CoinSFX);
            colaEstrellas.Enqueue(collision.gameObject);
            collision.gameObject.SetActive(false);

            Debug.Log("Estrella recogida. Total en cola: " + colaEstrellas.Count);

            OnEstrellasChanged?.Invoke(colaEstrellas.Count);
        }

        if (collision.gameObject.CompareTag("Health"))
        {
            if (LevelManager.Instance != null)
            {
                if (jugador.PerfilJugador.CoinSFX != null)
                    audioSource.PlayOneShot(jugador.PerfilJugador.CoinSFX);

            }
        }
    }


    private void LanzarEstrella()
    {
        if (colaEstrellas.Count == 0) return;

        colaEstrellas.Dequeue();
        OnEstrellasChanged?.Invoke(colaEstrellas.Count);

        Debug.Log("Instanciando proyectil...");

        Vector3 offset = new Vector3(direccionDisparo * 0.5f, 0, 0); // 0.5 unidades delante según dirección
        GameObject proyectilGO = Instantiate(prefabEstrellaDisparo, transform.position + offset, Quaternion.identity);



        if (proyectilGO == null)
        {
            Debug.LogError("El prefab es NULL!");
            return;
        }

        ProyectilJugador proyectil = proyectilGO.GetComponent<ProyectilJugador>();
        if (proyectil == null)
        {
            Debug.LogError("El prefab NO tiene el script Proyectil!");
        }
        else
        {
            Debug.Log("Disparando proyectil en dirección " + direccionDisparo);
            proyectil.Disparar(new Vector2(direccionDisparo, 0), fuerzaDisparo);
            if (jugador.PerfilJugador.JumpSFX != null)
                audioSource.PlayOneShot(jugador.PerfilJugador.ShootSFX);
        }
    }

}


