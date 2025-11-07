using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// [ShopItemSlot]
/// 상점의 개별 아이템 슬롯을 제어하는 스크립트
/// - 아이템 정보 표시
/// - 구매 버튼 처리
/// - 구매 후 SOLD 처리
/// </summary>
public class ShopItemSlot : MonoBehaviour
{
    [Header("UI 구성 요소")]
    [SerializeField] private TMP_Text itemNameText;
    [SerializeField] private TMP_Text itemDescText;
    [SerializeField] private TMP_Text itemPriceText;
    [SerializeField] private Image itemIconImage;
    [SerializeField] private Button buyButton;

    private Item currentItem;
    private PlayerInventory playerInventory;

    /// <summary>
    /// 슬롯에 아이템과 인벤토리를 세팅한다
    /// </summary>
    public void SetItem(Item item, PlayerInventory inventory)
    {
        if (item == null || inventory == null)
            return;

        currentItem = item;
        playerInventory = inventory;

        if (itemNameText != null) itemNameText.text = item.itemName;
        if (itemDescText != null) itemDescText.text = item.description;
        if (itemPriceText != null) itemPriceText.text = "가격: "+ item.price.ToString();

        if (itemIconImage != null && item.itemSprite != null)
            itemIconImage.sprite = item.itemSprite;

        if (buyButton != null)
        {
            buyButton.onClick.RemoveAllListeners();
            buyButton.onClick.AddListener(OnBuyButtonClicked);
            buyButton.interactable = true;
        }
    }

    /// <summary>
    /// 구매 버튼 클릭 시 호출
    /// </summary>
    private void OnBuyButtonClicked()
    {
        if (currentItem == null || playerInventory == null)
            return;

        // 소지금 부족 시 리턴
        if (playerInventory.gold < currentItem.price)
        {
            // 실패 사운드 대신 코인 사운드라도 재생하고 싶으면 여기서 호출
            AudioManager.Instance?.PlayCoinSFX();
            return;
        }

        // 실제 구매
        playerInventory.BuyItem(currentItem);

        // UI 변경
        if (itemPriceText != null) itemPriceText.text = "SOLD";
        if (buyButton != null) buyButton.interactable = false;

        // 패시브 리스트 갱신
        FindObjectOfType<UI_PassiveList>()?.RefreshList();

        // 구매 성공 사운드
        AudioManager.Instance?.PlayCoinSFX();
    }
}
