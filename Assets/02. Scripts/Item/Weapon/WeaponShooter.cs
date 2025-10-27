using UnityEngine;
using static UnityEngine.GraphicsBuffer;

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
        //player가 할당안됐으면 플레이어를 찾아서 자동으로 연결
        if (player == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;
            else
                Debug.Log("WeaponShooter Start - Player not found");
        }

        // 씬에서 실제 PlayerStats를 강제로 찾아서 연결
        if (player != null)
        {
            _playerStats = player.GetComponent<PlayerStats>();
            if (_playerStats == null)
                Debug.Log("WeaponShooter Start - PlayerStats not found on Player");
        }
        else
        {
            Debug.Log("WeaponShooter Start - Player reference is null");
        }
    }

    // 무기가 활성화될 때마다 PlayerStats를 재연결
    void OnEnable()
    {
        if (player == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;
        }

        if (player != null && _playerStats == null)
            _playerStats = player.GetComponent<PlayerStats>();
    }

    void Update()
    {
        if (player == null || weaponData == null) return;

        GameObject nearestEnemy = FindClosestEnemy();
        if (nearestEnemy == null) return;

        Vector2 direction = (nearestEnemy.transform.position - transform.position).normalized;
        transform.right = direction; //대신 스프라이트 파일을 항상 오른쪽이 올바른 방향이게 설정하자

        float attackSpeed = weaponData.fireRate * (_playerStats != null ? _playerStats.currentAttackSpeed : 1f); //공격속도도 곱연산 처리
        _cooldownTimer -= Time.deltaTime;

        if (_cooldownTimer <= 0f)
        {
            FireProjectile(direction);
            _cooldownTimer = 1f / attackSpeed; //한번 공격의 간격은 1/초당공격속도 이니까
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
        float finalDamage = baseDamage + weaponDamage; // 합산 구조로 변경

        Bullet bullet = bulletObj.GetComponent<Bullet>();
        if (bullet != null)
            bullet.damage = finalDamage;

        // 콘솔 로그 출력
        string weaponName = string.IsNullOrEmpty(weaponData.itemName) ? "Unnamed Weapon" : weaponData.itemName;
        Debug.Log
            (
            "[" + weaponName + "] 발사됨\n" +
            "기본 공격력: " + baseDamage + "\n" +
            "무기 공격력: " + weaponDamage + "\n" +
            "최종 공격력: " + finalDamage
             );
    }

    GameObject FindClosestEnemy()
    {
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
        GameObject nearestEnemy = null; //일단 null로 두고
        float range = weaponData.detectionRange * (_playerStats != null ? _playerStats.currentRange : 1f); //무기의 공격범위도 곱연산으로 두자
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
}
