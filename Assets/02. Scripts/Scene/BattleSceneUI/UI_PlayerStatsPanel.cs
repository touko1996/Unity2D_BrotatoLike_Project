using UnityEngine;
using TMPro;

/// <summary>
/// [UI_PlayerStatsPanel]
/// ------------------------------------------------------------
/// 플레이어의 주요 능력치(공격력, 사거리, 공격속도, 이동속도, 최대체력)를  
/// 실시간으로 UI에 표시하는 스크립트.
/// ------------------------------------------------------------
/// </summary>
public class UI_PlayerStatsPanel : MonoBehaviour
{
    [Header("플레이어 참조")]
    [SerializeField] private PlayerStats playerStats; // 플레이어 스탯 스크립트 참조

    [Header("스탯 텍스트 UI")]
    [SerializeField] private TMP_Text damageText;       // 공격력
    [SerializeField] private TMP_Text rangeText;        // 사거리
    [SerializeField] private TMP_Text attackSpeedText;  // 공격속도
    [SerializeField] private TMP_Text moveSpeedText;    // 이동속도
    [SerializeField] private TMP_Text maxHpText;        // 최대체력

    private void Update()
    {
        // 플레이어 스탯이 없으면 실행하지 않음
        if (playerStats == null)
            return;

        // 각 스탯 텍스트 업데이트
        UpdateStatText(damageText, "공격력", playerStats.currentDamage, "F1"); // 소수점 1자리
        UpdateStatText(rangeText, "사거리", playerStats.currentRange, "F1");
        UpdateStatText(attackSpeedText, "공격속도", playerStats.currentAttackSpeed, "F2"); // 소수점 2자리
        UpdateStatText(moveSpeedText, "이동속도", playerStats.currentMoveSpeed, "F1");
        UpdateStatText(maxHpText, "최대체력", playerStats.maxHp, "F0"); // 정수형
    }

    /// <summary>
    /// 특정 스탯 텍스트를 지정된 형식으로 갱신한다.
    /// </summary>
    private void UpdateStatText(TMP_Text textField, string label, float value, string format)
    {
        if (textField == null) return;
        textField.text = $"{label}: {value.ToString(format)}";
    }
}
