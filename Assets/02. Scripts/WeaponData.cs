using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWeaponData", menuName = "Items/WeaponData")]
public class WeaponData : Item
{
    [Header("무기 정보")]
    public GameObject projectilePrefab;
    public float damage = 10f;
    public float fireRate = 1f;
    public float projectileSpeed = 10f;
    public float detectionRange = 8f;

    private static List<WeaponOrbit> equippedWeapons = new List<WeaponOrbit>();

    public override void ApplyEffect(GameObject player)
    {
        if (equippedWeapons.Count >= 6)
        {
            Debug.LogWarning("최대 6개까지 장착 가능합니다!");
            return;
        }

        foreach (var w in equippedWeapons)
        {
            if (w != null && w.weaponData == this)
            {
                Debug.LogWarning($"{itemName}은(는) 이미 장착되어 있습니다.");
                return;
            }
        }

        GameObject weaponObj = new GameObject(itemName);
        weaponObj.transform.SetParent(player.transform);

        WeaponOrbit weaponOrbit = weaponObj.AddComponent<WeaponOrbit>();
        weaponOrbit.player = player.transform;
        weaponOrbit.weaponData = this;

        equippedWeapons.Add(weaponOrbit);
        UpdateWeaponOffsets(player.transform);

        Debug.Log($"{itemName} 장착 완료! (현재 {equippedWeapons.Count}/6)");
    }

    private void UpdateWeaponOffsets(Transform player)
    {
        int count = equippedWeapons.Count;
        float angleStep = 360f / count;

        for (int i = 0; i < count; i++)
        {
            if (equippedWeapons[i] == null) continue;

            float angle = angleStep * i * Mathf.Deg2Rad;
            float radius = 1.5f;
            Vector3 offset = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius;

            equippedWeapons[i].offset = offset;
        }
    }
}
