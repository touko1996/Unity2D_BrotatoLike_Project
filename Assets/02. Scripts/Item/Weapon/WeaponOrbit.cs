using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponOrbit : MonoBehaviour
{
    [Header("참조")]
    public Transform player;
    public WeaponData weaponData;

    [Header("위치 설정")]
    public Vector3 offset = new Vector3(1.5f, 0, 0);

    private float cooldown = 0f;

    void Update()
    {
        if (player == null || weaponData == null) return;

        transform.position = player.position + offset;

        GameObject nearest = FindNearestMonster();
        if (nearest == null) return;

        Vector2 dir = (nearest.transform.position - transform.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        cooldown -= Time.deltaTime;
        if (cooldown <= 0f)
        {
            Shoot(dir);
            cooldown = weaponData.fireRate;
        }
    }

    void Shoot(Vector2 dir)
    {
        if (weaponData.projectilePrefab == null) return;

        GameObject bullet = Instantiate(weaponData.projectilePrefab, transform.position, Quaternion.identity);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.velocity = dir * weaponData.projectileSpeed;

        Bullet b = bullet.GetComponent<Bullet>();
        if (b != null)
            b.damage = weaponData.damage;

        Debug.Log($"{weaponData.itemName} 발사!");
    }

    GameObject FindNearestMonster()
    {
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
        GameObject nearest = null;
        float minDist = weaponData.detectionRange;

        foreach (GameObject m in monsters)
        {
            float dist = Vector2.Distance(transform.position, m.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                nearest = m;
            }
        }
        return nearest;
    }
}