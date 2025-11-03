using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MonsterSpawnInfo
{
    public string monsterTag;       // 풀에서 찾을 이름 (ex: "NormalMonster")
    public float spawnInterval;     // 개별 스폰 간격
}

[System.Serializable]
public class WaveData
{
    public int waveNumber;
    public List<MonsterSpawnInfo> monsters = new List<MonsterSpawnInfo>();
}

public class MonsterSpawner : MonoBehaviour
{
    [Header("웨이브별 스폰 설정")]
    public List<WaveData> waveSettings = new List<WaveData>();

    [Header("연결된 매니저")]
    public SpawnPoolManager spawnPoolManager;  // 풀 매니저 연결
    public Transform player;

    [Header("스폰 범위 설정")]
    public float spawnRangeX = 15f;
    public float spawnRangeY = 15f;
    public float minSpawnDistanceFromPlayer = 5f;

    private List<Coroutine> activeCoroutines = new List<Coroutine>();
    private int currentWave = 1;

    private void Start()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (spawnPoolManager == null)
            spawnPoolManager = FindObjectOfType<SpawnPoolManager>();
    }

    // 웨이브 시작 시 호출
    public void SetWave(int wave)
    {
        StopAllCoroutines();
        activeCoroutines.Clear();

        currentWave = wave;
        WaveData data = waveSettings.Find(w => w.waveNumber == wave);

        if (data == null)
        {
            Debug.LogWarning("Wave 설정 없음: " + wave);
            return;
        }

        foreach (MonsterSpawnInfo info in data.monsters)
        {
            Coroutine c = StartCoroutine(SpawnRoutine(info));
            activeCoroutines.Add(c);
        }

        Debug.Log($"[MonsterSpawner] Wave {wave} 시작됨 (스폰 종류: {data.monsters.Count})");
    }

    private IEnumerator SpawnRoutine(MonsterSpawnInfo info)
    {
        while (true)
        {
            yield return new WaitForSeconds(info.spawnInterval);

            if (player == null || spawnPoolManager == null)
                continue;

            Vector2 spawnPos;
            int safetyLimit = 100;
            do
            {
                spawnPos = new Vector2(Random.Range(-spawnRangeX, spawnRangeX),
                                       Random.Range(-spawnRangeY, spawnRangeY));
                safetyLimit--;
                if (safetyLimit <= 0)
                {
                    Debug.LogWarning("Spawn 위치 찾기 실패");
                    yield break;
                }
            } while (Vector2.Distance(spawnPos, player.position) < minSpawnDistanceFromPlayer);

            // 오브젝트 풀에서 소환
            GameObject monster = spawnPoolManager.SpawnFromPool(info.monsterTag, spawnPos, Quaternion.identity);

            if (monster == null)
                Debug.LogWarning($"[MonsterSpawner] {info.monsterTag} 풀에서 몬스터를 불러올 수 없음");
        }
    }
    public void StopSpawning()
    {
        StopAllCoroutines();
        activeCoroutines.Clear();
        Debug.Log($"[MonsterSpawner] Wave {currentWave} 스폰 중지됨");
    }

}
