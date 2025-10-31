using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class StatSelectionUI : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject selectionPanel;
    public Button[] statButtons;
    public TMP_Text[] statNameTexts;
    public TMP_Text[] statDescTexts;

    [Header("References")]
    public PlayerStats playerStats;
    public PlayerInventory playerInventory;
    public GameObject playerStatsPanel;

    private List<StatOption> allStats = new List<StatOption>();
    private List<StatOption> currentOptions = new List<StatOption>();

    private int remainingSelections = 0; // 남은 선택 횟수

    private void OnEnable()
    {
        if (allStats == null || allStats.Count == 0)
        {
            InitializeStatOptions();
        }
    }

    private void InitializeStatOptions()
    {
        allStats.Clear();

        allStats.Add(new StatOption { statName = "공격력 증가", description = "공격력이 2 증가합니다.", applyEffect = (p) => p.currentDamage += 2f });
        allStats.Add(new StatOption { statName = "사거리 증가", description = "공격 사거리가 1 증가합니다.", applyEffect = (p) => p.currentRange += 1f });
        allStats.Add(new StatOption { statName = "공격속도 증가", description = "공격속도가 15% 증가합니다.", applyEffect = (p) => p.currentAttackSpeed *= 1.15f });
        allStats.Add(new StatOption { statName = "이동속도 증가", description = "이동속도가 10% 증가합니다.", applyEffect = (p) => p.currentMoveSpeed *= 1.1f });
        allStats.Add(new StatOption { statName = "최대체력 증가", description = "최대 체력이 5 증가합니다.", applyEffect = (p) => { p.maxHp += 5f; p.currentHp += 5f; } });
        allStats.Add(new StatOption { statName = "즉시 회복", description = "체력을 5 회복합니다.", applyEffect = (p) => p.Heal(5f) });
    }

    public void Open(int count)
    {
        remainingSelections = count;
        selectionPanel.SetActive(true);
        if (playerStatsPanel != null)
            playerStatsPanel.SetActive(true);
        Time.timeScale = 0f;
        PickRandomStats();
    }

    private void PickRandomStats()
    {
        currentOptions = allStats.OrderBy(x => Random.value).Take(4).ToList();

        for (int i = 0; i < statButtons.Length; i++)
        {
            statNameTexts[i].text = currentOptions[i].statName;
            statDescTexts[i].text = currentOptions[i].description;

            int index = i;
            statButtons[i].onClick.RemoveAllListeners();
            statButtons[i].onClick.AddListener(() => SelectStat(currentOptions[index]));
        }
    }

    private void SelectStat(StatOption selectedStat)
    {
        selectedStat.applyEffect.Invoke(playerStats);
        remainingSelections--;

        if (remainingSelections > 0)
        {
            // 다음 스탯 선택으로 넘어가기
            PickRandomStats();
        }
        else
        {
            // 모든 선택 끝남
            Close();
        }
    }

    public void Close()
    {
        selectionPanel.SetActive(false);
        if (playerStatsPanel != null)
            playerStatsPanel.SetActive(false);
        Time.timeScale = 1f;
        playerInventory.ResetWaveLevelUpCount();
    }
}
