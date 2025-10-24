using UnityEngine;

// �������� �⺻ Ŭ���� (����, �нú� �������� �θ�)
public abstract class Item : ScriptableObject
{
    [Header("������ �⺻ ����")]
    public string itemName;
    [TextArea] public string description;
    public Sprite itemSprite;
    public int price;

    // ������ ȿ�� ���� (��ӹ��� Ŭ�������� ����)
    public abstract void ApplyEffect(GameObject player);

    // ������ ȿ�� ����
    public virtual void RemoveEffect(GameObject player) { }

    // ���� ȯ�ҿ� (���߿� ��� ����)
    public virtual void RefundAtStore(GameObject player) { }

    // �������� ���� ����� �������� ��ġ�� ��� (���߿� ��� ����)
    public virtual void MixAtStore(GameObject player) { }
}
