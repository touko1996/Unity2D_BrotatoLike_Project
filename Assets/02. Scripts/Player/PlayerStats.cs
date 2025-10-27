using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Current Stats")]
    public float currentDamage = 10f;
    public float currentRange = 5f;
    public float currentAttackSpeed = 1f;
    public float currentMoveSpeed = 5f;

    // 간단한 증감 메서드만 유지
    public void AddStatModifier(float dmg, float range, float atkSpeed, float move)
    {
        currentDamage += dmg;
        currentRange += range;
        currentAttackSpeed += atkSpeed;
        currentMoveSpeed += move;
    }

    public void RemoveStatModifier(float dmg, float range, float atkSpeed, float move)
    {
        currentDamage -= dmg;
        currentRange -= range;
        currentAttackSpeed -= atkSpeed;
        currentMoveSpeed -= move;
    }
}
