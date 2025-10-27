using System.Collections.Generic;
using UnityEngine;

// ���� ������ �����ϰ� �����ϴ� �Ŵ���
public class WeaponSlotManager : MonoBehaviour
{
    [Header("���� ���� ����")]
    [SerializeField] private float slotDistance = 1.5f; //�÷��̾ �������� ������� ���� �Ÿ�
    [SerializeField] private int slotCount = 6; //�� ���� ����

    [Header("����")]
    [SerializeField] private Transform player; //�÷��̾��� ��ġ
    [SerializeField] private List<Transform> _weaponSlots = new List<Transform>(); //���� ���� ���

    void Awake()
    {
        //���� ���� ����
        for (int i = 0; i < slotCount; i++)
        {
            GameObject slot = new GameObject("WeaponSlot_" + (i + 1)); //���� ���� �̸�
            slot.transform.SetParent(transform, false);
            _weaponSlots.Add(slot.transform);
        }
    }

    void Update()
    {
        //�÷��̾��� ��ġ�� ���󰡰� ����
        if (player != null)
        {
            transform.position = player.position;
        }

        //���� ��ġ ����
        UpdateSlotPositions();
    }

    private void UpdateSlotPositions()
    {
        //���Ե��� ���� ���·� ��ġ
        for (int i = 0; i < _weaponSlots.Count; i++)
        {
            float angle = (360f / _weaponSlots.Count) * i * Mathf.Deg2Rad; //�� ������ ȸ�� ����
            Vector3 pos = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f) * slotDistance;
            _weaponSlots[i].localPosition = pos;
        }
    }

    // ����ִ� ���� ��ȯ
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

    // ���� ��ü ����
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

    // ���� ������ ���� ���� ��ȯ
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
