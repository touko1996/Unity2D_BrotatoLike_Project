using UnityEngine;

public class BossAttack : MonoBehaviour
{
    private BossMonster boss;

    private void Start()
    {
        boss = FindObjectOfType<BossMonster>();
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false; // 처음에는 비활성화 (경고 표시용)
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (boss != null)
        {
            boss.DealDamageToPlayer(other);
        }
    }
}
