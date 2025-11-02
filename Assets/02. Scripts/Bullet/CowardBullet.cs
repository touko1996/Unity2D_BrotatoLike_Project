using UnityEngine;

// 몬스터 전용 투사체 (플레이어에게만 데미지)
public class CowardBullet: MonoBehaviour
{
    [Header("Bullet Settings")]
    public float lifetime = 2f;       // 총알 지속시간
    public float damage = 10f;        // 총알 데미지

    private void Start()
    {
        // 지정된 시간 후 자동 파괴
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 플레이어에게만 반응
        if (other.CompareTag("Player"))
        {
            PlayerStats playerStats = other.GetComponent<PlayerStats>();
            if (playerStats != null)
                playerStats.TakeDamage(damage);

            Destroy(gameObject);
        }
    }
}
