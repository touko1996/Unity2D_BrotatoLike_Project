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
        if (playerStats == null)
        {
            Debug.LogWarning("PlayerStats ������Ʈ�� ã�� �� �����ϴ�.");
            return;
        }

        // ���� ����
        playerStats.AddStatModifier(damageBonus, rangeBonus, attackSpeedBonus, moveSpeedBonus);

        // ����� �α� (����� ��ġ ǥ��)
        Debug.Log(
            $"[PassiveItem �����] {itemName}\n" +
            $"���ݷ� +{damageBonus}\n" +
            $"���ݼӵ� +{attackSpeedBonus}\n" +
            $"�̵��ӵ� +{moveSpeedBonus}\n" +
            $"���ݹ��� +{rangeBonus}"
        );
    }

    public override void RemoveEffect(GameObject player)
    {
        PlayerStats playerStats = player.GetComponent<PlayerStats>();
        if (playerStats == null)
        {
            Debug.LogWarning("PlayerStats ������Ʈ�� ã�� �� �����ϴ�.");
            return;
        }

        // ���� ����
        playerStats.RemoveStatModifier(damageBonus, rangeBonus, attackSpeedBonus, moveSpeedBonus);

        Debug.Log($"[PassiveItem ������] {itemName}");
    }
}
