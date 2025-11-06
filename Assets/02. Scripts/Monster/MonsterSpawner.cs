using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MonsterSpawnInfo
{
    public string monsterTag;        // 스폰할 몬스터의 풀 태그
    public float spawnInterval;      // 해당 몬스터의 스폰 간격
}

[System.Serializable]
public class WaveData
{
    public int waveNumber;                           // 웨이브 번호
    public List<MonsterSpawnInfo> monsters = new();  // 해당 웨이브에서 스폰될 몬스터 목록
}

public class MonsterSpawner : MonoBehaviour
{
    [Header("웨이브 설정")]
    public List<WaveData> waveSettings = new();

    [Header("매니저 참조")]
    public SpawnPoolManager spawnPoolManager;
    public Transform playerTransform;

    [Header("스폰 범위 설정")]
    public float spawnRangeX = 15f;
    public float spawnRangeY = 15f;
    public float minSpawnDistanceFromPlayer = 5f;

    [Header("웨이브별 능력치 배율")]
    public float hpMultiplierPerWave = 1.1f;
    public float damageMultiplierPerWave = 1.05f;
    public float speedMultiplierPerWave = 1.03f;

    private List<Coroutine> activeSpawnCoroutines = new();
    private int currentWave = 1;
    private bool isSpawning = true;

    private void Start()
    {
        if (playerTransform == null)
            playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (spawnPoolManager == null)
            spawnPoolManager = FindObjectOfType<SpawnPoolManager>();
    }

    public void SetWave(int wave)
    {
        StopAllCoroutines();
        activeSpawnCoroutines.Clear();

        isSpawning = true;
        currentWave = wave;

        WaveData data = waveSettings.Find(w => w.waveNumber == wave);
        if (data == null)
        {
            return;
        }

        foreach (MonsterSpawnInfo info in data.monsters)
        {
            Coroutine corutine = StartCoroutine(SpawnRoutine(info));
            activeSpawnCoroutines.Add(corutine);
        }
    }

    public void StopSpawningEarly()
    {
        isSpawning = false;
    }

    public void StopSpawning()
    {
        isSpawning = false;
        StopAllCoroutines();
        activeSpawnCoroutines.Clear();
    }

    private IEnumerator SpawnRoutine(MonsterSpawnInfo info)
    {
        bool isBoss = info.monsterTag.Contains("Boss");

        // 보스 스폰
        if (isBoss)
        {
            yield return new WaitForSeconds(1f);

            Vector2 spawnPosition = GetValidSpawnPosition();
            GameObject bossObj = spawnPoolManager.SpawnFromPool(info.monsterTag, spawnPosition, Quaternion.identity);
            if (bossObj != null)
            {
                Monster monster = bossObj.GetComponent<Monster>();
                if (monster != null)
                    ApplyWaveScalingToMonster(monster);

                UI_BossHP bossUI = FindObjectOfType<UI_BossHP>(true);
                BossMonster boss = bossObj.GetComponent<BossMonster>();
                if (bossUI != null && boss != null)
                    bossUI.InitBoss(boss);
            }

            yield break;
        }

        while (true)
        {
            yield return new WaitForSeconds(info.spawnInterval);

            if (!isSpawning || playerTransform == null || spawnPoolManager == null)
                continue;

            Vector2 spawnPosition = GetValidSpawnPosition();

            GameObject monsterObj = spawnPoolManager.SpawnFromPool(info.monsterTag, spawnPosition, Quaternion.identity);
            if (monsterObj == null)
                continue;

            Monster monster = monsterObj.GetComponent<Monster>();
            if (monster != null)
                ApplyWaveScalingToMonster(monster);
        }
    }

    // 플레이어 근처를 피해서 스폰할 위치를 구함
    private Vector2 GetValidSpawnPosition()
    {
        Vector2 spawnPos;
        int safetyCount = 100;

        do
        {
            spawnPos = new Vector2(
                Random.Range(-spawnRangeX, spawnRangeX),
                Random.Range(-spawnRangeY, spawnRangeY)
            );
            safetyCount--;
        }
        while (Vector2.Distance(spawnPos, playerTransform.position) < minSpawnDistanceFromPlayer && safetyCount > 0);

        return spawnPos;
    }

    // 웨이브 수에 따른 스케일을 몬스터에 적용
    private void ApplyWaveScalingToMonster(Monster monster)
    {
        float hpScale = Mathf.Pow(hpMultiplierPerWave, currentWave - 1);
        float dmgScale = Mathf.Pow(damageMultiplierPerWave, currentWave - 1);
        float spdScale = Mathf.Pow(speedMultiplierPerWave, currentWave - 1);

        // 여기서 이름을 Monster 쪽에 맞춰서 호출
        monster.ApplyWaveScaling(hpScale, dmgScale, spdScale);
    }
}
