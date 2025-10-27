using UnityEngine;
using static UnityEngine.GraphicsBuffer;

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
        //player�� �Ҵ�ȵ����� �÷��̾ ã�Ƽ� �ڵ����� ����
        if (player == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;
            else
                Debug.Log("WeaponShooter Start - Player not found");
        }

        // ������ ���� PlayerStats�� ������ ã�Ƽ� ����
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

    // ���Ⱑ Ȱ��ȭ�� ������ PlayerStats�� �翬��
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
        transform.right = direction; //��� ��������Ʈ ������ �׻� �������� �ùٸ� �����̰� ��������

        float attackSpeed = weaponData.fireRate * (_playerStats != null ? _playerStats.currentAttackSpeed : 1f); //���ݼӵ��� ������ ó��
        _cooldownTimer -= Time.deltaTime;

        if (_cooldownTimer <= 0f)
        {
            FireProjectile(direction);
            _cooldownTimer = 1f / attackSpeed; //�ѹ� ������ ������ 1/�ʴ���ݼӵ� �̴ϱ�
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
        float finalDamage = baseDamage + weaponDamage; // �ջ� ������ ����

        Bullet bullet = bulletObj.GetComponent<Bullet>();
        if (bullet != null)
            bullet.damage = finalDamage;

        // �ܼ� �α� ���
        string weaponName = string.IsNullOrEmpty(weaponData.itemName) ? "Unnamed Weapon" : weaponData.itemName;
        Debug.Log
            (
            "[" + weaponName + "] �߻��\n" +
            "�⺻ ���ݷ�: " + baseDamage + "\n" +
            "���� ���ݷ�: " + weaponDamage + "\n" +
            "���� ���ݷ�: " + finalDamage
             );
    }

    GameObject FindClosestEnemy()
    {
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
        GameObject nearestEnemy = null; //�ϴ� null�� �ΰ�
        float range = weaponData.detectionRange * (_playerStats != null ? _playerStats.currentRange : 1f); //������ ���ݹ����� ���������� ����
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
