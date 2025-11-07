using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// [UI_PassiveList]
/// ------------------------------------------------------------
/// 패시브 아이템 UI 목록을 표시하는 스크립트
/// - 플레이어 인벤토리의 패시브 아이템 리스트를 불러와
///   각 아이콘을 자동으로 생성 및 갱신함
/// - 아이템 구매, 환불 등 인벤토리 변경 시 자동 갱신
/// ------------------------------------------------------------
/// </summary>
public class UI_PassiveList : MonoBehaviour
{
    [Header("UI 참조")]
    [SerializeField] private GameObject iconPrefab;   // 아이콘 프리팹 (80x80 크기)
    [SerializeField] private Transform container;     // 아이콘이 배치될 부모 컨테이너 (GridLayoutGroup)

    private PlayerInventory playerInventory;          // 플레이어 인벤토리 참조

    private void Start()
    {
        // 플레이어 인벤토리 검색
        playerInventory = FindObjectOfType<PlayerInventory>();

        if (playerInventory != null)
        {
            // 인벤토리 변경 이벤트 구독 (아이템 추가/환불 시 자동 업데이트)
            playerInventory.OnInventoryChanged += RefreshList;
        }

        RefreshList();
    }

    private void OnDestroy()
    {
        // 씬 전환 시 이벤트 구독 해제 (메모리 누수 방지)
        if (playerInventory != null)
        {
            playerInventory.OnInventoryChanged -= RefreshList;
        }
    }

    /// <summary>
    /// 현재 보유 중인 패시브 아이템 목록으로 UI를 갱신한다.
    /// </summary>
    public void RefreshList()
    {
        if (playerInventory == null || iconPrefab == null || container == null)
            return;

        // 기존 아이콘 모두 제거
        ClearContainer(container);

        // 플레이어 인벤토리에서 패시브 아이템 목록 가져오기
        List<PassiveItem> ownedPassives = playerInventory.GetOwnedPassives();

        // 보유한 패시브마다 아이콘 생성
        foreach (PassiveItem passive in ownedPassives)
        {
            GameObject icon = Instantiate(iconPrefab, container);
            Image iconImage = icon.GetComponent<Image>();

            if (iconImage != null && passive.itemSprite != null)
            {
                iconImage.sprite = passive.itemSprite;
            }
        }
    }

    /// <summary>
    /// 컨테이너 내 모든 자식 오브젝트 제거
    /// </summary>
    private void ClearContainer(Transform targetContainer)
    {
        for (int i = targetContainer.childCount - 1; i >= 0; i--)
        {
            Destroy(targetContainer.GetChild(i).gameObject);
        }
    }
}
