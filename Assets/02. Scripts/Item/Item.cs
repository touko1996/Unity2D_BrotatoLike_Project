using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : ScriptableObject
{
    [Header("�������� �⺻����")]
    public string itemName;
    [TextArea] public string description;
    public Sprite itemSprite;
    public int price;

    public abstract void ApplyEffect(GameObject player); //�÷��̾�� ������ȿ���� �����Ҷ� ȣ��
    public virtual void RemoveEffect(GameObject player) { } //�÷��̾�� ������ȿ���� �����Ҷ� ȣ��/ �нú�� �ǸŰ� �ȵǰ� ����� �ǸŸ� �������Ű���
    public virtual void RefundAtStore(GameObject player) //�����Ǹű��
    {

    }
    public virtual void MixAtStore(GameObject player) //�������� ���� ����� ���⸦ ���� ��޾� ����
    {

    }
    
   
}
