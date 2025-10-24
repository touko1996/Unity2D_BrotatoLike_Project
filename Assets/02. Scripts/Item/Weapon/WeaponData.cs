using System.Collections.Generic;
using UnityEngine;

// ���� ���� (ScriptableObject)
[CreateAssetMenu(fileName = "NewWeaponData", menuName = "Items/WeaponData")]
public class WeaponData : Item
{
    [Header("���� ����")]
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
            Debug.LogError("WeaponSlotManager�� �����ϴ�. Player�� �߰��ϼ���.");
            return;
        }

        Transform emptySlot = slotManager.GetEmptySlot();
        if (emptySlot == null)
        {
            Debug.LogWarning("��� ���� ������ ���� á���ϴ�.");
            return;
        }

        // �ߺ� ���� ����
        foreach (WeaponShooter shooter in equippedWeapons)
        {
            if (shooter != null && shooter.weaponData == this)
            {
                Debug.LogWarning($"{itemName}��(��) �̹� �����Ǿ� �ֽ��ϴ�.");
                return;
            }
        }

        // ���� ���� �� ����
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
        Debug.Log($"{itemName} ���� �Ϸ� ({slotManager.GetCurrentWeaponCount()}/6)");
    }
}
