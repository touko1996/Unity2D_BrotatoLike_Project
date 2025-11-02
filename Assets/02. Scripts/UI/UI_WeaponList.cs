using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_WeaponList : MonoBehaviour
{
    public GameObject iconPrefab;   // 아이콘 프리팹 (80x80 Image)
    public Transform container;     // WeaponContainer (GridLayoutGroup)

    private PlayerInventory inventory;

    private void Start()
    {
        inventory = FindObjectOfType<PlayerInventory>();
        if (inventory != null)
        {
            // ★ 인벤토리 변경 시 자동 갱신
            inventory.OnInventoryChanged += RefreshList;
        }

        RefreshList();
    }

    private void OnDestroy()
    {
        if (inventory != null)
        {
            // ★ 구독 해제
            inventory.OnInventoryChanged -= RefreshList;
        }
    }

    public void RefreshList()
    {
        if (inventory == null || iconPrefab == null || container == null)
            return;

        // 기존 아이콘 전부 삭제
        for (int i = container.childCount - 1; i >= 0; i--)
        {
            Destroy(container.GetChild(i).gameObject);
        }

        // 무기 리스트 가져오기
        List<WeaponData> weapons = inventory.GetOwnedWeapons();

        foreach (var w in weapons)
        {
            GameObject icon = Instantiate(iconPrefab, container);
            Image img = icon.GetComponent<Image>();

            if (img != null && w.itemSprite != null)
                img.sprite = w.itemSprite;
        }
    }
}
