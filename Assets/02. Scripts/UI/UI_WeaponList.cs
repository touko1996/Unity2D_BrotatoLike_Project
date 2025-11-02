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

        // 환불/합성 선택 처리
        
        ShowWeaponOptions(weapon);
    }

    // 간단 버전: 콘솔에서 환불 or 합성
    private void ShowWeaponOptions(WeaponData weapon)
    {
        // 예: Shift 누르고 클릭하면 환불, Ctrl 누르고 클릭하면 Mix
        if (Input.GetKey(KeyCode.LeftShift))
        {
            inventory.RefundItem(weapon);
            Debug.Log($"{weapon.itemName} 환불 완료!");
        }
        else if (Input.GetKey(KeyCode.LeftControl))
        {
            weapon.MixAtStore(inventory.gameObject);
            Debug.Log($"{weapon.itemName} 합성 완료!");
        }
        else
        {
            Debug.Log("Shift 클릭 → 환불, Ctrl 클릭 → 합성");
        }

        RefreshList();
    }
}
