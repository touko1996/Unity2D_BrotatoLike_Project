using UnityEngine;

public class WeaponShooter : MonoBehaviour
{
    [Header("Weapon Info")]
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

        // 공격속도 보정 계산 (로그 기반 완화)
        float baseFireRate = weaponData.fireRate;
        float rawAttackSpeed = (_playerStats != null ? _playerStats.currentAttackSpeed : 1f);

        // 공격속도가 커질수록 효율 감소 (로그 보정)
        float effectiveAttackSpeed = 1f + Mathf.Log10(rawAttackSpeed); // 예: 2.0 → 1.3, 3.0 → 1.48
        float attackSpeed = baseFireRate * effectiveAttackSpeed;

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

        // 총알 생성
        GameObject bulletObj = Instantiate(weaponData.projectilePrefab, transform.position, Quaternion.identity);

        // Bullet 스크립트 가져오기
        Bullet bullet = bulletObj.GetComponent<Bullet>();
        if (bullet != null)
        {
            // 데미지 합연산 방식 유지
            float finalDamage = weaponData.damage + (_playerStats.currentDamage * 0.5f);
            bullet.damage = finalDamage;

            // 방향 설정 (이동 + 회전 동시 처리)
            bullet.SetDirection(direction);
        }
        else
        {
            // Bullet 스크립트가 없다면 기존 방식으로 속도만 적용
            Rigidbody2D rb = bulletObj.GetComponent<Rigidbody2D>();
            if (rb != null)
                rb.velocity = direction * weaponData.projectileSpeed;
        }

        // 발사 사운드
        if (weaponData.fireSFX != null)
            AudioManager.Instance.PlaySFX(weaponData.fireSFX, 0.9f);
        else
            AudioManager.Instance.PlayGunSFX(); // 기본 발사음
    }

    private GameObject FindClosestEnemy()
    {
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
        GameObject nearestEnemy = null;

        float playerRangeBonus = (_playerStats != null ? _playerStats.currentRange : 0f);
        float range = weaponData.detectionRange + playerRangeBonus;
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
