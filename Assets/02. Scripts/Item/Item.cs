using UnityEngine;

public abstract class Item : ScriptableObject
{
    [Header("아이템 기본 정보")]
    public string itemName;           
    [TextArea] public string description; 
    public Sprite itemSprite;         
    public int price;                

    //아이템 효과 적용
    public abstract void ApplyEffect(GameObject player);
}
