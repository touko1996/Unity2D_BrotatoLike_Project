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
            damageText.text = "공격력: " + playerStats.currentDamage.ToString("F1");

        if (rangeText != null)
            rangeText.text = "공격사거리: " + playerStats.currentRange.ToString("F1");

        if (attackSpeedText != null)
            attackSpeedText.text = "공격속도: " + playerStats.currentAttackSpeed.ToString("F2");

        if (moveSpeedText != null)
            moveSpeedText.text = "이동속도: " + playerStats.currentMoveSpeed.ToString("F1");

        if (maxHpText != null)
            maxHpText.text = "최대체력: " + playerStats.maxHp.ToString("F0");
    }
}
