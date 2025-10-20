using UnityEngine;

public class EnemigoMovimientoInteligente : MonoBehaviour
{
    [Header("Movimiento Patrulla")]
    [SerializeField] private float distanciaMovimiento = 5f;
    [SerializeField] private float velocidadPatrulla = 2f;

    [Header("Persecución")]
    [SerializeField] private float distanciaDeteccion = 5f;
    [SerializeField] private float velocidadPersecucion = 3f;

    private Transform jugador;
    private Vector3 puntoIzquierdo;
    private Vector3 puntoDerecho;
    private bool moviendoDerecha = true;
    private bool estaPersiguiendo = false;

    void Start()
    {
        // Buscar jugador por tag
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            jugador = player.transform;

        // Establecer puntos de patrulla iniciales
        ActualizarPuntosPatrulla();
    }

    void Update()
    {
        // Detectar jugador
        if (jugador != null && Vector2.Distance(transform.position, jugador.position) <= distanciaDeteccion)
        {
            estaPersiguiendo = true;
        }
        else
        {
            // Si estaba persiguiendo y perdió al jugador, actualizar puntos de patrulla
            if (estaPersiguiendo)
            {
                ActualizarPuntosPatrulla();
            }
            estaPersiguiendo = false;
        }

        // Movimiento
        if (estaPersiguiendo)
        {
            // Perseguir jugador solo en X
            Vector3 targetPos = new Vector3(jugador.position.x, transform.position.y, transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, targetPos, velocidadPersecucion * Time.deltaTime);
        }
        else
        {
            // Patrullar
            Vector3 objetivo = moviendoDerecha ? puntoDerecho : puntoIzquierdo;
            transform.position = Vector3.MoveTowards(transform.position, objetivo, velocidadPatrulla * Time.deltaTime);

            // Cambiar dirección al llegar al punto
            if (Vector3.Distance(transform.position, objetivo) < 0.1f)
            {
                moviendoDerecha = !moviendoDerecha;
            }
        }
    }

    private void ActualizarPuntosPatrulla()
    {
        // Actualizar puntos de patrulla basados en la posición actual (solo X)
        Vector3 posicionActual = transform.position;
        puntoDerecho = new Vector3(posicionActual.x + distanciaMovimiento, posicionActual.y, posicionActual.z);
        puntoIzquierdo = new Vector3(posicionActual.x - distanciaMovimiento, posicionActual.y, posicionActual.z);
    }

    // Gizmos para visualización
    private void OnDrawGizmosSelected()
    {
        // Puntos de patrulla
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(puntoDerecho, 0.2f);
        Gizmos.DrawWireSphere(puntoIzquierdo, 0.2f);
        Gizmos.DrawLine(puntoIzquierdo, puntoDerecho);

        // Rango de detección
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, distanciaDeteccion);
    }
}