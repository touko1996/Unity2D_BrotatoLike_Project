using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [Header("���� ����")]
    [SerializeField] private string[] monsterPoolNames; // SpawnPoolManager�� ��ϵ� Ǯ �̸���
    [SerializeField] private float spawnInterval = 2f;  // ���� �ֱ� (��)
    [SerializeField] private int spawnCountPerWave = 1; // �� ���� ������ ���� ��
    [SerializeField] private Vector2 spawnAreaMin;      // ���� ���� �ּ�
    [SerializeField] private Vector2 spawnAreaMax;      // ���� ���� �ִ�

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
            Debug.LogWarning("MonsterSpawner - ������ ���� Ǯ �̸��� �����ϴ�!");
            return;
        }

        for (int i = 0; i < spawnCountPerWave; i++)
        {
            // ���� ��ġ ���� ���
            Vector2 spawnPos = new Vector2(
                Random.Range(spawnAreaMin.x, spawnAreaMax.x),
                Random.Range(spawnAreaMin.y, spawnAreaMax.y)
            );

            // ���� Ǯ ����
            string poolName = monsterPoolNames[Random.Range(0, monsterPoolNames.Length)];

            // Ǯ �Ŵ������� ����
            var monster = SpawnPoolManager.Instance.SpawnFromPool(poolName, spawnPos, Quaternion.identity);
            if (monster == null)
            {
                Debug.LogWarning($"MonsterSpawner - '{poolName}' Ǯ���� ���͸� �������� ���߽��ϴ�!");
                continue;
            }
        }
    }
}
