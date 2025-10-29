using UnityEngine;

public class WeaponShooter : MonoBehaviour
{
    [Header("")]
    public Transform player;
    public WeaponData weaponData;

    private PlayerStats _playerStats;
    private float _cooldownTimer = 0f;

    private void Start()
    {
        if (player == null)
            player = GameObject.FindWithTag("Player")?.transform;

        _playerStats = FindAnyObjectByType<PlayerStats>();
    }

    private void Update()
    {
        if (weaponData == null) return;

        GameObject nearestEnemy = FindClosestEnemy();
        if (nearestEnemy == null) return;

        Vector2 direction = (nearestEnemy.transform.position - transform.position).normalized;
        transform.right = direction;

        float attackSpeed = weaponData.fireRate * (_playerStats != null ? _playerStats.currentAttackSpeed : 1f);
        _cooldownTimer -= Time.deltaTime;

        if (_cooldownTimer <= 0f)
        {
            FireProjectile(direction);
            _cooldownTimer = 1f / attackSpeed;
        }
    }

    private void FireProjectile(Vector2 direction)
    {
        if (weaponData.projectilePrefab == null) return;

        GameObject bulletObj = Instantiate(weaponData.projectilePrefab, transform.position, Quaternion.identity);
        Rigidbody2D rb = bulletObj.GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.velocity = direction * weaponData.projectileSpeed;

        Bullet bullet = bulletObj.GetComponent<Bullet>();
        if (bullet != null)
        {
            float finalDamage = weaponData.damage * (_playerStats != null ? _playerStats.currentDamage : 1f);
            bullet.damage = finalDamage;
        }
    }

    private GameObject FindClosestEnemy()
    {
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
        GameObject nearestEnemy = null;

        float playerRangeBonus = (_playerStats != null ? _playerStats.currentRange : 0f);
        float range = weaponData.detectionRange + playerRangeBonus; // sum, not multiply
        float minDistance = range;

        foreach (GameObject monster in monsters)
        {
            if (monster == null) continue;

            float distance = Vector2.Distance(transform.position, monster.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestEnemy = monster;
            }
        }

        return nearestEnemy;
    }

    private void OnDrawGizmosSelected()
    {
        if (weaponData == null) return;

        float playerRangeBonus = (_playerStats != null ? _playerStats.currentRange : 0f);
        float range = weaponData.detectionRange + playerRangeBonus;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
