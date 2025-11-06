using UnityEngine;

public class CowardMonster : Monster
{
    [Header("원거리 몬스터 설정")]
    [SerializeField] private float retreatRange = 4f;        // 플레이어가 너무 가까울 때 도망치는 거리
    [SerializeField] private float attackRange = 7f;         // 공격 가능한 거리
    [SerializeField] private float projectileSpeed = 8f;     // 투사체 속도
    [SerializeField] private float shootCooldown = 2f;       // 공격 간격
    [SerializeField] private GameObject projectilePrefab;    // CowardBullet 프리팹

    private float shootTimer = 0f;                           // 공격 타이머

    // 매 프레임마다 플레이어 거리 기반 행동 결정
    protected override void FixedUpdate()
    {
        if (playerTransform == null) return;
        if (Time.timeScale == 0f) return;

        // 플레이어와의 거리 계산
        float distanceToPlayer = Vector2.Distance(playerTransform.position, transform.position);

        // 1. 플레이어가 너무 가까우면 반대 방향으로 도망
        if (distanceToPlayer < retreatRange)
        {
            Vector2 retreatDirection = (transform.position - playerTransform.position).normalized;
            rigidBody.MovePosition(rigidBody.position + retreatDirection * moveSpeed * Time.deltaTime);
        }
        // 2. 사거리 안에서는 제자리에서 공격
        else if (distanceToPlayer <= attackRange)
        {
            shootTimer -= Time.deltaTime;
            if (shootTimer <= 0f)
            {
                ShootProjectile();
                shootTimer = shootCooldown; // 쿨타임 초기화
            }
        }
        // 3. 사거리 밖이면 플레이어 쪽으로 천천히 접근
        else
        {
            base.Move();
        }

        // 플레이어의 위치에 따라 스프라이트 방향 반전
        float deltaX = playerTransform.position.x - transform.position.x;
        if (deltaX > 0)
            spriteRenderer.flipX = false;
        else if (deltaX < 0)
            spriteRenderer.flipX = true;
    }

    // 투사체 발사
    private void ShootProjectile()
    {
        if (projectilePrefab == null) return;

        // 발사 방향 계산
        Vector2 shootDirection = (playerTransform.position - transform.position).normalized;

        // CowardBullet 프리팹 생성
        GameObject bulletObj = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        // Rigidbody2D 설정
        Rigidbody2D bulletRb = bulletObj.GetComponent<Rigidbody2D>();
        if (bulletRb == null)
        {
            bulletRb = bulletObj.AddComponent<Rigidbody2D>();
            bulletRb.gravityScale = 0f;
            bulletRb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        }

        // Collider가 있다면 Trigger 모드로 설정
        Collider2D col = bulletObj.GetComponent<Collider2D>();
        if (col != null)
            col.isTrigger = true;

        // 투사체 속도 및 방향 지정
        bulletRb.velocity = shootDirection * projectileSpeed;

        // CowardBullet 스크립트가 있다면 방향 회전 설정
        CowardBullet bullet = bulletObj.GetComponent<CowardBullet>();
        if (bullet != null)
        {
            bullet.transform.right = shootDirection;
        }
    }
}
