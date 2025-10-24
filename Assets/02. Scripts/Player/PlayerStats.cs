using UnityEngine;

// �÷��̾��� �⺻ �� ���� ���� ����
public class PlayerStats : MonoBehaviour
{
    [Header("�⺻ ����")]
    public float baseDamage = 10f;
    public float baseRange = 5f;
    public float baseAttackSpeed = 1f;
    public float baseMoveSpeed = 5f;

    [Header("���� ����")]
    public float currentDamage;
    public float currentRange;
    public float currentAttackSpeed;
    public float currentMoveSpeed;

    private void Awake()
    {
        ResetStats();
    }

    // ��� ������ �⺻������ �ʱ�ȭ
    public void ResetStats()
    {
        currentDamage = baseDamage;
        currentRange = baseRange;
        currentAttackSpeed = baseAttackSpeed;
        currentMoveSpeed = baseMoveSpeed;
    }

    // ���� �߰�
    public void AddStatModifier(float dmg, float range, float atkSpeed, float move)
    {
        currentDamage += dmg;
        currentRange += range;
        currentAttackSpeed += atkSpeed;
        currentMoveSpeed += move;
    }

    // ���� ����
    public void RemoveStatModifier(float dmg, float range, float atkSpeed, float move)
    {
        currentDamage -= dmg;
        currentRange -= range;
        currentAttackSpeed -= atkSpeed;
        currentMoveSpeed -= move;
    }
}
