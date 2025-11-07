using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Items/Weapon")]
public class WeaponData : Item
{
    [Header("무기 스탯")]
    public float damage = 10f;
    public float fireRate = 1f;
    public float detectionRange = 5f;
    public float projectileSpeed = 10f;
    public int tier = 1;
    public GameObject projectilePrefab;

    [Header("사운드 설정")]
    public AudioClip fireSFX; // 무기 발사음

    public override void ApplyEffect(GameObject player)
    {
        WeaponSlotManager slotManager = Object.FindObjectOfType<WeaponSlotManager>();
        if (slotManager == null) return;

        Transform emptySlot = slotManager.GetEmptySlot();
        if (emptySlot == null) return;

        GameObject weaponObj = new GameObject(itemName);
        weaponObj.transform.SetParent(emptySlot, false);

        SpriteRenderer sr = weaponObj.AddComponent<SpriteRenderer>();
        if (itemSprite != null) sr.sprite = itemSprite;

        WeaponShooter shooter = weaponObj.AddComponent<WeaponShooter>();
        shooter.weaponData = this;
    }

    // Mix 시 스탯 업
    public void MixUpgrade()
    {
        tier++;
        damage *= 1.3f;
        fireRate *= 1.2f;
        detectionRange += 1f;
    }
}
