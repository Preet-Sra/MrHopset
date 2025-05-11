using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [System.Serializable]
    public class PoolItem
    {
        public string categoryName; // e.g. "Obstacle", "PowerUp"
        public string prefabName;
        public GameObject prefab;
        public int poolSize = 10;
    }

    public List<PoolItem> poolItems;
    public static ObjectPooler instance;

    private Dictionary<string, List<GameObject>> poolDictionary = new Dictionary<string, List<GameObject>>();
    private Dictionary<string, GameObject> prefabDictionary = new Dictionary<string, GameObject>();
    private Dictionary<string, List<string>> categoryMap = new Dictionary<string, List<string>>();

    void Awake()
    {
        if (instance == null)
            instance = this;
        InitializePool();
    }

    void InitializePool()
    {
        foreach (PoolItem item in poolItems)
        {
            if (!prefabDictionary.ContainsKey(item.prefabName))
                prefabDictionary.Add(item.prefabName, item.prefab);

            if (!poolDictionary.ContainsKey(item.prefabName))
                poolDictionary[item.prefabName] = new List<GameObject>();

            if (!categoryMap.ContainsKey(item.categoryName))
                categoryMap[item.categoryName] = new List<string>();

            categoryMap[item.categoryName].Add(item.prefabName);

            for (int i = 0; i < item.poolSize; i++)
            {
                GameObject obj = Instantiate(item.prefab, transform);
                obj.SetActive(false);
                poolDictionary[item.prefabName].Add(obj);
            }
        }
    }

    public GameObject GetRandomFromCategory(string categoryName)
    {
        if (!categoryMap.ContainsKey(categoryName))
        {
            Debug.LogWarning("Category not found: " + categoryName);
            return null;
        }

        List<string> prefabNames = categoryMap[categoryName];
        string randomPrefabName = prefabNames[Random.Range(0, prefabNames.Count)];
        return GetObject(randomPrefabName);
    }

    public GameObject GetObject(string prefabName)
    {
        if (poolDictionary.ContainsKey(prefabName))
        {
            foreach (GameObject obj in poolDictionary[prefabName])
            {
                if (!obj.activeInHierarchy)
                {
                    obj.SetActive(true);
                    return obj;
                }
            }

            // Expand pool if needed
            if (prefabDictionary.ContainsKey(prefabName))
            {
                GameObject newObj = Instantiate(prefabDictionary[prefabName]);
                newObj.SetActive(true);
                poolDictionary[prefabName].Add(newObj);
                return newObj;
            }
        }

        Debug.LogWarning("Prefab not found: " + prefabName);
        return null;
    }

    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
    }
}
