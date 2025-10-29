using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Pool
{
    [Header("Ǯ ���� ����")]
    public string poolName;          // Ǯ �̸� (�ĺ���)
    public GameObject prefab;        // ������
    public int poolSize = 10;        // �ʱ� ���� ��
}

public class SpawnPoolManager : MonoBehaviour
{
    public static SpawnPoolManager Instance { get; private set; }

    [Header("Ǯ ���")]
    public List<Pool> pools = new List<Pool>();

    private Dictionary<string, Queue<GameObject>> poolDictionary;

    private void Awake()
    {
        // �̱���
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        InitializePools();
    }

    private void InitializePools()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            if (pool.prefab == null)
            {
                Debug.LogWarning($"Ǯ '{pool.poolName}'�� �������� �������� �ʾҽ��ϴ�!");
                continue;
            }

            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.poolSize; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.poolName, objectPool);
        }

        Debug.Log($"SpawnPoolManager - {pools.Count}���� Ǯ �ʱ�ȭ �Ϸ�!");
    }

    public GameObject SpawnFromPool(string poolName, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(poolName))
        {
            Debug.LogWarning($"'{poolName}' Ǯ�� ã�� �� �����ϴ�!");
            return null;
        }

        GameObject obj = null;

        // ��Ȱ��ȭ�� ������Ʈ ã��
        Queue<GameObject> objectPool = poolDictionary[poolName];

        if (objectPool.Count > 0 && !objectPool.Peek().activeInHierarchy)
        {
            obj = objectPool.Dequeue();
        }
        else
        {
            // Ǯ�� ��� ������Ʈ�� ��� ���� ��� ���� ����
            Pool pool = pools.Find(p => p.poolName == poolName);
            obj = Instantiate(pool.prefab);
        }

        obj.transform.position = position;
        obj.transform.rotation = rotation;
        obj.SetActive(true);

        // �ٽ� ť�� �߰� (��ȯ ���� ����)
        objectPool.Enqueue(obj);

        return obj;
    }
}
