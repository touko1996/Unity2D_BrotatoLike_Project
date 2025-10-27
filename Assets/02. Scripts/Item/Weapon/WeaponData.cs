using UnityEngine;

// 무기 데이터 ScriptableObject
[CreateAssetMenu(fileName = "NewWeapon", menuName = "Items/Weapon")]
public class WeaponData : Item
{
    [Header("무기 정보")]
    public float damage = 10f;          // 무기 공격력
    public float fireRate = 1f;         // 초당 발사 속도
    public float detectionRange = 5f;   // 적 탐지 범위
    public float projectileSpeed = 10f; // 투사체 속도
    public GameObject projectilePrefab; // 투사체 프리팹

    public override void ApplyEffect(GameObject player)
    {
        // WeaponSlotManager를 통해 비어있는 슬롯 찾기
        WeaponSlotManager slotManager = FindObjectOfType<WeaponSlotManager>();
        Transform emptySlot = slotManager != null ? slotManager.GetEmptySlot() : null;

        if (emptySlot == null)
        {
            Debug.LogWarning("WeaponData ApplyEffect - No empty slot available for " + itemName);
            return;
        }

        // 무기 오브젝트 생성
        GameObject weaponObj = new GameObject(itemName);
        weaponObj.transform.SetParent(emptySlot, false);

        // 스프라이트 렌더러 추가 및 itemSprite 적용
        SpriteRenderer sr = weaponObj.GetComponent<SpriteRenderer>();
        if (sr == null)
            sr = weaponObj.AddComponent<SpriteRenderer>();

        if (itemSprite != null)
            sr.sprite = itemSprite;
        else
            Debug.LogWarning("WeaponData ApplyEffect - itemSprite not assigned for " + itemName);

        // WeaponShooter 컴포넌트 추가 및 설정
        WeaponShooter newShooter = weaponObj.AddComponent<WeaponShooter>();
        newShooter.weaponData = this;

        Debug.Log("WeaponData ApplyEffect - Weapon applied: " + itemName);
    }

    public override void RemoveEffect(GameObject player)
    {
        Debug.Log("WeaponData RemoveEffect - " + itemName + " removed from player.");
    }
}
