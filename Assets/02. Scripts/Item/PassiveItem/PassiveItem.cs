using UnityEngine;

// �нú� ������: ������ ���� ����/���ҽ�Ű�� ������
[CreateAssetMenu(fileName = "NewPassiveItem", menuName = "Items/PassiveItem")]
public class PassiveItem : Item
{
    [Header("���� ��ȭ ��")]
    public float damageBonus = 0f;
    public float rangeBonus = 0f;
    public float attackSpeedBonus = 0f;
    public float moveSpeedBonus = 0f;

    public override void ApplyEffect(GameObject player)
    {
        PlayerStats playerStats = player.GetComponent<PlayerStats>();
        if (playerStats == null) return;

        playerStats.AddStatModifier(damageBonus, rangeBonus, attackSpeedBonus, moveSpeedBonus);
        Debug.Log($"{itemName} �����! (���ݷ� +{damageBonus}, �̵��ӵ� +{moveSpeedBonus})");
    }

    public override void RemoveEffect(GameObject player)
    {
        PlayerStats playerStats = player.GetComponent<PlayerStats>();
        if (playerStats == null) return;

        playerStats.RemoveStatModifier(damageBonus, rangeBonus, attackSpeedBonus, moveSpeedBonus);
        Debug.Log($"{itemName} ������!");
    }
}
