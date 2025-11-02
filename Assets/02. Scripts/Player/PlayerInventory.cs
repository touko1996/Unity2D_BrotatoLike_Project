using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [Header("Player EXP Settings")]
    public int level = 0;
    public float currentExp = 0f;
    public float expToNextLevel = 16f;

    [Header("Player Gold")]
    public int gold = 0;

    [Header("Wave LevelUp Counter")]
    public int waveLevelUpCount = 0;

    [Header("Inventory Lists")]
    public List<Item> ownedItems = new List<Item>(); // 무기 + 패시브 통합 리스트

    [SerializeField] private UI_PlayerStatus uiPlayerStatus;

    // -------------------------------
    // 인벤토리 변경 이벤트 (UI 갱신용)
    // -------------------------------
    public Action OnInventoryChanged;

    // -------------------------------
    // 경험치 & 골드 처리
    // -------------------------------
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
    }

    public void ResetWaveLevelUpCount()
    {
        waveLevelUpCount = 0;
    }

    // -------------------------------
    // 상점 관련 기능
    // -------------------------------
    public void BuyItem(Item item)
    {
        if (item == null || gold < item.price) return;

        gold -= item.price;
        ownedItems.Add(item);
        item.ApplyEffect(gameObject);

        uiPlayerStatus?.UpdateCoinUI(gold);

        // ★ 인벤토리 변경 이벤트 실행
        OnInventoryChanged?.Invoke();
    }

    public void RefundItem(Item item)
    {
        if (!ownedItems.Contains(item)) return;

        item.RefundAtStore(gameObject);
        gold += item.price / 2;
        ownedItems.Remove(item);

        uiPlayerStatus?.UpdateCoinUI(gold);

        // ★ 인벤토리 변경 이벤트 실행
        OnInventoryChanged?.Invoke();
    }

    public void MixItem(Item item)
    {
        if (!ownedItems.Contains(item)) return;
        item.MixAtStore(gameObject);

        // ★ 합성 후에도 UI 갱신 필요 시
        OnInventoryChanged?.Invoke();
    }

    // -------------------------------
    // UI 표시용 헬퍼
    // -------------------------------
    public List<WeaponData> GetOwnedWeapons()
    {
        List<WeaponData> weapons = new List<WeaponData>();
        foreach (var item in ownedItems)
        {
            if (item is WeaponData weapon)
                weapons.Add(weapon);
        }
        return weapons;
    }

    public List<PassiveItem> GetOwnedPassives()
    {
        List<PassiveItem> passives = new List<PassiveItem>();
        foreach (var item in ownedItems)
        {
            if (item is PassiveItem passive)
                passives.Add(passive);
        }
        return passives;
    }
}
