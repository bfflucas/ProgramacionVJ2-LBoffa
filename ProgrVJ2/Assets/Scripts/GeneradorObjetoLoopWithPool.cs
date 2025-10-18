using UnityEngine;

public class GeneradorObjetoLoopWithPool : MonoBehaviour
{

    [SerializeField]
    [Range(0.5f, 5f)] private float tiempoEspera;

    [SerializeField]
    [Range(0.5f, 5f)] private float tiempoIntervalo;

    [SerializeField]
    [Range(0.5f, 5f)] private float spawnDistance = 1f;

    private ObjectPooler objectPool;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake ()
    {
        objectPool = GetComponent<ObjectPooler>();
    }

    private void Start()
    {
        InvokeRepeating(nameof(GenerarObjetoLoop), tiempoEspera, tiempoIntervalo);
    }

    void GenerarObjetoLoop()
    {
        GameObject pooledObject = objectPool.GetPooledObject();

        if (pooledObject != null)
        {
            pooledObject.transform.position = transform.position + Vector3.left * spawnDistance;
            pooledObject.transform.rotation = Quaternion.identity;
            pooledObject.SetActive(true);

            ProyectilEnemigo pe = pooledObject.GetComponent<ProyectilEnemigo>();
            if (pe != null)
            {
                pe.Disparar(Vector2.left);
            }

        }
    }

    private void OnDisable()
    {
        CancelInvoke(nameof(GenerarObjetoLoop));
    }
}
