using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class StatSelectionUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject selectionPanel;
    [SerializeField] private Button[] statButtons;
    [SerializeField] private TMP_Text[] statNameTexts;
    [SerializeField] private TMP_Text[] statDescTexts;

    [Header("References")]
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private PlayerInventory playerInventory;

    private List<StatOption> allStats = new List<StatOption>();
    private List<StatOption> currentOptions = new List<StatOption>();

    private int remainingSelections = 0;
    private System.Action onCompleteCallback;

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

        // add your stat options here
        allStats.Add(new StatOption
        {
            statName = "근력운동",
            description = "공격력 +2",
            applyEffect = (p) => p.currentDamage += 2f
        });

        allStats.Add(new StatOption
        {
            statName = "KBO 시청",
            description = "사거리 +1",
            applyEffect = (p) => p.currentRange += 1f
        });

        allStats.Add(new StatOption
        {
            statName = "핫식스 섭취",
            description = "공격속도x1.15",
            applyEffect = (p) => p.currentAttackSpeed *= 1.15f
        });

        allStats.Add(new StatOption
        {
            statName = "하체운동",
            description = "이동속도 x1.1",
            applyEffect = (p) => p.currentMoveSpeed *= 1.1f
        });

        allStats.Add(new StatOption
        {
            statName = "숙면",
            description = "최대체력 +5",
            applyEffect = (p) =>
            {
                p.maxHp += 5f;
                p.currentHp += 5f;
            }
        });

        allStats.Add(new StatOption
        {
            statName = "공격력 도박",
            description = "공격력이 -5~+5 범위에서 랜덤하게 변동된다.",
            applyEffect = (p) =>
            {
                float randomChange = Random.Range(-5f, 5f);
                p.currentDamage += randomChange;
                Debug.Log($"[공격력 도박] 공격력 변화: {randomChange:+0.0;-0.0} → 현재 공격력: {p.currentDamage:0.0}");
            }
        });
        allStats.Add(new StatOption
        {
            statName = "사거리 도박",
            description = "사거리가 -3~+3 범위에서 랜덤하게 변동된다.",
            applyEffect = (p) =>
            {
                float randomChange = Random.Range(-3f, 3f);
                p.currentDamage += randomChange;
                Debug.Log($"[사거리 도박] 사거리 변화: {randomChange:+0.0;-0.0} → 현재 사거리: {p.currentDamage:0.0}");
            }
        });
        allStats.Add(new StatOption
        {
            statName = "최대체력 도박",
            description = "체력이 -5~+10 범위에서 랜덤하게 변동된다.",
            applyEffect = (p) =>
            {
                float randomChange = Random.Range(-5f, 10f);
                p.currentDamage += randomChange;
                Debug.Log($"[최대체력 도박] 최대체력 변화: {randomChange:+0.0;-0.0} → 현재 최대체력: {p.currentDamage:0.0}");
            }
        });
    }

    // this is the version that ShopManager will call
    public void Open(int selectionCount, System.Action onComplete)
    {
        onCompleteCallback = onComplete;
        Open(selectionCount);
    }

    // original version (can still be used)
    public void Open(int selectionCount)
    {
        remainingSelections = selectionCount;

        if (remainingSelections <= 0)
        {
            // no stat to choose, just close
            Close();
            return;
        }

        selectionPanel.SetActive(true);
        Time.timeScale = 0f;

        PickRandomStats();
    }

    private void PickRandomStats()
    {
        // make sure we have enough stats
        int countToTake = Mathf.Min(statButtons.Length, allStats.Count);

        // shuffle and take
        currentOptions = allStats
            .OrderBy(x => Random.value)
            .Take(countToTake)
            .ToList();

        for (int i = 0; i < statButtons.Length; i++)
        {
            if (i < currentOptions.Count)
            {
                StatOption opt = currentOptions[i];

                statNameTexts[i].text = opt.statName;
                statDescTexts[i].text = opt.description;

                int index = i;
                statButtons[i].onClick.RemoveAllListeners();
                statButtons[i].onClick.AddListener(() => SelectStat(currentOptions[index]));

                statButtons[i].gameObject.SetActive(true);
            }
            else
            {
                // hide extra buttons
                statButtons[i].gameObject.SetActive(false);
            }
        }
    }

    private void SelectStat(StatOption selectedStat)
    {
        if (selectedStat != null && selectedStat.applyEffect != null && playerStats != null)
        {
            selectedStat.applyEffect.Invoke(playerStats);
        }

        remainingSelections--;

        if (remainingSelections > 0)
        {
            // show next stat choices
            PickRandomStats();
        }
        else
        {
            Close();
        }
    }

    public void Close()
    {
        selectionPanel.SetActive(false);
        Time.timeScale = 1f;

        if (playerInventory != null)
        {
            // we were calling this before, but ShopManager also resets it
            // you can remove this line if you want only ShopManager to handle it
            // playerInventory.ResetWaveLevelUpCount();
        }

        if (onCompleteCallback != null)
        {
            onCompleteCallback.Invoke();
            onCompleteCallback = null;
        }
    }
}
