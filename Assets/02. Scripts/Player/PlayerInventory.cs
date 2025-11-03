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

    [Header("보유 아이템 리스트 (복제본만 저장)")]
    public List<Item> ownedItems = new List<Item>();

    [SerializeField] private UI_PlayerStatus uiPlayerStatus;
    public Action OnInventoryChanged;

    // ---------------------------------------------------------
    // 경험치 및 보상
    // ---------------------------------------------------------
    public void AddReward(int coin, float exp)
    {
        gold += coin;
        AddExperience(exp);
        uiPlayerStatus?.UpdateCoinUI(gold);
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

        // 레벨업 시 체력 +2
        PlayerStats stats = GetComponent<PlayerStats>();
        if (stats != null)
        {
            stats.maxHp += 2f;
            stats.currentHp += 2f;

            // 체력이 최대치를 넘지 않게 보정
            if (stats.currentHp > stats.maxHp)
                stats.currentHp = stats.maxHp;

            Debug.Log($"[LevelUp] 체력 +2 적용됨 (현재 HP: {stats.currentHp}/{stats.maxHp})");
        }
    }


    public void ResetWaveLevelUpCount()
    {
        waveLevelUpCount = 0;
    }

    // ---------------------------------------------------------
    // 아이템 구매 (복제본만 저장, 원본 SO 절대 수정 안 함)
    // ---------------------------------------------------------
    public void BuyItem(Item item)
    {
        if (item == null || gold < item.price) return;

        WeaponData weapon = item as WeaponData;
        if (weapon != null)
        {
            WeaponSlotManager slotManager = FindObjectOfType<WeaponSlotManager>();
            if (slotManager == null)
            {
                Debug.LogWarning("[BuyItem] WeaponSlotManager를 찾을 수 없음");
                return;
            }

            // 동일 이름 무기 복제본 검색
            WeaponData sameWeapon = ownedItems
                .OfType<WeaponData>()
                .FirstOrDefault(w => w.itemName == weapon.itemName);

            // 동일 무기가 이미 있다면 -> 그 복제본을 티어업
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

                Debug.Log("[BuyItem] 동일 무기 발견 → 복제본 티어업 (" + sameWeapon.itemName + " Tier " + sameWeapon.tier + ")");
                return;
            }

            // 슬롯이 가득 찼으면 구매 불가
            int equippedCount = slotManager.GetEquippedWeaponCount();
            if (equippedCount >= 6)
            {
                Debug.Log("[BuyItem] 슬롯이 가득 차서 무기를 더 구매할 수 없음");
                return;
            }

            // 완전히 새로운 무기 -> 복제본 생성 후 사용
            WeaponData clone = ScriptableObject.Instantiate(weapon);
            clone.name = weapon.name + "_Clone";

            gold -= weapon.price;
            ownedItems.Add(clone);
            clone.ApplyEffect(gameObject);

            uiPlayerStatus?.UpdateCoinUI(gold);
            OnInventoryChanged?.Invoke();

            Debug.Log("[BuyItem] 복제본 무기 추가: " + clone.itemName);
            return;
        }

        // 패시브 아이템
        gold -= item.price;
        Item cloneItem = ScriptableObject.Instantiate(item);
        cloneItem.name = item.name + "_Clone";

        ownedItems.Add(cloneItem);
        cloneItem.ApplyEffect(gameObject);

        uiPlayerStatus?.UpdateCoinUI(gold);
        OnInventoryChanged?.Invoke();

        Debug.Log("[BuyItem] 패시브 아이템 추가 (복제본): " + cloneItem.itemName);
    }

    // ---------------------------------------------------------
    // 환불 (복제본 기준 동일하게 작동)
    // ---------------------------------------------------------
    public void RefundItem(Item item)
    {
        if (!ownedItems.Contains(item)) return;

        WeaponData weaponData = item as WeaponData;
        if (weaponData != null)
        {
            WeaponSlotManager slotManager = FindObjectOfType<WeaponSlotManager>();
            if (slotManager != null)
            {
                slotManager.RemoveSingleWeaponByName(weaponData.itemName);
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

        gold += item.price / 2;
        uiPlayerStatus?.UpdateCoinUI(gold);
        OnInventoryChanged?.Invoke();

        Debug.Log("[Refund] 복제본 환불 완료: " + item.itemName);
    }

    // ---------------------------------------------------------
    // 리스트 반환
    // ---------------------------------------------------------
    public List<WeaponData> GetOwnedWeapons()
    {
        return ownedItems.OfType<WeaponData>().ToList();
    }

    public List<PassiveItem> GetOwnedPassives()
    {
        return ownedItems.OfType<PassiveItem>().ToList();
    }
}
