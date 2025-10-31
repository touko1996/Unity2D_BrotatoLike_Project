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

    private int remainingSelections = 0; // ���� ���� Ƚ��

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

        allStats.Add(new StatOption { statName = "���ݷ� ����", description = "���ݷ��� 2 �����մϴ�.", applyEffect = (p) => p.currentDamage += 2f });
        allStats.Add(new StatOption { statName = "��Ÿ� ����", description = "���� ��Ÿ��� 1 �����մϴ�.", applyEffect = (p) => p.currentRange += 1f });
        allStats.Add(new StatOption { statName = "���ݼӵ� ����", description = "���ݼӵ��� 15% �����մϴ�.", applyEffect = (p) => p.currentAttackSpeed *= 1.15f });
        allStats.Add(new StatOption { statName = "�̵��ӵ� ����", description = "�̵��ӵ��� 10% �����մϴ�.", applyEffect = (p) => p.currentMoveSpeed *= 1.1f });
        allStats.Add(new StatOption { statName = "�ִ�ü�� ����", description = "�ִ� ü���� 5 �����մϴ�.", applyEffect = (p) => { p.maxHp += 5f; p.currentHp += 5f; } });
        allStats.Add(new StatOption { statName = "��� ȸ��", description = "ü���� 5 ȸ���մϴ�.", applyEffect = (p) => p.Heal(5f) });
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
            // ���� ���� �������� �Ѿ��
            PickRandomStats();
        }
        else
        {
            // ��� ���� ����
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
