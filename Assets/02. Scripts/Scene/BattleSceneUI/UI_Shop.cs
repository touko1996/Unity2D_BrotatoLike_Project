using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

/// <summary>
/// [UI_Shop]
/// ------------------------------------------------------------
/// 웨이브 종료 후 등장하는 상점 UI를 관리한다.
/// - 무작위 아이템을 슬롯에 배치
/// - 리롤 및 구매 버튼 처리
/// - 코인 표시 갱신
/// - 다음 웨이브로 진행 버튼 제어
/// ------------------------------------------------------------
/// </summary>
public class UI_Shop : MonoBehaviour
{
    [Header("UI 참조")]
    [SerializeField] private TMP_Text coinText;         // 코인 표시 텍스트
    [SerializeField] private Button rerollButton;       // 리롤 버튼
    [SerializeField] private Button nextWaveButton;     // 다음 웨이브 버튼
    [SerializeField] private GameObject[] itemSlots;    // 상점 아이템 슬롯 (4개 예상)

    [Header("상점 설정")]
    [SerializeField] private int rerollCost = 3;        // 리롤 가격
    [SerializeField] private List<Item> allItems = new List<Item>(); // 판매 가능한 모든 아이템 리스트

    private PlayerInventory playerInventory;  // 플레이어 인벤토리 참조
    private UI_ShopManager shopManager;       // 상점 매니저 참조

    private int currentWave = 1;              // 현재 웨이브 번호 (UI_GameWave에서 전달)

    // 초기화
    private void Start()
    {
        playerInventory = FindObjectOfType<PlayerInventory>();
        shopManager = FindObjectOfType<UI_ShopManager>();

        // 버튼 이벤트 연결
        rerollButton?.onClick.AddListener(OnClickReroll);
        nextWaveButton?.onClick.AddListener(OnClickNextWave);
    }

    private void OnEnable()
    {
        // 인벤토리 참조 없을 경우 재할당
        if (playerInventory == null)
            playerInventory = FindObjectOfType<PlayerInventory>();

        // 코인 변경 이벤트 구독
        if (playerInventory != null)
            playerInventory.OnInventoryChanged += UpdateCoinUI;

        UpdateCoinUI();

        // 리스트 새로고침 (패시브, 무기 아이콘 업데이트)
        FindObjectOfType<UI_PassiveList>()?.RefreshList();
        FindObjectOfType<UI_WeaponList>()?.RefreshWeaponList();
    }

    private void OnDisable()
    {
        // 이벤트 구독 해제 (씬 변경 시 메모리 누수 방지)
        if (playerInventory != null)
            playerInventory.OnInventoryChanged -= UpdateCoinUI;
    }

    // 상점 열기 (UI_ShopManager에서 호출)
    public void OpenShop(int waveNumber)
    {
        currentWave = waveNumber;
        gameObject.SetActive(true);
        RefreshShopItems();
    }

    // 상점 슬롯 갱신 (무작위 아이템 4개 선택)
    private void RefreshShopItems()
    {
        UpdateCoinUI();

        // 웨이브마다 10% 가격 상승
        float priceMultiplier = 1f + 0.1f * (currentWave - 1);

        foreach (GameObject slotObj in itemSlots)
        {
            if (slotObj == null) continue;

            ShopItemSlot slot = slotObj.GetComponent<ShopItemSlot>();
            if (slot == null) continue;

            // 무작위 아이템 선택
            Item randomItem = allItems[Random.Range(0, allItems.Count)];
            if (randomItem == null) continue;

            // 복제본 생성 후 가격 조정
            Item clonedItem = ScriptableObject.Instantiate(randomItem);
            clonedItem.price = Mathf.RoundToInt(randomItem.price * priceMultiplier);

            // 슬롯에 아이템 정보 세팅
            slot.SetItem(clonedItem, playerInventory);
        }
    }

    // 코인 텍스트 갱신
    private void UpdateCoinUI()
    {
        if (playerInventory == null || coinText == null) return;
        coinText.text = $"Coins: {playerInventory.gold}";
    }

    // 리롤 버튼 클릭 시
    private void OnClickReroll()
    {
        if (playerInventory == null) return;
        if (playerInventory.gold < rerollCost) return;

        playerInventory.gold -= rerollCost;
        UpdateCoinUI();
        RefreshShopItems();
    }

    // 다음 웨이브로 이동
    private void OnClickNextWave()
    {
        gameObject.SetActive(false);

        // 상점 매니저에게 다음 웨이브 진행 알림
        shopManager?.OnGoNextWave();
    }
}
