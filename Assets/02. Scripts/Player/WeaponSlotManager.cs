using System.Collections.Generic;
using UnityEngine;

// �÷��̾� �ֺ��� ���⸦ ��ġ�� ���� ����
public class WeaponSlotManager : MonoBehaviour
{
    [Header("���� ���� ����")]
    [SerializeField] private float slotDistance = 1.5f; // �÷��̾�� ���� �� �Ÿ�
    [SerializeField] private Transform player;          // �÷��̾� ����

    private readonly List<Transform> _weaponSlots = new List<Transform>();

    private void Awake()
    {
        CreateWeaponSlots();
    }

    private void CreateWeaponSlots()
    {
        // ���� ���� ����
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            if (transform.GetChild(i).name.StartsWith("WeaponSlot_"))
                DestroyImmediate(transform.GetChild(i).gameObject);
        }

        _weaponSlots.Clear();

        // ���� 6�� ���� ����
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
        // �÷��̾� ��ġ�� ���󰡵�, ������ ��ȭ�� ����
        if (player != null)
            transform.position = player.position;
    }

    // ������ ��ġ�� �������� ���ġ
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

    // ��� �ִ� ���� ��ȯ
    public Transform GetEmptySlot()
    {
        foreach (Transform slot in _weaponSlots)
        {
            if (slot.childCount == 0)
                return slot;
        }
        return null;
    }

    // ���� ������ ���� ����
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
