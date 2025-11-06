using System.Collections.Generic;
using UnityEngine;

public class WeaponSlotManager : MonoBehaviour
{
    [Header("슬롯 설정")]
    [SerializeField] private float slotDistance = 1.5f;
    private List<Transform> weaponSlots = new List<Transform>();

    [Header("초기 무기")]
    [SerializeField] private WeaponData startingWeapon;

    private Transform player;

    private void Awake()
    {
        // 슬롯 생성
        for (int i = 0; i < 6; i++)
        {
            GameObject slot = new GameObject("WeaponSlot_" + (i + 1));
            slot.transform.SetParent(transform, false);
            weaponSlots.Add(slot.transform);
        }

        UpdateSlotPositions();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) player = playerObj.transform;
    }

    private void Start()
    {
        if (startingWeapon != null)
        {
            // 초기무기도 복제본으로 생성
            WeaponData clone = ScriptableObject.Instantiate(startingWeapon);
            clone.name = startingWeapon.name + "_Clone";

            // 슬롯에 장착
            Transform emptySlot = GetEmptySlot();
            if (emptySlot != null)
            {
                GameObject weaponObj = new GameObject(clone.itemName);
                weaponObj.transform.SetParent(emptySlot, false);

                SpriteRenderer weaponSr = weaponObj.AddComponent<SpriteRenderer>();
                if (clone.itemSprite != null)
                    weaponSr.sprite = clone.itemSprite;

                WeaponShooter shooter = weaponObj.AddComponent<WeaponShooter>();
                shooter.weaponData = clone;

                // 인벤토리에 복제본 등록
                PlayerInventory inven = FindObjectOfType<PlayerInventory>();
                if (inven != null && !inven.GetOwnedWeapons().Exists(w => w.itemName == clone.itemName))
                {
                    inven.ownedItems.Add(clone);
                    inven.OnInventoryChanged?.Invoke();
                }
            }
        }
    }

    private void LateUpdate()
    {
        if (player != null) transform.position = player.position;
    }

    private void UpdateSlotPositions()
    {
        for (int i = 0; i < weaponSlots.Count; i++)
        {
            float angle = (360f / weaponSlots.Count) * i * Mathf.Deg2Rad; //Mathf.Deg2Rad는 각도를 라디안으로 변환
            Vector3 pos = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f) * slotDistance;
            weaponSlots[i].localPosition = pos;
        }
    }

    public Transform GetEmptySlot()
    {
        foreach (Transform weaponSlot in weaponSlots)
        {
            if (weaponSlot.childCount == 0) return weaponSlot;
        }
        return null;
    }

    public int GetEquippedWeaponCount()
    {
        int count = 0;
        foreach (Transform weaponSlot in weaponSlots)
        {
            if (weaponSlot.childCount > 0)
                count++;
        }
        return count;
    }

    public bool RemoveWeaponByName(string weaponName)
    {
        foreach (Transform weaponSlot in weaponSlots)
        {
            if (weaponSlot.childCount > 0)
            {
                Transform child = weaponSlot.GetChild(0);
                string name = child.name.Replace("(Clone)", "").Trim();

                if (name == weaponName)
                {
                    GameObject.Destroy(child.gameObject);
                    return true;
                }
            }
        }
        return false;
    }
}
