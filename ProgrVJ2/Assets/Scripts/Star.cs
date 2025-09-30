using UnityEngine;

public class Star : MonoBehaviour
{
    [SerializeField] private float tiempoVida = 3f;

    void Start()
    {
        Destroy(gameObject, tiempoVida); // destruir despues de un tiempo
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject); // mata al enemigo
            Destroy(gameObject); // destruir la estrella al impactar
        }
    }
}
