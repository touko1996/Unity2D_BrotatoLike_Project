using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MonsterSpawnInfo
{
    public string monsterTag;
    public float spawnInterval;
}

[System.Serializable]
public class WaveData
{
    public int waveNumber;
    public List<MonsterSpawnInfo> monsters = new List<MonsterSpawnInfo>();
}

public class MonsterSpawner : MonoBehaviour
{
    [Header("Wave Settings")]
    public List<WaveData> waveSettings = new List<WaveData>();

    [Header("Managers")]
    public SpawnPoolManager spawnPoolManager;
    public Transform player;

    [Header("Spawn Range")]
    public float spawnRangeX = 15f;
    public float spawnRangeY = 15f;
    public float minSpawnDistanceFromPlayer = 5f;

    [Header("Scaling")]
    public float hpMultiplierPerWave = 1.1f;
    public float damageMultiplierPerWave = 1.05f;
    public float speedMultiplierPerWave = 1.03f;

    private List<Coroutine> activeCoroutines = new List<Coroutine>();
    private int currentWave = 1;
    private bool canSpawn = true;

    private void Start()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (spawnPoolManager == null)
            spawnPoolManager = FindObjectOfType<SpawnPoolManager>();
    }

    public void SetWave(int wave)
    {
        StopAllCoroutines();
        activeCoroutines.Clear();

        canSpawn = true;
        currentWave = wave;

        WaveData data = waveSettings.Find(w => w.waveNumber == wave);
        if (data == null)
        {
            Debug.LogWarning("No wave data for wave: " + wave);
            return;
        }

        foreach (MonsterSpawnInfo info in data.monsters)
        {
            Coroutine c = StartCoroutine(SpawnRoutine(info));
            activeCoroutines.Add(c);
        }

        Debug.Log("[MonsterSpawner] wave " + wave + " started.");
    }

    public void StopSpawningEarly()
    {
        canSpawn = false;
        Debug.Log("[MonsterSpawner] spawn stopped early.");
    }

    public void StopSpawning()
    {
        canSpawn = false;
        StopAllCoroutines();
        activeCoroutines.Clear();
        Debug.Log("[MonsterSpawner] spawn stopped.");
    }

    private IEnumerator SpawnRoutine(MonsterSpawnInfo info)
    {
        bool isBoss = info.monsterTag.Contains("Boss");

        if (isBoss)
        {
            yield return new WaitForSeconds(1f);

            Vector2 spawnPos;
            int safety = 100;
            do
            {
                spawnPos = new Vector2(
                    Random.Range(-spawnRangeX, spawnRangeX),
                    Random.Range(-spawnRangeY, spawnRangeY)
                );
                safety--;
            }
            while (Vector2.Distance(spawnPos, player.position) < minSpawnDistanceFromPlayer && safety > 0);

            GameObject bossObj = spawnPoolManager.SpawnFromPool(info.monsterTag, spawnPos, Quaternion.identity);
            if (bossObj != null)
            {
                Debug.Log("[MonsterSpawner] boss spawned: " + bossObj.name);

                Monster m = bossObj.GetComponent<Monster>();
                if (m != null)
                {
                    float hpScale = Mathf.Pow(hpMultiplierPerWave, currentWave - 1);
                    float dmgScale = Mathf.Pow(damageMultiplierPerWave, currentWave - 1);
                    float spdScale = Mathf.Pow(speedMultiplierPerWave, currentWave - 1);
                    m.SetWaveScaling(hpScale, dmgScale, spdScale);
                }

                UI_BossHP bossUI = FindObjectOfType<UI_BossHP>(true);
                BossMonster boss = bossObj.GetComponent<BossMonster>();
                if (bossUI != null && boss != null)
                {
                    bossUI.InitBoss(boss);
                }
            }

            yield break;
        }

        while (true)
        {
            yield return new WaitForSeconds(info.spawnInterval);

            if (!canSpawn || player == null || spawnPoolManager == null)
                continue;

            Vector2 spawnPos;
            int safety = 100;
            do
            {
                spawnPos = new Vector2(
                    Random.Range(-spawnRangeX, spawnRangeX),
                    Random.Range(-spawnRangeY, spawnRangeY)
                );
                safety--;
            }
            while (Vector2.Distance(spawnPos, player.position) < minSpawnDistanceFromPlayer && safety > 0);

            GameObject monsterObj = spawnPoolManager.SpawnFromPool(info.monsterTag, spawnPos, Quaternion.identity);
            if (monsterObj == null) continue;

            Monster monster = monsterObj.GetComponent<Monster>();
            if (monster != null)
            {
                float hpScale = Mathf.Pow(hpMultiplierPerWave, currentWave - 1);
                float dmgScale = Mathf.Pow(damageMultiplierPerWave, currentWave - 1);
                float spdScale = Mathf.Pow(speedMultiplierPerWave, currentWave - 1);
                monster.SetWaveScaling(hpScale, dmgScale, spdScale);
            }
        }
    }
}
