using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [Header("경험치 및 골드 관련")]
    public int level = 0;
    public float currentExp = 0f;
    public float expToNextLevel = 16f;
    public int gold = 0;
    public int waveLevelUpCount = 0;

    [Header("보유 아이템 리스트")]
    public List<Item> ownedItems = new List<Item>(); // 복제본만 저장, 원본 SO는 절대 수정하지 않음

    [SerializeField] private UI_PlayerStatus uiPlayerStatus;
    public Action OnInventoryChanged;

    public void AddReward(int coin, float exp)
    {
        gold += coin;
        AddExperience(exp);
        uiPlayerStatus?.UpdateCoinUI(gold);
        OnInventoryChanged?.Invoke();
    }

    public void AddExperience(float exp)
    {
        currentExp += exp;
        if (currentExp >= expToNextLevel)
            LevelUp();

        uiPlayerStatus?.UpdateExpUI(currentExp, expToNextLevel);
    }

    private void LevelUp()
    {
        level++;
        currentExp = 0f;
        expToNextLevel = Mathf.Pow(level + 4, 2);
        waveLevelUpCount++;

        PlayerStats stats = GetComponent<PlayerStats>();
        if (stats != null)
        {
            stats.maxHp += 5f;
            stats.currentHp += 5f;

            if (stats.currentHp > stats.maxHp)
                stats.currentHp = stats.maxHp;
        }

        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayLevelUpSFX();
    }

    public void ResetWaveLevelUpCount()
    {
        waveLevelUpCount = 0;
    }

    public void BuyItem(Item item)
    {
        if (item == null || gold < item.price) return;

        WeaponData weapon = item as WeaponData;
        if (weapon != null)
        {
            WeaponSlotManager slotManager = FindObjectOfType<WeaponSlotManager>();
            if (slotManager == null) return;

            WeaponData sameWeapon = ownedItems
                .OfType<WeaponData>()
                .FirstOrDefault(w => w.itemName == weapon.itemName);

            if (sameWeapon != null)
            {
                sameWeapon.tier++;
                sameWeapon.damage *= 1.2f;
                sameWeapon.fireRate *= 1.1f;
                sameWeapon.detectionRange += 1f;
                sameWeapon.projectileSpeed *= 1.05f;

                gold -= weapon.price;
                uiPlayerStatus?.UpdateCoinUI(gold);
                OnInventoryChanged?.Invoke();
                return;
            }

            int equippedCount = slotManager.GetEquippedWeaponCount();
            if (equippedCount >= 6) return;

            WeaponData clone = ScriptableObject.Instantiate(weapon);
            clone.name = weapon.name + "_Clone";

            gold -= weapon.price;
            ownedItems.Add(clone);
            clone.ApplyEffect(gameObject);

            uiPlayerStatus?.UpdateCoinUI(gold);
            OnInventoryChanged?.Invoke();
            return;
        }

        gold -= item.price;
        Item cloneItem = ScriptableObject.Instantiate(item);
        cloneItem.name = item.name + "_Clone";

        ownedItems.Add(cloneItem);
        cloneItem.ApplyEffect(gameObject);

        uiPlayerStatus?.UpdateCoinUI(gold);
        OnInventoryChanged?.Invoke();
    }

    public void RefundItem(Item item)
    {
        if (!ownedItems.Contains(item)) return;

        WeaponData weaponData = item as WeaponData;
        if (weaponData != null)
        {
            WeaponSlotManager slotManager = FindObjectOfType<WeaponSlotManager>();
            if (slotManager != null)
            {
                slotManager.RemoveWeaponByName(weaponData.itemName);
            }
        }

        for (int i = 0; i < ownedItems.Count; i++)
        {
            if (ownedItems[i].itemName == item.itemName)
            {
                ownedItems.RemoveAt(i);
                break;
            }
        }

        int refundAmount = Mathf.RoundToInt(item.price * 0.5f);
        gold += refundAmount;

        uiPlayerStatus?.UpdateCoinUI(gold);
        OnInventoryChanged?.Invoke();
    }

    public List<WeaponData> GetOwnedWeapons()
    {
        return ownedItems.OfType<WeaponData>().ToList();
    }

    public List<PassiveItem> GetOwnedPassives()
    {
        return ownedItems.OfType<PassiveItem>().ToList();
    }
}
