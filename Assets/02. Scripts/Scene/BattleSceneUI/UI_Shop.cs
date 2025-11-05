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

    private int currentWave = 1; // UI_GameWave로부터 전달받는 값

    private void OnEnable()
    {
        if (playerInventory == null)
            playerInventory = FindObjectOfType<PlayerInventory>();

        if (playerInventory != null)
            playerInventory.OnInventoryChanged += UpdateCoinText;

        UpdateCoinText();

        // Refresh 리스트
        FindObjectOfType<UI_PassiveList>()?.RefreshList();
        FindObjectOfType<UI_WeaponList>()?.RefreshList();
    }

    private void OnDisable()
    {
        if (playerInventory != null)
            playerInventory.OnInventoryChanged -= UpdateCoinText;
    }

    private void Start()
    {
        playerInventory = FindObjectOfType<PlayerInventory>();
        shopManager = FindObjectOfType<UI_ShopManager>();

        rerollButton.onClick.AddListener(RerollItems);
        goButton.onClick.AddListener(OnGoNextWave);
    }

    // UI_ShopManager에서 호출됨
    public void OpenShop(int waveNumber)
    {
        currentWave = waveNumber;
        Debug.Log("[Shop] Opened for Wave " + currentWave);
        gameObject.SetActive(true);
        RefreshShop();
    }

    private void RefreshShop()
    {
        UpdateCoinText();

        // 웨이브당 10% 상승
        float priceMultiplier = 1f + 0.1f * (currentWave - 1);
        Debug.Log($"[Shop] Wave {currentWave} | Price Multiplier: {priceMultiplier}");

        for (int i = 0; i < itemSlots.Length; i++)
        {
            var slot = itemSlots[i].GetComponent<ShopItemSlot>();
            Item randomItem = allItems[Random.Range(0, allItems.Count)];

            // 복제본 생성 후 가격 조정
            Item itemClone = ScriptableObject.Instantiate(randomItem);
            itemClone.price = Mathf.RoundToInt(randomItem.price * priceMultiplier);

            slot.SetItem(itemClone, playerInventory);
        }
    }

    private void UpdateCoinText()
    {
        if (playerInventory != null && coinText != null)
            coinText.text = $"Coin: {playerInventory.gold}";
    }

    private void RerollItems()
    {
        if (playerInventory.gold < rerollCost) return;

        playerInventory.gold -= rerollCost;
        UpdateCoinText();
        RefreshShop();
    }

    private void OnGoNextWave()
    {
        gameObject.SetActive(false);

        if (shopManager != null)
            shopManager.OnGoNextWave();
        else
            Debug.LogWarning("UI_ShopManager not found! Cannot start next wave.");
    }
}
