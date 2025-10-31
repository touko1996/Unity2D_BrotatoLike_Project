using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [Header("Player EXP Settings")]
    public int level = 0;
    public float currentExp = 0f;
    public float expToNextLevel = 16f;

    [Header("Player Gold")]
    public int gold = 0; // 코인 저장용

    [Header("Wave LevelUp Counter")]
    public int waveLevelUpCount = 0; // 이번 웨이브 동안 레벨업 횟수

    [SerializeField] private UI_PlayerStatus uiPlayerStatus;

    public void AddReward(int coin, float exp)
    {
        gold += coin; // 코인 증가
        AddExperience(exp);
        uiPlayerStatus?.UpdateCoinUI(gold); // 코인 UI 즉시 반영
    }

    public void AddExperience(float exp)
    {
        currentExp += exp;
        Debug.Log($"[EXP] +{exp} | Total: {currentExp}/{expToNextLevel}");

        if (currentExp >= expToNextLevel)
        {
            LevelUp();
        }

        uiPlayerStatus?.UpdateExpUI(currentExp, expToNextLevel);
    }

    private void LevelUp()
    {
        level++;
        currentExp = 0f;
        expToNextLevel = Mathf.Pow(level + 4, 2);
        waveLevelUpCount++;

        Debug.Log($"[LevelUp] Lv.{level} | Next EXP: {expToNextLevel}");
    }

    public void ResetWaveLevelUpCount()
    {
        waveLevelUpCount = 0;
    }
}
