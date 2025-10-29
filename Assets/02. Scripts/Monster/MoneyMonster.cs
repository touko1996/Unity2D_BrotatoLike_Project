using UnityEngine;

public class MoneyMonster : Monster
{
    [Header("머니 몬스터 설정")]
    [SerializeField] private int minCoinDrop = 3;        // 최소 드롭 개수
    [SerializeField] private int maxCoinDrop = 7;        // 최대 드롭 개수
    [SerializeField] private float wanderRadius = 5f;    // 배회 범위
    [SerializeField] private float changeDirectionTime = 3f; // 방향 변경 주기
    [SerializeField] private GameObject coinPrefab;      // 드롭할 코인 프리팹
    [SerializeField] private float dropRadius = 1.5f;    // 코인 생성 반경

    private Vector2 wanderDirection;
    private float directionTimer;

    protected override void Start()
    {
        base.Start();
        ChooseNewDirection();
    }

    protected override void Update()
    {
        // 단순 배회만 함 (플레이어 추격 X)
        directionTimer -= Time.deltaTime;
        if (directionTimer <= 0)
            ChooseNewDirection();

        rb.MovePosition(rb.position + wanderDirection * moveSpeed * Time.deltaTime);
        spriteRenderer.flipX = wanderDirection.x < 0;
    }

    private void ChooseNewDirection()
    {
        wanderDirection = Random.insideUnitCircle.normalized;
        directionTimer = changeDirectionTime;
    }

    protected override void Die()
    {
        Debug.Log($"{gameObject.name} 사망 - 코인 드롭!");

        int coinCount = Random.Range(minCoinDrop, maxCoinDrop + 1);

        for (int i = 0; i < coinCount; i++)
        {
            if (coinPrefab == null) continue;

            // DropRadius 범위 안 랜덤 위치에 즉시 생성
            Vector2 offset = Random.insideUnitCircle * dropRadius;
            Instantiate(coinPrefab, transform.position + (Vector3)offset, Quaternion.identity);
        }

        gameObject.SetActive(false);
    }
}
