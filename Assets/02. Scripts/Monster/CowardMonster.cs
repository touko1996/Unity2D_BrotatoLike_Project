using UnityEngine;

public class CowardMonster : Monster
{
    [Header("���Ÿ� ���� ����")]
    [SerializeField] private float stopRange = 4f;       // �ʹ� ��������� ����ġ�� �Ÿ�
    [SerializeField] private float attackRange = 7f;     // ���� ��Ÿ�
    [SerializeField] private float projectileSpeed = 8f; // ����ü �ӵ�
    [SerializeField] private float shootInterval = 2f;   // �߻� ����
    [SerializeField] private GameObject projectilePrefab; // CowardBullet ������

    private float shootTimer;

    protected override void Update()
    {
        if (player == null) return;
        if (Time.timeScale == 0f) return;

        float distance = Vector2.Distance(player.position, transform.position);

        // 1. �ʹ� ������ �÷��̾� �ݴ� �������� ����
        if (distance < stopRange)
        {
            Vector2 dir = (transform.position - player.position).normalized;
            rb.MovePosition(rb.position + dir * moveSpeed * Time.deltaTime);
        }
        // 2. ���� ��Ÿ� �ȿ����� ���ڸ����� ����
        else if (distance <= attackRange)
        {
            shootTimer -= Time.deltaTime;
            if (shootTimer <= 0f)
            {
                ShootProjectile();
                shootTimer = shootInterval;
            }
        }
        // 3. �ʹ� �ָ� �÷��̾ ���� õõ�� ����
        else
        {
            base.Move();
        }

        // �¿� ���� ����
        float deltaX = player.position.x - transform.position.x;
        if (deltaX > 0) spriteRenderer.flipX = false;
        else if (deltaX < 0) spriteRenderer.flipX = true;
    }

    private void ShootProjectile()
    {
        if (projectilePrefab == null) return;

        // �߻� ���� ���
        Vector2 dir = (player.position - transform.position).normalized;

        // CowardBullet ������ ����
        GameObject bulletObj = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        // Rigidbody ����
        Rigidbody2D rb2d = bulletObj.GetComponent<Rigidbody2D>();
        if (rb2d == null)
        {
            rb2d = bulletObj.AddComponent<Rigidbody2D>();
            rb2d.gravityScale = 0f;
            rb2d.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        }

        // Collider�� �ִٸ� Trigger ��� ����
        Collider2D col = bulletObj.GetComponent<Collider2D>();
        if (col != null) col.isTrigger = true;

        // ����ü �̵� ���� �� �ӵ� ����
        rb2d.velocity = dir * projectileSpeed;

        // CowardBullet ��ũ��Ʈ�� �ִٸ� ȸ������ ����
        CowardBullet bullet = bulletObj.GetComponent<CowardBullet>();
        if (bullet != null)
        {
            // CowardBullet�� ������ damage, lifetime�� �����ϹǷ� ���� ���� ���ʿ�
            bullet.transform.right = dir;
        }
    }
}
