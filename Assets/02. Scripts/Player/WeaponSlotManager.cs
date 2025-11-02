using System.Collections.Generic;
using UnityEngine;

public class WeaponSlotManager : MonoBehaviour
{
    [Header("Weapon Slot Settings")]
    [SerializeField] private float slotDistance = 1.5f;
    [SerializeField] private Transform slotParent;

    [Header("Starting Weapon")]
    [SerializeField] private WeaponData startingWeapon;

    private List<Transform> _weaponSlots = new List<Transform>();
    private PlayerInventory inventory;

    // ★ 플레이어 위치 추적용
    private Transform playerTransform;

    private void Awake()
    {
        int slotCount = 6;
        for (int i = 0; i < slotCount; i++)
        {
            GameObject slot = new GameObject("WeaponSlot_" + (i + 1));
            slot.transform.SetParent(slotParent != null ? slotParent : transform, false);
            _weaponSlots.Add(slot.transform);
        }
    }

    private void Start()
    {
        inventory = FindObjectOfType<PlayerInventory>();
        ArrangeSlots();

        // ★ 플레이어 Transform 캐싱
        playerTransform = GameObject.FindWithTag("Player")?.transform;

        // ★ 시작 무기 장착
        if (startingWeapon != null)
        {
            EquipWeapon(startingWeapon);
        }
    }

    private void Update()
    {
        // ★ 플레이어 위치를 따라가게 (숨쉬기모션은 제외)
        if (playerTransform != null)
        {
            transform.position = playerTransform.position;
        }
    }

    private void ArrangeSlots()
    {
        for (int i = 0; i < _weaponSlots.Count; i++)
        {
            float angle = (360f / _weaponSlots.Count) * i * Mathf.Deg2Rad;
            Vector3 pos = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f) * slotDistance;
            _weaponSlots[i].localPosition = pos;
        }
    }

    public Transform GetEmptySlot()
    {
        foreach (Transform slot in _weaponSlots)
        {
            if (slot.childCount == 0)
                return slot;
        }
        return null;
    }

    public void EquipWeapon(WeaponData weaponData)
    {
        if (weaponData == null)
            return;

        Transform emptySlot = GetEmptySlot();
        if (emptySlot == null)
        {
            Debug.LogWarning("No empty weapon slot available!");
            return;
        }

        GameObject weaponObj = new GameObject(weaponData.itemName);
        weaponObj.transform.SetParent(emptySlot, false);

        SpriteRenderer sr = weaponObj.AddComponent<SpriteRenderer>();
        if (weaponData.itemSprite != null)
            sr.sprite = weaponData.itemSprite;

        WeaponShooter shooter = weaponObj.AddComponent<WeaponShooter>();
        shooter.weaponData = weaponData;

        if (inventory != null && !inventory.ownedItems.Contains(weaponData))
        {
            inventory.ownedItems.Add(weaponData);
            inventory.OnInventoryChanged?.Invoke();
        }

        Debug.Log($"Equipped weapon: {weaponData.itemName}");
    }
}
