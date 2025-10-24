using UnityEngine;

// 무기의 조준 및 발사 로직
public class WeaponShooter : MonoBehaviour
{
    [Header("참조")]
    public Transform player;
    public WeaponData weaponData;

    private PlayerStats _playerStats;
    private float _cooldownTimer = 0f;
    void Start()
    {
        if (player == null)
            player = GameObject.FindWithTag("Player").transform;

        // 씬에서 실제 PlayerStats를 강제로 찾아서 연결
        _playerStats = FindObjectOfType<PlayerStats>();
    }
    // 무기가 활성화될 때마다 PlayerStats를 재연결
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

        // 플레이어 공격력 + 무기 공격력 합산
        float baseDamage = _playerStats != null ? _playerStats.currentDamage : 0f;
        float weaponDamage = weaponData.damage;
        float finalDamage = baseDamage + weaponDamage;

        Bullet bullet = bulletObj.GetComponent<Bullet>();
        if (bullet != null)
            bullet.damage = finalDamage;

        // 콘솔 로그 출력
        string weaponName = string.IsNullOrEmpty(weaponData.itemName) ? "Unnamed Weapon" : weaponData.itemName;
        Debug.Log(
            "[" + weaponName + "] 발사됨\n" +
            "기본 공격력: " + baseDamage + "\n" +
            "무기 공격력: " + weaponDamage + "\n" +
            "최종 공격력: " + finalDamage
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
