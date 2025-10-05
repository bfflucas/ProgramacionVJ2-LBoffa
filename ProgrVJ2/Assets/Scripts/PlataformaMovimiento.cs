using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlataformaMovimiento : MonoBehaviour
{
    public enum DireccionMovimiento { Horizontal, Vertical, Diagonal }

    [Header("Configuracion")]
    public DireccionMovimiento direccion = DireccionMovimiento.Horizontal;
    public float velocidad = 2f;
    public Vector2 distancia = new Vector2(3f, 3f);

    private Rigidbody2D rb;
    private Vector2 posicionInicial;

    private Rigidbody2D jugadorRb; // jugador encima

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        posicionInicial = rb.position;
    }

    void FixedUpdate()
    {
        Vector2 nuevaPos = rb.position;

        // Calcular nueva posición de la plataforma
        switch (direccion)
        {
            case DireccionMovimiento.Horizontal:
                nuevaPos.x = posicionInicial.x + Mathf.PingPong(Time.time * velocidad, distancia.x * 2) - distancia.x;
                break;
            case DireccionMovimiento.Vertical:
                nuevaPos.y = posicionInicial.y + Mathf.PingPong(Time.time * velocidad, distancia.y * 2) - distancia.y;
                break;
            case DireccionMovimiento.Diagonal:
                nuevaPos.x = posicionInicial.x + Mathf.PingPong(Time.time * velocidad, distancia.x * 2) - distancia.x;
                nuevaPos.y = posicionInicial.y + Mathf.PingPong(Time.time * velocidad, distancia.y * 2) - distancia.y;
                break;
        }

        Vector2 deltaMovimiento = nuevaPos - rb.position;

        // Mover plataforma
        rb.MovePosition(nuevaPos);

        // Si el jugador está encima, moverlo con la plataforma
        if (jugadorRb != null)
        {
            jugadorRb.MovePosition(jugadorRb.position + deltaMovimiento);
        }
    }  
}
