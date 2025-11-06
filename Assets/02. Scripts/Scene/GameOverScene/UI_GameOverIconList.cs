using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// [UI_GameOverIconList]
/// ------------------------------------------------------------
/// 게임 오버 화면에서 플레이어가 보유했던 무기 / 패시브 아이템의
/// 아이콘 목록을 표시하는 스크립트.
/// 
/// PlayerPrefsData에 저장된 마지막 게임 데이터를 기반으로
/// 각 카테고리별 아이콘을 생성한다.
/// ------------------------------------------------------------
/// </summary>
public class UI_GameOverIconList : MonoBehaviour
{
    [Header("UI 참조")]
    [SerializeField] private GameObject iconPrefab;      // 80x80 크기의 아이콘 프리팹
    [SerializeField] private Transform weaponContainer;  // 무기 아이콘 표시 영역 (오른쪽 아래)
    [SerializeField] private Transform passiveContainer; // 패시브 아이콘 표시 영역 (왼쪽 아래)

    private void Start()
    {
        RefreshIconLists();
    }

    /// <summary>
    /// 무기 / 패시브 아이콘 목록 갱신
    /// </summary>
    private void RefreshIconLists()
    {
        // 기존 아이콘 제거
        ClearContainer(weaponContainer);
        ClearContainer(passiveContainer);

        // Grid 자동 구성
        SetupGridIfNeeded(weaponContainer);
        SetupGridIfNeeded(passiveContainer);

        // 무기 아이콘 생성
        foreach (string weaponName in PlayerPrefsData.ownedWeaponNames)
        {
            GameObject icon = Instantiate(iconPrefab, weaponContainer);
            Image img = icon.GetComponent<Image>();

            Sprite sprite = FindWeaponSprite(weaponName);
            if (sprite != null)
                img.sprite = sprite;
        }

        // 패시브 아이콘 생성
        foreach (string passiveName in PlayerPrefsData.ownedPassiveNames)
        {
            GameObject icon = Instantiate(iconPrefab, passiveContainer);
            Image img = icon.GetComponent<Image>();

            Sprite sprite = FindPassiveSprite(passiveName);
            if (sprite != null)
                img.sprite = sprite;
        }
    }

    /// <summary>
    /// 컨테이너 내의 모든 자식 오브젝트 삭제
    /// </summary>
    private void ClearContainer(Transform container)
    {
        for (int i = container.childCount - 1; i >= 0; i--)
            Destroy(container.GetChild(i).gameObject);
    }

    /// <summary>
    /// GridLayoutGroup 자동 세팅
    /// </summary>
    private void SetupGridIfNeeded(Transform container)
    {
        GridLayoutGroup grid = container.GetComponent<GridLayoutGroup>();
        if (grid == null)
            grid = container.gameObject.AddComponent<GridLayoutGroup>();

        grid.cellSize = new Vector2(80, 80);
        grid.spacing = new Vector2(10, 10);
        grid.startCorner = GridLayoutGroup.Corner.UpperLeft;
        grid.startAxis = GridLayoutGroup.Axis.Horizontal;
        grid.childAlignment = TextAnchor.UpperLeft;
        grid.constraint = GridLayoutGroup.Constraint.FixedRowCount;
        grid.constraintCount = 2;
    }

    /// <summary>
    /// 무기 이름으로 해당 무기 스프라이트를 찾아 반환
    /// </summary>
    private Sprite FindWeaponSprite(string weaponName)
    {
        foreach (var weapon in Resources.LoadAll<WeaponData>("Weapon"))
        {
            if (weapon.itemName == weaponName)
                return weapon.itemSprite;
        }
        return null;
    }

    /// <summary>
    /// 패시브 이름으로 해당 패시브 스프라이트를 찾아 반환
    /// </summary>
    private Sprite FindPassiveSprite(string passiveName)
    {
        foreach (var passive in Resources.LoadAll<PassiveItem>("PassiveItem"))
        {
            if (passive.itemName == passiveName)
                return passive.itemSprite;
        }
        return null;
    }
}
