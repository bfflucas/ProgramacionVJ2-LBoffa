using UnityEngine;

public class ProyectilRecto : MonoBehaviour
{

    [SerializeField]
    [Range(1f, 30f)]
    private float speed = 10f;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        Mover();
    }

    private void Mover()
    {
        Vector2 direction = Vector2.left;
        rb.linearVelocity = direction * speed;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        // Opcional: también verificar durante la colisión continua
        if (collision.gameObject.CompareTag("Player"))
        {
            gameObject.SetActive(false);
        }
    }
}
