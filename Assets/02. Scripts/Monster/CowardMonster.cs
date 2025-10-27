using UnityEngine;

public class CowardMonster : Monster
{
    [Header("���Ÿ� ���� ����")]
    [SerializeField] private float stopRange = 4f;      // �ʹ� ��������� ����ġ�� �Ÿ�
    [SerializeField] private float projectileDamage = 10f;
    [SerializeField] private float projectileSpeed = 8f;
    [SerializeField] private float shootInterval = 2f;
    [SerializeField] private GameObject projectilePrefab;

    private float shootTimer;

    protected override void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(player.position, transform.position);

        // �ʹ� ������ ����
        if (distance < stopRange)
        {
            Vector2 dir = (transform.position - player.position).normalized;
            rb.MovePosition(rb.position + dir * moveSpeed * Time.deltaTime);
        }
        else
        {
            // ���� �Ÿ� �̻��̸� ��� �߰�
            base.Move();
        }

        // ��Ÿ� �ȿ����� ����ü �߻�
        shootTimer -= Time.deltaTime;
        if (shootTimer <= 0f)
        {
            ShootProjectile();
            shootTimer = shootInterval;
        }

        // �¿� ���� ����
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

        Debug.Log($"{gameObject.name}��(��) ����ü �߻�!");
    }
}
