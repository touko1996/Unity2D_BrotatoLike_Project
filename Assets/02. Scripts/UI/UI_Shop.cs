using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class UI_Shop : MonoBehaviour
{
    [Header("References")]
    public TMP_Text coinText;
    public Button rerollButton;
    public Button goButton;
    public GameObject[] itemSlots; // 4개 슬롯 오브젝트

    [Header("Shop Settings")]
    public int rerollCost = 3;
    public List<Item> allItems = new List<Item>(); // 등록된 모든 무기/패시브 리스트

    private PlayerInventory playerInventory;
    private UI_ShopManager shopManager;

    private void OnEnable()
    {
        if (playerInventory == null)
            playerInventory = FindObjectOfType<PlayerInventory>();

        coinText.text = "Coin: " + playerInventory.gold;

        // Refresh passive list
        UI_PassiveList passiveList = FindObjectOfType<UI_PassiveList>();
        if (passiveList != null)
            passiveList.RefreshList();

        // Refresh weapon list (important)
        UI_WeaponList weaponList = FindObjectOfType<UI_WeaponList>();
        if (weaponList != null)
            weaponList.RefreshList();
    }


    private void Start()
    {
        playerInventory = FindObjectOfType<PlayerInventory>();
        shopManager = FindObjectOfType<UI_ShopManager>();

        rerollButton.onClick.AddListener(RerollItems);
        goButton.onClick.AddListener(OnGoNextWave);

        RefreshShop();
    }

    private void RefreshShop()
    {
        coinText.text = $"Coin: {playerInventory.gold}";

        for (int i = 0; i < itemSlots.Length; i++)
        {
            var slot = itemSlots[i].GetComponent<ShopItemSlot>();
            Item randomItem = allItems[Random.Range(0, allItems.Count)];
            slot.SetItem(randomItem, playerInventory);
        }
    }

    private void RerollItems()
    {
        if (playerInventory.gold < rerollCost) return;

        playerInventory.gold -= rerollCost;
        RefreshShop();
    }

    private void OnGoNextWave()
    {
        // 상점 닫기
        gameObject.SetActive(false);

        // 다음 웨이브 시작 신호 보내기
        if (shopManager != null)
        {
            shopManager.OnGoNextWave();
        }
        else
        {
            Debug.LogWarning("UI_ShopManager not found! Cannot start next wave.");
        }
    }
}
