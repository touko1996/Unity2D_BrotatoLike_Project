using UnityEngine;

public class CowardMonster : Monster
{
    [Header("원거리 몬스터 설정")]
    [SerializeField] private float stopRange = 4f;      // 너무 가까워지면 도망치는 거리
    [SerializeField] private float projectileDamage = 10f;
    [SerializeField] private float projectileSpeed = 8f;
    [SerializeField] private float shootInterval = 2f;
    [SerializeField] private GameObject projectilePrefab;

    private float shootTimer;

    protected override void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(player.position, transform.position);

        // 너무 가까우면 후퇴
        if (distance < stopRange)
        {
            Vector2 dir = (transform.position - player.position).normalized;
            rb.MovePosition(rb.position + dir * moveSpeed * Time.deltaTime);
        }
        else
        {
            // 일정 거리 이상이면 계속 추격
            base.Move();
        }

        // 사거리 안에서는 투사체 발사
        shootTimer -= Time.deltaTime;
        if (shootTimer <= 0f)
        {
            ShootProjectile();
            shootTimer = shootInterval;
        }

        // 좌우 방향 반전
        float deltaX = player.position.x - transform.position.x;
        if (deltaX > 0) spriteRenderer.flipX = false;
        else if (deltaX < 0) spriteRenderer.flipX = true;
    }

    private void ShootProjectile()
    {
        if (projectilePrefab == null) return;

        Vector2 dir = (player.position - transform.position).normalized;
        GameObject bullet = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        bulletRb.velocity = dir * projectileSpeed;

        Debug.Log($"{gameObject.name}이(가) 투사체 발사!");
    }
}
