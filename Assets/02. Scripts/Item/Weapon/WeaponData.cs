using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Items/Weapon")]
public class WeaponData : Item
{
    [Header("Weapon Stats")]
    public float damage = 10f;
    public float fireRate = 1f;
    public float detectionRange = 5f;
    public float projectileSpeed = 10f;
    public int tier = 1;
    public GameObject projectilePrefab;

    public override void ApplyEffect(GameObject player)
    {
        WeaponSlotManager slotManager = Object.FindObjectOfType<WeaponSlotManager>();
        if (slotManager == null) return;

        Transform emptySlot = slotManager.GetEmptySlot();
        if (emptySlot == null)
        {
            Debug.LogWarning("[WeaponData] ½½·ÔÀÌ °¡µæ Âü");
            return;
        }

        GameObject weaponObj = new GameObject(itemName);
        weaponObj.transform.SetParent(emptySlot, false);

        SpriteRenderer sr = weaponObj.AddComponent<SpriteRenderer>();
        if (itemSprite != null)
            sr.sprite = itemSprite;

        WeaponShooter shooter = weaponObj.AddComponent<WeaponShooter>();
        shooter.weaponData = this;

        Debug.Log("[WeaponData] ÀåÂø ¿Ï·á: " + itemName);
    }

    // Mix ½Ã ½ºÅÈ ¾÷
    public void MixUpgrade()
    {
        tier++;
        damage *= 1.3f;
        fireRate *= 1.1f;
        detectionRange += 1f;
        Debug.Log("[MixUpgrade] " + itemName + "ÀÌ Tier " + tier + "·Î °­È­µÊ");
    }
}
