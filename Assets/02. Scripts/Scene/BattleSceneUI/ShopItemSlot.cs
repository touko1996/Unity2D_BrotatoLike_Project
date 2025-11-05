using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopItemSlot : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text descText;
    public TMP_Text priceText;
    public Image iconImage;
    public Button buyButton;

    private Item currentItem;
    private PlayerInventory playerInventory;

    public void SetItem(Item item, PlayerInventory inventory)
    {
        currentItem = item;
        playerInventory = inventory;

        nameText.text = item.itemName;
        descText.text = item.description;
        priceText.text = item.price.ToString();

        if (iconImage != null && item.itemSprite != null)
            iconImage.sprite = item.itemSprite;

        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(BuyItem);
        buyButton.interactable = true;
    }

    private void BuyItem()
    {
        if (playerInventory.gold < currentItem.price) return;

        playerInventory.BuyItem(currentItem);
        priceText.text = "SOLD";
        buyButton.interactable = false;

        FindObjectOfType<UI_PassiveList>()?.RefreshList();
        
    }
}
