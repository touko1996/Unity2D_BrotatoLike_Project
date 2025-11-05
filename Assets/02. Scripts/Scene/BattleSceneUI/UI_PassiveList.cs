using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_PassiveList : MonoBehaviour
{
    public GameObject iconPrefab;   // 아이콘 프리팹 (80x80 Image)
    public Transform container;     // PassiveContainer (GridLayoutGroup)

    private PlayerInventory inventory;

    private void Start()
    {
        inventory = FindObjectOfType<PlayerInventory>();
        if (inventory != null)
        {
            // ★ 인벤토리 변경 이벤트 구독
            inventory.OnInventoryChanged += RefreshList;
        }

        RefreshList();
    }

    private void OnDestroy()
    {
        if (inventory != null)
        {
            // ★ 구독 해제 (씬 변경 시 메모리 누수 방지)
            inventory.OnInventoryChanged -= RefreshList;
        }
    }

    public void RefreshList()
    {
        if (inventory == null || iconPrefab == null || container == null)
            return;

        // 기존 아이콘 모두 삭제
        for (int i = container.childCount - 1; i >= 0; i--)
        {
            Destroy(container.GetChild(i).gameObject);
        }

        // 패시브 아이템 리스트 불러오기
        List<PassiveItem> passives = inventory.GetOwnedPassives();

        foreach (var p in passives)
        {
            GameObject icon = Instantiate(iconPrefab, container);
            Image img = icon.GetComponent<Image>();

            if (img != null && p.itemSprite != null)
                img.sprite = p.itemSprite;
        }
    }
}
