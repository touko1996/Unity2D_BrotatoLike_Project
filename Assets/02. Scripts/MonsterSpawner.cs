using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [Header("스폰 설정")]
    [SerializeField] private string[] monsterPoolNames; // SpawnPoolManager에 등록된 풀 이름들
    [SerializeField] private float spawnInterval = 2f;  // 스폰 주기 (초)
    [SerializeField] private int spawnCountPerWave = 1; // 한 번에 스폰할 몬스터 수
    [SerializeField] private Vector2 spawnAreaMin;      // 스폰 영역 최소
    [SerializeField] private Vector2 spawnAreaMax;      // 스폰 영역 최대

    private float spawnTimer;

    private void Start()
    {
        spawnTimer = spawnInterval;
    }

    private void Update()
    {
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0f)
        {
            SpawnMonsters();
            spawnTimer = spawnInterval;
        }
    }

    private void SpawnMonsters()
    {
        if (monsterPoolNames == null || monsterPoolNames.Length == 0)
        {
            Debug.LogWarning("MonsterSpawner - 스폰할 몬스터 풀 이름이 없습니다!");
            return;
        }

        for (int i = 0; i < spawnCountPerWave; i++)
        {
            // 스폰 위치 랜덤 계산
            Vector2 spawnPos = new Vector2(
                Random.Range(spawnAreaMin.x, spawnAreaMax.x),
                Random.Range(spawnAreaMin.y, spawnAreaMax.y)
            );

            // 랜덤 풀 선택
            string poolName = monsterPoolNames[Random.Range(0, monsterPoolNames.Length)];

            // 풀 매니저에서 스폰
            var monster = SpawnPoolManager.Instance.SpawnFromPool(poolName, spawnPos, Quaternion.identity);
            if (monster == null)
            {
                Debug.LogWarning($"MonsterSpawner - '{poolName}' 풀에서 몬스터를 가져오지 못했습니다!");
                continue;
            }
        }
    }
}
