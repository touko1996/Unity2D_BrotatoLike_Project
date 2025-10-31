using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Current Stats")]
    public float currentDamage = 10f;
    public float currentRange = 5f;
    public float currentAttackSpeed = 1f;
    public float currentMoveSpeed = 5f;

    [Header("HP")]
    public float maxHp = 10f;
    public float currentHp = 10f;

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

    public void TakeDamage(float amount)
    {
        currentHp -= amount;
        if (currentHp < 0f) currentHp = 0f;

        if (currentHp <= 0f)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        currentHp += amount;
        if (currentHp > maxHp) currentHp = maxHp;
    }

    private void Die()
    {
        Debug.Log("Player died");
        // 나중에 게임오버 UI 연결
    }
}
