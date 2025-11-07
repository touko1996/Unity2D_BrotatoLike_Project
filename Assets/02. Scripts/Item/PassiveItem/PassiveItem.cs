using UnityEngine;

// 패시브 아이템: 스탯을 직접 증가/감소시키는 아이템
[CreateAssetMenu(fileName = "NewPassiveItem", menuName = "Items/PassiveItem")]
public class PassiveItem : Item
{
    [Header("스탯 변화 값")]
    public float damageBonus = 0f;
    public float rangeBonus = 0f;
    public float attackSpeedBonus = 0f;
    public float moveSpeedBonus = 0f;

    public override void ApplyEffect(GameObject player)
    {
        PlayerStats playerStats = player.GetComponent<PlayerStats>();
        if (playerStats == null) return;

        // 스탯 적용
        playerStats.AddStatModifier(damageBonus, rangeBonus, attackSpeedBonus, moveSpeedBonus);
    }
}
