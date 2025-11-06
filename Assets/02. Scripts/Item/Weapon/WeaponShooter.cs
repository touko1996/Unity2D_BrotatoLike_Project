using UnityEngine;

/// <summary>
/// [WeaponShooter]
/// --------------------------------------------------------------------
/// WeaponData의 정보를 기반으로 발사체를 생성하고, 가장 가까운 적에게 자동 조준 발사.
/// - 공격속도는 PlayerStats.currentAttackSpeed를 기반으로 계산.
/// - 발사체 속도는 WeaponData.projectileSpeed를 사용.
/// --------------------------------------------------------------------
/// </summary>
public class WeaponShooter : MonoBehaviour
{
    [Header("무기 참조")]
    public Transform player;
    public WeaponData weaponData;

    private PlayerStats playerStats;
    private float cooldownTimer = 0f;

    private void Start()
    {
        // 플레이어 및 스탯 참조 자동 할당
        if (player == null)
            player = GameObject.FindWithTag("Player")?.transform;

        playerStats = FindAnyObjectByType<PlayerStats>();
    }

    private void Update()
    {
        if (weaponData == null) return;

        // 가장 가까운 적 탐색
        GameObject nearestEnemy = FindClosestEnemy();
        if (nearestEnemy == null) return;

        // 적 방향 계산 및 총구 회전
        Vector2 direction = (nearestEnemy.transform.position - transform.position).normalized;
        transform.right = direction; // transform.right = 무기의 발사 방향

        // 공격속도 계산 (로그 기반 완화)
        float baseFireRate = weaponData.fireRate;
        float rawAttackSpeed = (playerStats != null ? playerStats.currentAttackSpeed : 1f);
        float effectiveAttackSpeed = 1f + Mathf.Log10(rawAttackSpeed); // 공격속도 증가 완화 곡선 적용
        float attackSpeed = baseFireRate * effectiveAttackSpeed;

        cooldownTimer -= Time.deltaTime;

        // 쿨타임이 끝났을 때 발사
        if (cooldownTimer <= 0f)
        {
            FireProjectile(direction);
            cooldownTimer = 1f / attackSpeed;
        }
    }

    /// <summary>
    /// 발사체 생성 및 초기화
    /// </summary>
    private void FireProjectile(Vector2 direction)
    {
        if (weaponData.projectilePrefab == null) return;

        // 발사체 생성
        GameObject bulletObj = Instantiate(weaponData.projectilePrefab, transform.position, Quaternion.identity);

        // Bullet 스크립트가 있다면 세부 데이터 전달
        Bullet bullet = bulletObj.GetComponent<Bullet>();
        if (bullet != null)
        {
            // 데미지 계산 (무기 데미지 + 플레이어 보정)
            float finalDamage = weaponData.damage + playerStats.currentDamage;
            bullet.damage = finalDamage;

            // 방향 및 속도 설정
            bullet.SetDirection(direction);
            bullet.SetSpeed(weaponData.projectileSpeed); // WeaponData의 발사체 속도 전달
        }
        else
        {
            // Bullet 스크립트가 없다면 Rigidbody로 직접 속도 적용
            Rigidbody2D rb = bulletObj.GetComponent<Rigidbody2D>();
            if (rb != null)
                rb.velocity = direction * weaponData.projectileSpeed;
        }

        // 발사 사운드 재생
        if (weaponData.fireSFX != null)
            AudioManager.Instance.PlaySFX(weaponData.fireSFX, 0.9f);
        else
            AudioManager.Instance.PlayGunSFX();
    }

    /// <summary>
    /// 가장 가까운 몬스터 탐색
    /// </summary>
    private GameObject FindClosestEnemy()
    {
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
        GameObject nearestEnemy = null;

        float playerRangeBonus = (playerStats != null ? playerStats.currentRange : 0f);
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

    /// <summary>
    /// 사정거리 시각화 (Scene 뷰에서만 보임)
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        if (weaponData == null) return;

        float playerRangeBonus = (playerStats != null ? playerStats.currentRange : 0f);
        float range = weaponData.detectionRange + playerRangeBonus;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
