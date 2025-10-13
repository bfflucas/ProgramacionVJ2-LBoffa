using UnityEngine;

public class GeneradorObjeto : MonoBehaviour
{
    [SerializeField] private GameObject objetoPrefab;

    [SerializeField, Range(5f, 15f)]
    private float tiempoEspera = 1f;

    [Header("Rango X aleatorio")]
    [SerializeField] private float minX = -5f;
    [SerializeField] private float maxX = 5f;

    void Start()
    {
        InvokeRepeating(nameof(GenerarObjeto), tiempoEspera, tiempoEspera);
    }

    void GenerarObjeto()
    {
        // Elegimos una posición X aleatoria dentro del rango
        float xAleatorio = Random.Range(minX, maxX);
        float yFijo = transform.position.y; // misma Y del generador
        Vector3 posicion = new Vector3(xAleatorio, yFijo, transform.position.z);

        Instantiate(objetoPrefab, posicion, Quaternion.identity);
    }

    // ----------------------------
    // Mostrar Gizmo en la Scene
    // ----------------------------
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 0f, 0f, 0.5f); 

        // calcular el centro y tamaño del rectángulo
        float ancho = maxX - minX;
        Vector3 centro = new Vector3((minX + maxX) / 2f, transform.position.y, transform.position.z);
        Vector3 tamaño = new Vector3(ancho, 0.2f, 0.2f);

        Gizmos.DrawCube(centro, tamaño); // dibuja el rectángulo
    }
}
