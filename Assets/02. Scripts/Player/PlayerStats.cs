using UnityEngine;

// 플레이어의 기본 및 현재 스탯 관리
public class PlayerStats : MonoBehaviour
{
    [Header("기본 스탯")]
    public float baseDamage = 10f;
    public float baseRange = 5f;
    public float baseAttackSpeed = 1f;
    public float baseMoveSpeed = 5f;

    [Header("현재 스탯")]
    public float currentDamage;
    public float currentRange;
    public float currentAttackSpeed;
    public float currentMoveSpeed;

    private void Awake()
    {
        ResetStats();
    }

    // 모든 스탯을 기본값으로 초기화
    public void ResetStats()
    {
        currentDamage = baseDamage;
        currentRange = baseRange;
        currentAttackSpeed = baseAttackSpeed;
        currentMoveSpeed = baseMoveSpeed;
    }

    // 스탯 추가
    public void AddStatModifier(float dmg, float range, float atkSpeed, float move)
    {
        currentDamage += dmg;
        currentRange += range;
        currentAttackSpeed += atkSpeed;
        currentMoveSpeed += move;
    }

    // 스탯 제거
    public void RemoveStatModifier(float dmg, float range, float atkSpeed, float move)
    {
        currentDamage -= dmg;
        currentRange -= range;
        currentAttackSpeed -= atkSpeed;
        currentMoveSpeed -= move;
    }
}
