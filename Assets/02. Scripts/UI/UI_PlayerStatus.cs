using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_PlayerStatus : MonoBehaviour
{
    [Header("References")]
    public PlayerStats playerStats;
    public PlayerInventory playerInventory;

    [Header("HP UI")]
    public Image hpFill;
    public TMP_Text hpText;

    [Header("EXP UI")]
    public Image expFill;
    public TMP_Text levelText;

    [Header("Coin UI")]
    public TMP_Text coinText;

    private void Start()
    {
        UpdateHPUI(playerStats.currentHp, playerStats.maxHp);
        UpdateExpUI(playerInventory.currentExp, playerInventory.expToNextLevel);
        UpdateCoinUI(playerInventory.gold);
    }

    private void Update()
    {
        // HP는 프레임마다 갱신
        if (playerStats != null && hpFill != null)
        {
            float hpRatio = playerStats.currentHp / playerStats.maxHp;
            hpFill.fillAmount = hpRatio;
            if (hpText != null)
                hpText.text = $"{(int)playerStats.currentHp} / {(int)playerStats.maxHp}";
        }
    }

    public void UpdateHPUI(float current, float max)
    {
        if (hpFill != null)
            hpFill.fillAmount = current / max;

        if (hpText != null)
            hpText.text = $"{(int)current} / {(int)max}";
    }

    public void UpdateExpUI(float currentExp, float nextExp)
    {
        if (expFill != null)
            expFill.fillAmount = currentExp / nextExp;

        if (levelText != null)
            levelText.text = $"Lv {playerInventory.level}";
    }

    public void UpdateCoinUI(int gold)
    {
        if (coinText != null)
            coinText.text = $"Coins: {gold}";
    }
}
