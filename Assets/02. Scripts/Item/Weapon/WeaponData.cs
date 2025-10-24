using System.Collections.Generic;
using UnityEngine;

// 무기 정보 (ScriptableObject)
[CreateAssetMenu(fileName = "NewWeaponData", menuName = "Items/WeaponData")]
public class WeaponData : Item
{
    [Header("무기 정보")]
    public GameObject projectilePrefab;
    public float damage = 10f;
    public float fireRate = 1f;
    public float projectileSpeed = 10f;
    public float detectionRange = 8f;

    private static List<WeaponShooter> equippedWeapons = new List<WeaponShooter>();

    public override void ApplyEffect(GameObject player)
    {
        WeaponSlotManager slotManager = player.GetComponent<WeaponSlotManager>();
        if (slotManager == null)
        {
            Debug.LogError("WeaponSlotManager가 없습니다. Player에 추가하세요.");
            return;
        }

        Transform emptySlot = slotManager.GetEmptySlot();
        if (emptySlot == null)
        {
            Debug.LogWarning("모든 무기 슬롯이 가득 찼습니다.");
            return;
        }

        // 중복 장착 방지
        foreach (WeaponShooter shooter in equippedWeapons)
        {
            if (shooter != null && shooter.weaponData == this)
            {
                Debug.LogWarning($"{itemName}은(는) 이미 장착되어 있습니다.");
                return;
            }
        }

        // 무기 생성 및 설정
        GameObject weaponObj = new GameObject(itemName);
        weaponObj.transform.SetParent(emptySlot);
        weaponObj.transform.localPosition = Vector3.zero;

        WeaponShooter newShooter = weaponObj.AddComponent<WeaponShooter>();
        newShooter.player = player.transform;
        newShooter.weaponData = this;

        SpriteRenderer spriteRenderer = weaponObj.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = itemSprite;
        spriteRenderer.sortingOrder = 5;

        equippedWeapons.Add(newShooter);
        Debug.Log($"{itemName} 장착 완료 ({slotManager.GetCurrentWeaponCount()}/6)");
    }
}
