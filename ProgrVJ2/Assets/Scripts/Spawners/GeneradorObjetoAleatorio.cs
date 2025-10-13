//using UnityEngine;

//public class GeneradorObjetoAleatorio : MonoBehaviour
//{
//    [SerializeField] private GameObject[] objetosPrefab;

//    [SerializeField]
//    [Range(0.5f, 5f)] private float tiempoEspera;

//    [SerializeField]
//    [Range(0.5f, 5f)] private float tiempoIntervalo;

//    void Start()
//    {
//        //InvokeRepeating(nameof(GenerarObjetoAleatorio), tiempoEspera, tiempoIntervalo);
//    }

//    void GenerarObjetoAleatorio()
//    {
//        int indexAleatorio=Random.Range(0, objetosPrefab.Length);
//        GameObject prefabAleatorio=objetosPrefab[indexAleatorio];
//        Instantiate(prefabAleatorio, transform.position, Quaternion.identity); //el prefab, posicion, rotacion
//    }

//    private void OnBecameInvisible()
//    {
//        CancelInvoke(nameof(GenerarObjetoAleatorio));
//    }

//    private void OnBecameVisible()
//    {
//        InvokeRepeating(nameof(GenerarObjetoAleatorio), tiempoEspera, tiempoIntervalo);
//    }
//}

using UnityEngine;

public class GeneradorObjetoAleatorio : MonoBehaviour
{
    [SerializeField] private MultiObjectPooler pool; // referencia al pool compartido
    [SerializeField] private GameObject[] objetosPrefab; // prefabs posibles a generar

    [SerializeField, Range(0.5f, 5f)] private float tiempoEspera = 1f;
    [SerializeField, Range(0.5f, 5f)] private float tiempoIntervalo = 2f;

    private void OnBecameVisible()
    {
        InvokeRepeating(nameof(GenerarObjetoAleatorio), tiempoEspera, tiempoIntervalo);
    }

    private void OnBecameInvisible()
    {
        CancelInvoke(nameof(GenerarObjetoAleatorio));
    }

    private void GenerarObjetoAleatorio()
    {
        if (pool == null || objetosPrefab.Length == 0)
            return;

        int indexAleatorio = Random.Range(0, objetosPrefab.Length);
        GameObject prefabAleatorio = objetosPrefab[indexAleatorio];

        GameObject obj = pool.GetPooledObject(prefabAleatorio);
        if (obj != null)
        {
            obj.transform.position = transform.position;
            obj.transform.rotation = Quaternion.identity;
            obj.SetActive(true);
        }
    }
}
