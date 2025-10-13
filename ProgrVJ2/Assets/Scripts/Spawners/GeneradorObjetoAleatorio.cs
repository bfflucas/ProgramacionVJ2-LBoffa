using UnityEngine;

public class GeneradorObjetoAleatorio : MonoBehaviour
{
    [SerializeField] private GameObject[] objetosPrefab;

    [SerializeField]
    [Range(0.5f, 5f)] private float tiempoEspera;

    [SerializeField]
    [Range(0.5f, 5f)] private float tiempoIntervalo;

    void Start()
    {
        //InvokeRepeating(nameof(GenerarObjetoAleatorio), tiempoEspera, tiempoIntervalo);
    }

    void GenerarObjetoAleatorio()
    {
        int indexAleatorio=Random.Range(0, objetosPrefab.Length);
        GameObject prefabAleatorio=objetosPrefab[indexAleatorio];
        Instantiate(prefabAleatorio, transform.position, Quaternion.identity); //el prefab, posicion, rotacion
    }

    private void OnBecameInvisible()
    {
        CancelInvoke(nameof(GenerarObjetoAleatorio));
    }

    private void OnBecameVisible()
    {
        InvokeRepeating(nameof(GenerarObjetoAleatorio), tiempoEspera, tiempoIntervalo);
    }
}
