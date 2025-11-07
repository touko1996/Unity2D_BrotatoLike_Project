using UnityEngine;

/// <summary>
/// [CowardBullet]
/// --------------------------------------------------------------------
/// 도망형 몬스터(CowardMonster)가 발사하는 투사체.
/// - 플레이어에게만 데미지를 입힘.
/// - 지정된 시간(lifetime)이 지나면 자동 파괴.
/// - Rigidbody는 사용하지 않음 (직선 이동).
/// --------------------------------------------------------------------
/// </summary>
public class CowardBullet : MonoBehaviour
{
    [Header("투사체 기본 설정")]
    [SerializeField] private float lifetime = 2f;
    [SerializeField] private float damage = 7f;

    private void Start()
    {
        // 일정 시간이 지나면 자동으로 파괴
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 플레이어 태그만 반응
        if (!other.CompareTag("Player"))
            return;

        // 플레이어 스탯 참조
        PlayerStats playerStats = other.GetComponent<PlayerStats>();
        if (playerStats != null)
        {
            // 데미지 적용
            playerStats.TakeDamage(damage);
        }

        // 충돌 후 즉시 파괴
        Destroy(gameObject);
    }
}
