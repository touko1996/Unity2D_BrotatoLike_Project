using UnityEngine;

/// <summary>
/// [MoneyMonster]
/// ------------------------------------------------------------
/// 머니 몬스터는 플레이어를 추적하지 않고,
/// PerlinWander를 통해 부드럽게 배회만 하는 몬스터이다.
///
/// ▶ 특징
/// - Perlin Noise 기반으로 자연스러운 랜덤 이동을 수행.
/// - 플레이어와 충돌 시 부모 Monster의 OnCollisionEnter2D를 통해
///   데미지를 입힌다.
/// - 일정 반경 내에서 사망 시 코인을 여러 개 드롭한다.
///
/// ▶ 이동 관련
/// 실제 이동은 Monster의 moveSpeed가 아니라
/// PerlinWander의 moveSpeed에 의해 결정된다.
/// ------------------------------------------------------------
/// </summary>
public class MoneyMonster : Monster
{
    [Header("머니 몬스터 설정")]
    [Tooltip("최소 드롭 개수")]
    [SerializeField] private int minCoinDrop = 3;

    [Tooltip("최대 드롭 개수")]
    [SerializeField] private int maxCoinDrop = 7;

    [Tooltip("드롭할 코인 프리팹")]
    [SerializeField] private GameObject coinPrefab;

    [Tooltip("코인이 퍼질 반경")]
    [SerializeField] private float dropRadius = 1.5f;

    private PerlinWander wander;

    protected override void Start()
    {
        base.Start();

        // PerlinWander 컴포넌트를 찾아서 자동 배회 시작
        wander = GetComponent<PerlinWander>();
        if (wander != null)
        {
            wander.StartWander();
        }
    }

    protected override void Update()
    {
        // 플레이어 추적 로직은 완전히 비활성화
        // PerlinWander가 자동으로 이동을 처리하므로 Update는 비워둔다.
    }

    protected override void Die()
    {
        Debug.Log($"{gameObject.name} 사망 - 코인 드롭!");

        int coinCount = Random.Range(minCoinDrop, maxCoinDrop + 1);

        for (int i = 0; i < coinCount; i++)
        {
            if (coinPrefab == null) continue;

            // DropRadius 범위 내 랜덤 위치에 코인 생성
            Vector2 offset = Random.insideUnitCircle * dropRadius;
            Instantiate(coinPrefab, transform.position + (Vector3)offset, Quaternion.identity);
        }

        gameObject.SetActive(false);
    }

    // 충돌 시 부모(Monster)의 OnCollisionEnter2D가 자동으로 호출됨
    // → 플레이어에게 contactDamage를 적용
}
