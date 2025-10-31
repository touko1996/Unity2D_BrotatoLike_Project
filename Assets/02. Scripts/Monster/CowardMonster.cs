using UnityEngine;

public class CowardMonster : Monster
{
    [Header("원거리 몬스터 설정")]
    [SerializeField] private float stopRange = 4f;       // 너무 가까워지면 도망치는 거리
    [SerializeField] private float attackRange = 7f;     // 공격 사거리
    [SerializeField] private float projectileSpeed = 8f; // 투사체 속도
    [SerializeField] private float shootInterval = 2f;   // 발사 간격
    [SerializeField] private GameObject projectilePrefab; // CowardBullet 프리팹

    private float shootTimer;

    protected override void Update()
    {
        if (player == null) return;
        if (Time.timeScale == 0f) return;

        float distance = Vector2.Distance(player.position, transform.position);

        // 1. 너무 가까우면 플레이어 반대 방향으로 도망
        if (distance < stopRange)
        {
            Vector2 dir = (transform.position - player.position).normalized;
            rb.MovePosition(rb.position + dir * moveSpeed * Time.deltaTime);
        }
        // 2. 공격 사거리 안에서는 제자리에서 공격
        else if (distance <= attackRange)
        {
            shootTimer -= Time.deltaTime;
            if (shootTimer <= 0f)
            {
                ShootProjectile();
                shootTimer = shootInterval;
            }
        }
        // 3. 너무 멀면 플레이어를 향해 천천히 접근
        else
        {
            base.Move();
        }

        // 좌우 방향 반전
        float deltaX = player.position.x - transform.position.x;
        if (deltaX > 0) spriteRenderer.flipX = false;
        else if (deltaX < 0) spriteRenderer.flipX = true;
    }

    private void ShootProjectile()
    {
        if (projectilePrefab == null) return;

        // 발사 방향 계산
        Vector2 dir = (player.position - transform.position).normalized;

        // CowardBullet 프리팹 생성
        GameObject bulletObj = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        // Rigidbody 설정
        Rigidbody2D rb2d = bulletObj.GetComponent<Rigidbody2D>();
        if (rb2d == null)
        {
            rb2d = bulletObj.AddComponent<Rigidbody2D>();
            rb2d.gravityScale = 0f;
            rb2d.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        }

        // Collider가 있다면 Trigger 모드 보장
        Collider2D col = bulletObj.GetComponent<Collider2D>();
        if (col != null) col.isTrigger = true;

        // 투사체 이동 방향 및 속도 지정
        rb2d.velocity = dir * projectileSpeed;

        // CowardBullet 스크립트가 있다면 회전방향 세팅
        CowardBullet bullet = bulletObj.GetComponent<CowardBullet>();
        if (bullet != null)
        {
            // CowardBullet이 스스로 damage, lifetime을 관리하므로 따로 세팅 불필요
            bullet.transform.right = dir;
        }
    }
}
