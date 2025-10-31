using UnityEngine;
using TMPro;

public class UI_PlayerStatsPanel : MonoBehaviour
{
    public PlayerStats playerStats;

    public TMP_Text damageText;
    public TMP_Text rangeText;
    public TMP_Text attackSpeedText;
    public TMP_Text moveSpeedText;
    public TMP_Text maxHpText;

    private void Update()
    {
        if (playerStats == null) return;

        if (damageText != null)
            damageText.text = "Damage: " + playerStats.currentDamage.ToString("F1");

        if (rangeText != null)
            rangeText.text = "Range: " + playerStats.currentRange.ToString("F1");

        if (attackSpeedText != null)
            attackSpeedText.text = "AtkSpeed: " + playerStats.currentAttackSpeed.ToString("F2");

        if (moveSpeedText != null)
            moveSpeedText.text = "Move: " + playerStats.currentMoveSpeed.ToString("F1");

        if (maxHpText != null)
            maxHpText.text = "MaxHP: " + playerStats.maxHp.ToString("F0");
    }
}
