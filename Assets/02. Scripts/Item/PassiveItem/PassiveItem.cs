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
        if (playerStats == null)
        {
            Debug.LogWarning("PlayerStats 컴포넌트를 찾을 수 없습니다.");
            return;
        }

        // 스탯 적용
        playerStats.AddStatModifier(damageBonus, rangeBonus, attackSpeedBonus, moveSpeedBonus);

        // 디버그 로그 (적용된 수치 표시)
        Debug.Log(
            $"[PassiveItem 적용됨] {itemName}\n" +
            $"공격력 +{damageBonus}\n" +
            $"공격속도 +{attackSpeedBonus}\n" +
            $"이동속도 +{moveSpeedBonus}\n" +
            $"공격범위 +{rangeBonus}"
        );
    }

    public override void RemoveEffect(GameObject player)
    {
        PlayerStats playerStats = player.GetComponent<PlayerStats>();
        if (playerStats == null)
        {
            Debug.LogWarning("PlayerStats 컴포넌트를 찾을 수 없습니다.");
            return;
        }

        // 스탯 해제
        playerStats.RemoveStatModifier(damageBonus, rangeBonus, attackSpeedBonus, moveSpeedBonus);

        Debug.Log($"[PassiveItem 해제됨] {itemName}");
    }
}
