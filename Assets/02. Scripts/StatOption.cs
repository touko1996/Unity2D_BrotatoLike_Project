using UnityEngine;

[System.Serializable]
public class StatOption
{
    public string statName;        // ���� �̸� (��: ���ݷ� ����)
    public string description;     // ���� ����
    public System.Action<PlayerStats> applyEffect; // ȿ�� �Լ�
}
