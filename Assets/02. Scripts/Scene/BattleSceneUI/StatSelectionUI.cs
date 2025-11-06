using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// [StatSelectionUI]
/// ------------------------------------------------------------
/// 스탯 선택 UI를 관리하는 스크립트
/// - 웨이브 종료 후 플레이어가 스탯 보상을 선택할 수 있게 함
/// - 무작위로 선택된 스탯 옵션을 표시하고 클릭 시 적용
/// ------------------------------------------------------------
/// </summary>
public class StatSelectionUI : MonoBehaviour
{
    [Header("UI 구성요소")]
    [SerializeField] private GameObject selectionPanel;     // 스탯 선택 패널
    [SerializeField] private Button[] statButtons;          // 선택 버튼들
    [SerializeField] private TMP_Text[] statNameTexts;      // 스탯 이름 텍스트
    [SerializeField] private TMP_Text[] statDescTexts;      // 스탯 설명 텍스트

    [Header("참조")]
    [SerializeField] private PlayerStats playerStats;       // 플레이어 스탯
    [SerializeField] private PlayerInventory playerInventory; // 인벤토리 참조 (선택 종료 시 연동용)

    private List<StatOption> allStatOptions = new();        // 모든 스탯 목록
    private List<StatOption> currentChoices = new();        // 현재 표시 중인 선택지

    private int remainingSelections = 0;                    // 남은 선택 횟수
    private System.Action onCompleteCallback;               // 선택 완료 시 실행할 콜백

    private void OnEnable()
    {
        // 스탯 목록이 비어있다면 초기화
        if (allStatOptions == null || allStatOptions.Count == 0)
            InitializeStatOptions();
    }

    /// <summary>
    /// 스탯 옵션 초기화
    /// </summary>
    private void InitializeStatOptions()
    {
        allStatOptions.Clear();

        // 기본 스탯 상승 옵션
        allStatOptions.Add(new StatOption("근력운동", "공격력 +2", (p) => p.currentDamage += 2f));
        allStatOptions.Add(new StatOption("KBO 시청", "사거리 +1", (p) => p.currentRange += 1f));
        allStatOptions.Add(new StatOption("핫식스 섭취", "공격속도 x1.15", (p) => p.currentAttackSpeed *= 1.15f));
        allStatOptions.Add(new StatOption("하체운동", "이동속도 x1.1", (p) => p.currentMoveSpeed *= 1.1f));
        allStatOptions.Add(new StatOption("숙면", "최대체력 +5", (p) =>
        {
            p.maxHp += 5f;
            p.currentHp = Mathf.Min(p.currentHp + 5f, p.maxHp);
        }));

        // 도박(랜덤 변동) 옵션
        allStatOptions.Add(new StatOption("공격력 도박", "공격력이 -3~+5 범위에서 랜덤하게 변동된다.", (p) =>
        {
            float randomChange = Random.Range(-3f, 5f);
            p.currentDamage += randomChange;
        }));

        allStatOptions.Add(new StatOption("사거리 도박", "사거리가 -2~+3 범위에서 랜덤하게 변동된다.", (p) =>
        {
            float randomChange = Random.Range(-2f, 3f);
            p.currentRange += randomChange;
        }));

        allStatOptions.Add(new StatOption("최대체력 도박", "최대체력이 -5~+10 범위에서 랜덤하게 변동된다.", (p) =>
        {
            float randomChange = Random.Range(-5f, 10f);
            p.maxHp = Mathf.Max(1f, p.maxHp + randomChange);
            p.currentHp = Mathf.Min(p.currentHp, p.maxHp);
        }));
    }

    // ShopManager에서 호출하는 버전
    public void Open(int selectionCount, System.Action onComplete)
    {
        onCompleteCallback = onComplete;
        Open(selectionCount);
    }

    /// <summary>
    /// 스탯 선택창 열기
    /// </summary>
    public void Open(int selectionCount)
    {
        remainingSelections = selectionCount;

        if (remainingSelections <= 0)
        {
            Close();
            return;
        }

        selectionPanel.SetActive(true);
        Time.timeScale = 0f; // 일시정지
        PickRandomStats();
    }

    /// <summary>
    /// 무작위로 스탯 옵션을 추출하여 버튼에 표시
    /// </summary>
    private void PickRandomStats()
    {
        int countToDisplay = Mathf.Min(statButtons.Length, allStatOptions.Count);

        // 무작위 셔플 후 일부 선택
        currentChoices = allStatOptions
            .OrderBy(_ => Random.value)
            .Take(countToDisplay)
            .ToList();

        for (int i = 0; i < statButtons.Length; i++)
        {
            if (i < currentChoices.Count)
            {
                var option = currentChoices[i];

                statNameTexts[i].text = option.statName;
                statDescTexts[i].text = option.description;

                statButtons[i].onClick.RemoveAllListeners();
                statButtons[i].onClick.AddListener(() => SelectStat(option));

                statButtons[i].gameObject.SetActive(true);
            }
            else
            {
                statButtons[i].gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// 스탯 선택 시 적용 처리
    /// </summary>
    private void SelectStat(StatOption selectedStat)
    {
        if (selectedStat == null || playerStats == null)
            return;

        selectedStat.applyEffect?.Invoke(playerStats);
        remainingSelections--;

        if (remainingSelections > 0)
        {
            PickRandomStats();
        }
        else
        {
            Close();
        }
    }

    /// <summary>
    /// 스탯 선택창 닫기
    /// </summary>
    public void Close()
    {
        selectionPanel.SetActive(false);
        Time.timeScale = 1f; // 시간 재개

        onCompleteCallback?.Invoke();
        onCompleteCallback = null;
    }
}
