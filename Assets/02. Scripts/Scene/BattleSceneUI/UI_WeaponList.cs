using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_WeaponList : MonoBehaviour
{
    public GameObject iconPrefab;
    public Transform container;
    private PlayerInventory inventory;

    private void Start()
    {
        inventory = FindObjectOfType<PlayerInventory>();
        if (inventory != null)
        {
            inventory.OnInventoryChanged += RefreshList;
        }

        RefreshList();
    }

    private void OnDestroy()
    {
        if (inventory != null)
        {
            inventory.OnInventoryChanged -= RefreshList;
        }
    }

    public void RefreshList()
    {
        if (inventory == null || iconPrefab == null || container == null)
            return;

        // 기존 아이콘 제거
        for (int i = container.childCount - 1; i >= 0; i--)
        {
            Destroy(container.GetChild(i).gameObject);
        }

        // 현재 무기 목록 출력
        List<WeaponData> weapons = inventory.GetOwnedWeapons();

        foreach (var weapon in weapons)
        {
            GameObject icon = Instantiate(iconPrefab, container);
            Image img = icon.GetComponent<Image>();
            if (img != null && weapon.itemSprite != null)
                img.sprite = weapon.itemSprite;

            // 클릭 이벤트 추가
            Button btn = icon.GetComponent<Button>();
            if (btn == null)
                btn = icon.AddComponent<Button>();

            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() => OnWeaponIconClicked(weapon));
        }
    }

    // 무기 아이콘 클릭 시 호출
    private void OnWeaponIconClicked(WeaponData weapon)
    {
        Debug.Log($"[WeaponList] {weapon.itemName} 클릭됨 (Tier {weapon.tier})");

        // Shift 키로 환불 처리
        if (Input.GetKey(KeyCode.LeftShift))
        {
            ProcessRefund(weapon);
        }
        else
        {
            Debug.Log("Shift 키를 누른 상태에서 클릭 시 환불됩니다.");
        }
    }

    // ---------------------------------------------------------
    // 환불 처리 (구매가의 50% 회수)
    // ---------------------------------------------------------
    private void ProcessRefund(WeaponData weapon)
    {
        if (inventory == null || weapon == null)
            return;

        // 환불 금액 계산 (구매가격의 50%)
        int refundAmount = Mathf.RoundToInt(weapon.price * 0.5f);

        // 인벤토리에서 아이템 제거
        inventory.RefundItem(weapon);

        // 골드 추가
        inventory.gold += refundAmount;

        // UI 갱신
        inventory.OnInventoryChanged?.Invoke();

        Debug.Log($"[Refund] {weapon.itemName} 환불 완료! (+{refundAmount}G)");

        RefreshList();
    }
}
