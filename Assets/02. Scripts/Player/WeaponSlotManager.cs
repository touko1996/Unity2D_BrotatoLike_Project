using System.Collections.Generic;
using UnityEngine;

// 무기 슬롯을 생성하고 관리하는 매니저
public class WeaponSlotManager : MonoBehaviour
{
    [Header("무기 슬롯 설정")]
    [SerializeField] private float slotDistance = 1.5f; //플레이어를 기준으로 무기들이 도는 거리
    [SerializeField] private int slotCount = 6; //총 슬롯 개수

    [Header("참조")]
    [SerializeField] private Transform player; //플레이어의 위치
    [SerializeField] private List<Transform> _weaponSlots = new List<Transform>(); //무기 슬롯 목록

    void Awake()
    {
        //무기 슬롯 생성
        for (int i = 0; i < slotCount; i++)
        {
            GameObject slot = new GameObject("WeaponSlot_" + (i + 1)); //무기 슬롯 이름
            slot.transform.SetParent(transform, false);
            _weaponSlots.Add(slot.transform);
        }
    }

    void Update()
    {
        //플레이어의 위치를 따라가게 설정
        if (player != null)
        {
            transform.position = player.position;
        }

        //슬롯 배치 갱신
        UpdateSlotPositions();
    }

    private void UpdateSlotPositions()
    {
        //슬롯들을 원형 형태로 배치
        for (int i = 0; i < _weaponSlots.Count; i++)
        {
            float angle = (360f / _weaponSlots.Count) * i * Mathf.Deg2Rad; //각 슬롯의 회전 각도
            Vector3 pos = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f) * slotDistance;
            _weaponSlots[i].localPosition = pos;
        }
    }

    // 비어있는 슬롯 반환
    public Transform GetEmptySlot()
    {
        foreach (Transform slot in _weaponSlots)
        {
            if (slot.childCount == 0)
            {
                return slot;
            }
        }
        Debug.Log("WeaponSlotManager - No Empty Slot Available");
        return null;
    }

    // 슬롯 전체 비우기
    public void ClearAllSlots()
    {
        foreach (Transform slot in _weaponSlots)
        {
            for (int i = 0; i < slot.childCount; i++)
            {
                Destroy(slot.GetChild(i).gameObject);
            }
        }
        Debug.Log("WeaponSlotManager - All Slots Cleared");
    }

    // 현재 장착된 무기 개수 반환
    public int GetEquippedWeaponCount()
    {
        int count = 0;
        foreach (Transform slot in _weaponSlots)
        {
            if (slot.childCount > 0)
            {
                count++;
            }
        }
        return count;
    }
}
