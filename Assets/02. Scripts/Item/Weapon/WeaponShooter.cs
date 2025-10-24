using UnityEngine;

// ������ ���� �� �߻� ����
public class WeaponShooter : MonoBehaviour
{
    [Header("����")]
    public Transform player;
    public WeaponData weaponData;

    private PlayerStats _playerStats;
    private float _cooldownTimer = 0f;
    void Start()
    {
        if (player == null)
            player = GameObject.FindWithTag("Player").transform;

        // ������ ���� PlayerStats�� ������ ã�Ƽ� ����
        _playerStats = FindObjectOfType<PlayerStats>();
    }
    // ���Ⱑ Ȱ��ȭ�� ������ PlayerStats�� �翬��
    void OnEnable()
    {
        if (player != null)
            _playerStats = player.GetComponent<PlayerStats>();
    }

    void Update()
    {
        if (player == null || weaponData == null) return;

        GameObject nearestEnemy = FindClosestEnemy();
        if (nearestEnemy == null) return;

        Vector2 direction = (nearestEnemy.transform.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        float attackSpeed = _playerStats != null ? _playerStats.currentAttackSpeed : weaponData.fireRate;
        _cooldownTimer -= Time.deltaTime;

        if (_cooldownTimer <= 0f)
        {
            FireProjectile(direction);
            _cooldownTimer = 1f / attackSpeed;
        }
    }

    void FireProjectile(Vector2 direction)
    {
        if (weaponData.projectilePrefab == null) return;

        GameObject bulletObj = Instantiate(weaponData.projectilePrefab, transform.position, Quaternion.identity);
        Rigidbody2D rb = bulletObj.GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.velocity = direction * weaponData.projectileSpeed;

        // �÷��̾� ���ݷ� + ���� ���ݷ� �ջ�
        float baseDamage = _playerStats != null ? _playerStats.currentDamage : 0f;
        float weaponDamage = weaponData.damage;
        float finalDamage = baseDamage + weaponDamage;

        Bullet bullet = bulletObj.GetComponent<Bullet>();
        if (bullet != null)
            bullet.damage = finalDamage;

        // �ܼ� �α� ���
        string weaponName = string.IsNullOrEmpty(weaponData.itemName) ? "Unnamed Weapon" : weaponData.itemName;
        Debug.Log(
            "[" + weaponName + "] �߻��\n" +
            "�⺻ ���ݷ�: " + baseDamage + "\n" +
            "���� ���ݷ�: " + weaponDamage + "\n" +
            "���� ���ݷ�: " + finalDamage
        );
    }

    GameObject FindClosestEnemy()
    {
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
        GameObject nearestEnemy = null;
        float range = _playerStats != null ? _playerStats.currentRange : weaponData.detectionRange;
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
