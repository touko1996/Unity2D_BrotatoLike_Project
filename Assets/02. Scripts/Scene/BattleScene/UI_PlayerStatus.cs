using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// [UI_PlayerStatus]
/// ------------------------------------------------------------
/// 플레이어의 HP, 경험치, 레벨, 코인 정보를 UI에 실시간으로 표시한다.
/// - HP 게이지 및 수치 표시
/// - 경험치 게이지 및 레벨 표시
/// - 보유 코인 표시
/// ------------------------------------------------------------
/// </summary>
public class UI_PlayerStatus : MonoBehaviour
{
    [Header("플레이어 참조")]
    [SerializeField] private PlayerStats playerStats;         // 플레이어 스탯
    [SerializeField] private PlayerInventory playerInventory; // 플레이어 인벤토리

    [Header("HP UI")]
    [SerializeField] private Image hpFillImage;   // HP 게이지
    [SerializeField] private TMP_Text hpText;     // HP 텍스트

    [Header("경험치 UI")]
    [SerializeField] private Image expFillImage;  // 경험치 게이지
    [SerializeField] private TMP_Text levelText;  // 레벨 표시 텍스트

    [Header("코인 UI")]
    [SerializeField] private TMP_Text coinText;   // 코인 수 표시

    private void Start()
    {
        if (playerStats == null || playerInventory == null) return;

        // 초기 UI 값 설정
        UpdateHPUI(playerStats.currentHp, playerStats.maxHp);
        UpdateExpUI(playerInventory.currentExp, playerInventory.expToNextLevel);
        UpdateCoinUI(playerInventory.gold);
    }

    private void Update()
    {
        // HP는 프레임마다 갱신 (전투 중 수시로 변하므로)
        if (playerStats == null || hpFillImage == null)
            return;

        float hpRatio = Mathf.Clamp01(playerStats.currentHp / playerStats.maxHp);
        hpFillImage.fillAmount = hpRatio;

        if (hpText != null)
            hpText.text = $"{(int)playerStats.currentHp} / {(int)playerStats.maxHp}";
    }

    /// <summary>
    /// HP UI 갱신
    /// </summary>
    public void UpdateHPUI(float currentHp, float maxHp)
    {
        if (hpFillImage != null)
            hpFillImage.fillAmount = Mathf.Clamp01(currentHp / maxHp); //Mathf.Clamp01는 0~1 사이로 값 제한하는 함수

        if (hpText != null)
            hpText.text = $"{(int)currentHp} / {(int)maxHp}";
    }

    /// <summary>
    /// 경험치 UI 갱신
    /// </summary>
    public void UpdateExpUI(float currentExp, float nextExp)
    {
        if (expFillImage != null)
            expFillImage.fillAmount = Mathf.Clamp01(currentExp / nextExp);

        if (levelText != null && playerInventory != null)
            levelText.text = $"Lv {playerInventory.level}";
    }

    /// <summary>
    /// 코인 UI 갱신
    /// </summary>
    public void UpdateCoinUI(int currentGold)
    {
        if (coinText != null)
            coinText.text = $"Coins: {currentGold}";
    }
}
