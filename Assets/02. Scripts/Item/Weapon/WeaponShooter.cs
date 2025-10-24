using UnityEngine;

// ������ ���� �� �߻� ����
public class WeaponShooter : MonoBehaviour
{
    [Header("����")]
    public Transform player;
    public WeaponData weaponData;

    private PlayerStats playerStats;
    private float cooldownTimer = 0f;

    void Start()
    {
        if (player != null)
            playerStats = player.GetComponent<PlayerStats>();
    }

    void Update()
    {
        if (player == null || weaponData == null) return;

        // ���� ����� ���� ã��
        GameObject nearestEnemy = FindClosestEnemy();
        if (nearestEnemy == null) return;

        // ���� ���� ���
        Vector2 direction = (nearestEnemy.transform.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        // ���� ��Ÿ�� ���
        float attackSpeed = playerStats != null ? playerStats.currentAttackSpeed : weaponData.fireRate;
        cooldownTimer -= Time.deltaTime;

        if (cooldownTimer <= 0f)
        {
            FireProjectile(direction);
            cooldownTimer = 1f / attackSpeed;
        }
    }

    // ����ü �߻�
    void FireProjectile(Vector2 direction)
    {
        if (weaponData.projectilePrefab == null) return;

        GameObject bulletObj = Instantiate(weaponData.projectilePrefab, transform.position, Quaternion.identity);
        Rigidbody2D rb = bulletObj.GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.velocity = direction * weaponData.projectileSpeed;

        float finalDamage = playerStats != null ? playerStats.currentDamage : weaponData.damage;

        Bullet bullet = bulletObj.GetComponent<Bullet>();
        if (bullet != null)
            bullet.damage = finalDamage;

        Debug.Log($"{weaponData.itemName} �߻�! (���ݷ� {finalDamage})");
    }

    // ���� ����� �� ã��
    GameObject FindClosestEnemy()
    {
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
        GameObject nearestEnemy = null;
        float range = playerStats != null ? playerStats.currentRange : weaponData.detectionRange;
        float minDistance = range;

        foreach (GameObject monster in monsters)
        {
            float distance = Vector2.Distance(transform.position, monster.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestEnemy = monster;
            }
        }
        return nearestEnemy;
    }
}
