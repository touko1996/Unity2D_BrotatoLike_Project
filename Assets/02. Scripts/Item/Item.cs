using UnityEngine;

// 아이템의 기본 클래스 (무기, 패시브 아이템의 부모)
public abstract class Item : ScriptableObject
{
    [Header("아이템 기본 정보")]
    public string itemName;
    [TextArea] public string description;
    public Sprite itemSprite;
    public int price;

    // 아이템 효과 적용 (상속받은 클래스에서 구현)
    public abstract void ApplyEffect(GameObject player);

    // 아이템 효과 제거
    public virtual void RemoveEffect(GameObject player) { }

    // 상점 환불용 (나중에 사용 예정)
    public virtual void RefundAtStore(GameObject player) { }

    // 상점에서 같은 등급의 아이템을 합치는 기능 (나중에 사용 예정)
    public virtual void MixAtStore(GameObject player) { }
}
