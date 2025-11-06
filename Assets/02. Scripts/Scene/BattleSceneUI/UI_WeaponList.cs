using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// [UI_WeaponList]
/// ------------------------------------------------------------
/// 플레이어가 현재 보유 중인 무기 목록을 UI로 표시.
/// - PlayerInventory의 OnInventoryChanged 이벤트를 구독하여 실시간 갱신.
/// - 아이콘 클릭 시 Shift 키와 함께 누르면 환불 기능 실행.
/// ------------------------------------------------------------
/// </summary>
public class UI_WeaponList : MonoBehaviour
{
    [Header("UI 참조")]
    [SerializeField] private GameObject iconPrefab;   // 무기 아이콘 프리팹 (80x80 이미지)
    [SerializeField] private Transform container;     // 아이콘이 배치될 부모 컨테이너 (GridLayoutGroup)

    private PlayerInventory playerInventory;          // 플레이어 인벤토리 참조
    
    // 초기화
    private void Start()
    {
        playerInventory = FindObjectOfType<PlayerInventory>();
        if (playerInventory != null)
        {
            // 인벤토리 변경 시 자동 새로고침
            playerInventory.OnInventoryChanged += RefreshWeaponList;
        }

        RefreshWeaponList();
    }

    private void OnDestroy()
    {
        if (playerInventory != null)
        {
            // 이벤트 구독 해제 (씬 변경 시 메모리 누수 방지)
            playerInventory.OnInventoryChanged -= RefreshWeaponList;
        }
    }
   
    // 무기 목록 UI 새로고침
    public void RefreshWeaponList()
    {
        if (playerInventory == null || iconPrefab == null || container == null)
            return;

        // 기존 아이콘 모두 제거
        for (int i = container.childCount - 1; i >= 0; i--)
        {
            Destroy(container.GetChild(i).gameObject);
        }

        // 현재 인벤토리의 무기 목록 불러오기
        List<WeaponData> ownedWeapons = playerInventory.GetOwnedWeapons();

        foreach (WeaponData weapon in ownedWeapons)
        {
            if (weapon == null) continue;

            // 아이콘 생성
            GameObject icon = Instantiate(iconPrefab, container);

            // 스프라이트 적용
            Image iconImage = icon.GetComponent<Image>();
            if (iconImage != null && weapon.itemSprite != null)
                iconImage.sprite = weapon.itemSprite;

            // 클릭 이벤트 설정 (Shift + 클릭 → 환불)
            Button iconButton = icon.GetComponent<Button>() ?? icon.AddComponent<Button>();
            iconButton.onClick.RemoveAllListeners();
            iconButton.onClick.AddListener(() => OnWeaponIconClicked(weapon));
        }
    }
    
    // 무기 아이콘 클릭 시
    private void OnWeaponIconClicked(WeaponData weapon)
    {
        // Shift 키가 눌린 상태에서만 환불 처리
        if (Input.GetKey(KeyCode.LeftShift))
        {
            RefundWeapon(weapon);
        }
    }

    // 무기 환불 처리 (구매가의 50%)
    private void RefundWeapon(WeaponData weapon)
    {
        if (playerInventory == null || weapon == null)
            return;

        // 환불 금액 계산
        int refundAmount = Mathf.RoundToInt(weapon.price * 0.5f);

        // 인벤토리에서 무기 제거
        playerInventory.RefundItem(weapon);

        // 골드 환불
        playerInventory.gold += refundAmount;

        // UI 실시간 갱신
        playerInventory.OnInventoryChanged?.Invoke();
        RefreshWeaponList();

        // 환불 사운드 효과 (선택사항)
        AudioManager.Instance?.PlayCoinSFX();
    }
}
