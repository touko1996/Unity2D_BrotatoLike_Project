using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Pool
{
    [Header("풀 개별 설정")]
    public string poolName;          // 풀 이름 (식별용)
    public GameObject prefab;        // 프리팹
    public int poolSize = 10;        // 초기 생성 수
}

public class SpawnPoolManager : MonoBehaviour
{
    public static SpawnPoolManager Instance { get; private set; }

    [Header("풀 목록")]
    public List<Pool> pools = new List<Pool>();

    private Dictionary<string, Queue<GameObject>> poolDictionary;

    private void Awake()
    {
        // 싱글톤
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
                Debug.LogWarning($"풀 '{pool.poolName}'에 프리팹이 지정되지 않았습니다!");
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

        Debug.Log($"SpawnPoolManager - {pools.Count}개의 풀 초기화 완료!");
    }

    public GameObject SpawnFromPool(string poolName, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(poolName))
        {
            Debug.LogWarning($"'{poolName}' 풀을 찾을 수 없습니다!");
            return null;
        }

        GameObject obj = null;

        // 비활성화된 오브젝트 찾기
        Queue<GameObject> objectPool = poolDictionary[poolName];

        if (objectPool.Count > 0 && !objectPool.Peek().activeInHierarchy)
        {
            obj = objectPool.Dequeue();
        }
        else
        {
            // 풀의 모든 오브젝트가 사용 중일 경우 새로 생성
            Pool pool = pools.Find(p => p.poolName == poolName);
            obj = Instantiate(pool.prefab);
        }

        obj.transform.position = position;
        obj.transform.rotation = rotation;
        obj.SetActive(true);

        // 다시 큐에 추가 (순환 구조 유지)
        objectPool.Enqueue(obj);

        return obj;
    }
}
