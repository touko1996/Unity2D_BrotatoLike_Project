using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_GameOverIconList : MonoBehaviour
{
    [Header("UI References")]
    public GameObject iconPrefab; // 80x80 이미지 프리팹
    public Transform weaponContainer; // 오른쪽 아래
    public Transform passiveContainer; // 왼쪽 아래

    private void Start()
    {
        RefreshIconLists();
    }

    private void RefreshIconLists()
    {
        // 기존 아이콘 삭제
        ClearContainer(weaponContainer);
        ClearContainer(passiveContainer);

        // 무기 아이콘 생성
        foreach (string weaponName in PlayerPrefsData.weaponNames)
        {
            GameObject icon = Instantiate(iconPrefab, weaponContainer);
            Image img = icon.GetComponent<Image>();

            Sprite sprite = FindWeaponSprite(weaponName);
            if (sprite != null)
                img.sprite = sprite;
        }

        // 패시브 아이콘 생성
        foreach (string passiveName in PlayerPrefsData.passiveNames)
        {
            GameObject icon = Instantiate(iconPrefab, passiveContainer);
            Image img = icon.GetComponent<Image>();

            Sprite sprite = FindPassiveSprite(passiveName);
            if (sprite != null)
                img.sprite = sprite;
        }
    }

    private void ClearContainer(Transform container)
    {
        for (int i = container.childCount - 1; i >= 0; i--)
            Destroy(container.GetChild(i).gameObject);
    }

    // 무기 Sprite 찾기
    private Sprite FindWeaponSprite(string weaponName)
    {
        foreach (var w in Resources.LoadAll<WeaponData>("Weapon"))
        {
            if (w.itemName == weaponName)
                return w.itemSprite;
        }
        return null;
    }

    // 패시브 Sprite 찾기
    private Sprite FindPassiveSprite(string passiveName)
    {
        foreach (var p in Resources.LoadAll<PassiveItem>("PassiveItem"))
        {
            if (p.itemName == passiveName)
                return p.itemSprite;
        }
        return null;
    }
}
