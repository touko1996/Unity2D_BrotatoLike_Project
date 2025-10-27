using UnityEngine;

// ���� ������ ScriptableObject
[CreateAssetMenu(fileName = "NewWeapon", menuName = "Items/Weapon")]
public class WeaponData : Item
{
    [Header("���� ����")]
    public float damage = 10f;          // ���� ���ݷ�
    public float fireRate = 1f;         // �ʴ� �߻� �ӵ�
    public float detectionRange = 5f;   // �� Ž�� ����
    public float projectileSpeed = 10f; // ����ü �ӵ�
    public GameObject projectilePrefab; // ����ü ������

    public override void ApplyEffect(GameObject player)
    {
        // WeaponSlotManager�� ���� ����ִ� ���� ã��
        WeaponSlotManager slotManager = FindObjectOfType<WeaponSlotManager>();
        Transform emptySlot = slotManager != null ? slotManager.GetEmptySlot() : null;

        if (emptySlot == null)
        {
            Debug.LogWarning("WeaponData ApplyEffect - No empty slot available for " + itemName);
            return;
        }

        // ���� ������Ʈ ����
        GameObject weaponObj = new GameObject(itemName);
        weaponObj.transform.SetParent(emptySlot, false);

        // ��������Ʈ ������ �߰� �� itemSprite ����
        SpriteRenderer sr = weaponObj.GetComponent<SpriteRenderer>();
        if (sr == null)
            sr = weaponObj.AddComponent<SpriteRenderer>();

        if (itemSprite != null)
            sr.sprite = itemSprite;
        else
            Debug.LogWarning("WeaponData ApplyEffect - itemSprite not assigned for " + itemName);

        // WeaponShooter ������Ʈ �߰� �� ����
        WeaponShooter newShooter = weaponObj.AddComponent<WeaponShooter>();
        newShooter.weaponData = this;

        Debug.Log("WeaponData ApplyEffect - Weapon applied: " + itemName);
    }

    public override void RemoveEffect(GameObject player)
    {
        Debug.Log("WeaponData RemoveEffect - " + itemName + " removed from player.");
    }
}
