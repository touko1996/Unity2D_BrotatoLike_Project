using System.Collections.Generic;
using UnityEngine;

// 플레이어 주변에 무기를 배치할 슬롯 관리
public class WeaponSlotManager : MonoBehaviour
{
    [Header("무기 슬롯 설정")]
    [SerializeField] private float slotDistance = 1.5f; // 플레이어와 슬롯 간 거리
    [SerializeField] private Transform player;          // 플레이어 참조

    private readonly List<Transform> _weaponSlots = new List<Transform>();

    private void Awake()
    {
        CreateWeaponSlots();
    }

    private void CreateWeaponSlots()
    {
        // 기존 슬롯 제거
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            if (transform.GetChild(i).name.StartsWith("WeaponSlot_"))
                DestroyImmediate(transform.GetChild(i).gameObject);
        }

        _weaponSlots.Clear();

        // 슬롯 6개 고정 생성
        int slotCount = 6;
        for (int i = 0; i < slotCount; i++)
        {
            GameObject slot = new GameObject($"WeaponSlot_{i + 1}");
            slot.transform.SetParent(transform, false);
            _weaponSlots.Add(slot.transform);
        }

        UpdateSlotPositions();
    }

    private void Update()
    {
        UpdateSlotPositions();
    }

    private void LateUpdate()
    {
        // 플레이어 위치를 따라가되, 스케일 변화는 무시
        if (player != null)
            transform.position = player.position;
    }

    // 슬롯의 위치를 원형으로 재배치
    private void UpdateSlotPositions()
    {
        if (_weaponSlots.Count == 0) return;

        for (int i = 0; i < _weaponSlots.Count; i++)
        {
            float angle = (360f / _weaponSlots.Count) * i * Mathf.Deg2Rad;
            Vector3 pos = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f) * slotDistance;
            _weaponSlots[i].localPosition = pos;
        }
    }

    // 비어 있는 슬롯 반환
    public Transform GetEmptySlot()
    {
        foreach (Transform slot in _weaponSlots)
        {
            if (slot.childCount == 0)
                return slot;
        }
        return null;
    }

    // 현재 장착된 무기 개수
    public int GetCurrentWeaponCount()
    {
        int count = 0;
        foreach (Transform slot in _weaponSlots)
        {
            if (slot.childCount > 0)
                count++;
        }
        return count;
    }
}
