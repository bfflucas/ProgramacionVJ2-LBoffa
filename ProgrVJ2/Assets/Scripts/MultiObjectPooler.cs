using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PoolDefinition
{
    public GameObject prefab;
    public int size = 10;
}

public class MultiObjectPooler : MonoBehaviour
{
    [SerializeField] private PoolDefinition[] pools;

    // Diccionario: cada prefab tiene su propia lista de objetos
    private Dictionary<GameObject, List<GameObject>> poolDict = new Dictionary<GameObject, List<GameObject>>();

    private void Awake()
    {
        foreach (var p in pools)
        {
            var list = new List<GameObject>();
            for (int i = 0; i < p.size; i++)
            {
                GameObject obj = Instantiate(p.prefab);
                obj.SetActive(false);
                list.Add(obj);
            }
            poolDict[p.prefab] = list;
        }
    }

    public GameObject GetPooledObject(GameObject prefab)
    {
        if (!poolDict.ContainsKey(prefab))
        {
            Debug.LogWarning($"No existe pool para el prefab: {prefab.name}");
            return null;
        }

        var list = poolDict[prefab];

        // Buscar un objeto inactivo
        foreach (var o in list)
        {
            if (!o.activeInHierarchy)
                return o;
        }

        // Si no hay disponibles, podés expandir el pool
        GameObject newObj = Instantiate(prefab);
        newObj.SetActive(false);
        list.Add(newObj);
        return newObj;
    }
}
