using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [Header("Player EXP Settings")]
    public int level = 0;
    public float currentExp = 0f;
    public float expToNextLevel = 16f;

    [Header("Player Gold")]
    public int gold = 0; // ���� �����

    [Header("Wave LevelUp Counter")]
    public int waveLevelUpCount = 0; // �̹� ���̺� ���� ������ Ƚ��

    [SerializeField] private UI_PlayerStatus uiPlayerStatus;

    public void AddReward(int coin, float exp)
    {
        gold += coin; // ���� ����
        AddExperience(exp);
        uiPlayerStatus?.UpdateCoinUI(gold); // ���� UI ��� �ݿ�
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
