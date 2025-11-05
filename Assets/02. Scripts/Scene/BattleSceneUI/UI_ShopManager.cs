using UnityEngine;
using System.Collections;

public class UI_ShopManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private StatSelectionUI statSelectionUI;
    [SerializeField] private UI_Shop shopUI;
    [SerializeField] private UI_GameWave gameWaveUI;
    [SerializeField] private GameObject topLeftUI;   // HP, 코인, 레벨 표시 영역
    [SerializeField] private GameObject topCenterUI; // 타이머, 웨이브 표시 영역

    private PlayerInventory playerInventory;
    private int lastWaveNumber = 1; // 현재 웨이브 기억용

    private void Start()
    {
        playerInventory = FindObjectOfType<PlayerInventory>();

        if (statSelectionUI != null)
            statSelectionUI.gameObject.SetActive(false);

        if (shopUI != null)
            shopUI.gameObject.SetActive(false);
    }

    // -------------------------------------------------------
    // 웨이브 종료 시 호출됨 (UI_GameWave에서 waveNumber 인자로 전달)
    // -------------------------------------------------------
    public void OnWaveEnd(int waveNumber)
    {
        Debug.Log($"[ShopManager] Wave {waveNumber} ended - Opening StatSelectionUI");

        lastWaveNumber = waveNumber;

        if (statSelectionUI == null || playerInventory == null)
            return;

        statSelectionUI.gameObject.SetActive(true);
        statSelectionUI.Open(playerInventory.waveLevelUpCount, OnStatSelectionComplete);
    }

    // -------------------------------------------------------
    // 스탯 선택 완료 시 상점 열기
    // -------------------------------------------------------
    private void OnStatSelectionComplete()
    {
        Debug.Log("[ShopManager] Stat selection complete - Opening ShopUI");

        if (statSelectionUI != null)
            statSelectionUI.gameObject.SetActive(false);

        if (shopUI != null)
        {
            // 상점 열 때 웨이브 정보 전달
            shopUI.OpenShop(lastWaveNumber);

            // 상점 열릴 동안 게임 일시정지
            Time.timeScale = 0f;
        }

        playerInventory.ResetWaveLevelUpCount();

        if (topLeftUI != null)
            topLeftUI.SetActive(false);
        if (topCenterUI != null)
            topCenterUI.SetActive(false);
    }

    // -------------------------------------------------------
    // Go 버튼 클릭 시 다음 웨이브 시작
    // -------------------------------------------------------
    public void OnGoNextWave()
    {
        Debug.Log("[ShopManager] Go button clicked - Starting next wave");

        if (shopUI != null)
            shopUI.gameObject.SetActive(false);

        if (topLeftUI != null)
            topLeftUI.SetActive(true);
        if (topCenterUI != null)
            topCenterUI.SetActive(true);

        Time.timeScale = 1f;
        StartCoroutine(StartNextWaveDelayed());
    }

    private IEnumerator StartNextWaveDelayed()
    {
        yield return null; // 한 프레임 대기 후 다음 웨이브 시작

        if (gameWaveUI != null)
        {
            gameWaveUI.StartWave();
        }
    }
}
