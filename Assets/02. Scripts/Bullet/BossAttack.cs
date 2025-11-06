using UnityEngine;

/// <summary>
/// [BossAttack]
/// --------------------------------------------------------------------
/// 보스 몬스터의 공격 판정 스크립트.
/// - BossMonster의 공격 프리팹(예: 장판, 브레스 등)에 부착.
/// - Collider2D가 플레이어와 충돌하면 보스의 DealDamageToPlayer()를 호출.
/// - 시작 시 Collider는 비활성화되어 있으며,
///   실제 공격 타이밍에 BossMonster에서 활성화함.
/// --------------------------------------------------------------------
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class BossAttack : MonoBehaviour
{
    private BossMonster bossMonster;
    private Collider2D attackCollider;

    private void Awake()
    {
        // Collider 없을 경우 자동 추가
        attackCollider = GetComponent<Collider2D>();
        if (attackCollider == null)
            attackCollider = gameObject.AddComponent<BoxCollider2D>();

        // 기본적으로 비활성화 (경고 표시만)
        attackCollider.enabled = false;
    }

    private void Start()
    {
        // 보스 참조 (씬 내 보스는 1개만 존재한다고 가정)
        bossMonster = FindObjectOfType<BossMonster>();
    }

    /// <summary>
    /// Collider가 활성화된 상태에서 플레이어와 접촉 시 데미지 전달
    /// </summary>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (bossMonster == null) return;

        if (other.CompareTag("Player"))
        {
            bossMonster.DealDamageToPlayer(other);
        }
    }

    /// <summary>
    /// 외부(BossMonster)에서 호출하여 Collider를 켜거나 끄는 함수
    /// </summary>
    public void SetColliderActive(bool active)
    {
        if (attackCollider != null)
            attackCollider.enabled = active;
    }
}
