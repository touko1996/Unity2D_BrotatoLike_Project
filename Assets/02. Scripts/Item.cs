using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : ScriptableObject
{
    [Header("아이템의 기본정보")]
    public string itemName;
    [TextArea] public string description;
    public Sprite itemSprite;
    public int price;

    public abstract void ApplyEffect(GameObject player); //플레이어에게 아이템효과를 적용할때 호출
    public virtual void RemoveEffect(GameObject player) { } //플레이어에게 아이템효과를 제거할때 호출/ 패시브는 판매가 안되고 무기는 판매를 통해제거가능
    public virtual void RefundAtStore(GameObject player) //상점판매기능
    {

    }
    public virtual void MixAtStore(GameObject player) //상점에서 같은 등급의 무기를 합쳐 등급업 가능
    {

    }
    
   
}
