using UnityEngine;
using System.Collections;

/// <summary>
/// [UI_ShopManager]
/// ------------------------------------------------------------
/// 웨이브 종료 후 등장하는 상점/스탯선택 UI의 전환을 총괄 관리.
/// - 웨이브 종료 → 스탯 선택 → 상점 오픈 → 다음 웨이브 시작 순서로 진행.
/// - UI 활성화/비활성 및 게임 일시정지(TimeScale) 제어.
/// ------------------------------------------------------------
/// </summary>
public class UI_ShopManager : MonoBehaviour
{
    [Header("UI 참조")]
    [SerializeField] private StatSelectionUI statSelectionUI; // 스탯 선택 UI
    [SerializeField] private UI_Shop shopUI;                  // 상점 UI
    [SerializeField] private UI_GameWave gameWaveUI;          // 웨이브 관리 UI
    [SerializeField] private GameObject topLeftUI;            // HP/코인/레벨 영역
    [SerializeField] private GameObject topCenterUI;          // 타이머/웨이브 영역

    private PlayerInventory playerInventory;                  // 플레이어 인벤토리
    private int currentWaveNumber = 1;                        // 현재 웨이브 번호 저장

    // 초기화
    private void Start()
    {
        playerInventory = FindObjectOfType<PlayerInventory>();

        // UI 초기 비활성화 (게임 시작 시 표시되지 않도록)
        statSelectionUI?.gameObject.SetActive(false);
        shopUI?.gameObject.SetActive(false);
    }

    
    // 웨이브 종료 시 (UI_GameWave에서 호출)
    public void OnWaveEnd(int waveNumber)
    {
        currentWaveNumber = waveNumber;

        if (statSelectionUI == null || playerInventory == null)
            return;

        // 스탯 선택 UI 표시
        statSelectionUI.gameObject.SetActive(true);

        // 스탯 선택 시작 (선택 완료 후 OnStatSelectionComplete 실행)
        statSelectionUI.Open(playerInventory.waveLevelUpCount, OnStatSelectionComplete);
    }

    
    // 스탯 선택 완료 시 → 상점 오픈
    private void OnStatSelectionComplete()
    {
        // 스탯 선택창 닫기
        statSelectionUI?.gameObject.SetActive(false);

        // 상점 UI 오픈
        if (shopUI != null)
        {
            shopUI.OpenShop(currentWaveNumber);

            // 상점이 열려 있는 동안 시간 정지
            Time.timeScale = 0f;
        }

        // 이번 웨이브 동안의 레벨업 카운트 초기화
        playerInventory?.ResetWaveLevelUpCount();

        // 전투 HUD (HP, 타이머 등) 숨기기
        topLeftUI?.SetActive(false);
        topCenterUI?.SetActive(false);
    }

    
    // [GO] 버튼 클릭 시 → 다음 웨이브로 이동
    public void OnGoNextWave()
    {
        // 상점 닫기
        shopUI?.gameObject.SetActive(false);

        // HUD 다시 표시
        topLeftUI?.SetActive(true);
        topCenterUI?.SetActive(true);

        // 시간 정상화 후 다음 웨이브 시작 코루틴 실행
        Time.timeScale = 1f;
        StartCoroutine(StartNextWaveRoutine());
    }

    
    // 다음 웨이브 시작 (1프레임 대기 후 실행)
    private IEnumerator StartNextWaveRoutine()
    {
        yield return null; // 1프레임 대기 (UI 전환 안정화용)

        gameWaveUI?.StartWave();
    }
}
