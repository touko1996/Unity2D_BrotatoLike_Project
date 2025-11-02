using UnityEngine;

public class UI_ShopManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private StatSelectionUI statSelectionUI;
    [SerializeField] private UI_Shop shopUI;
    [SerializeField] private UI_GameWave gameWaveUI;
    [SerializeField] private GameObject topLeftUI;   // HP, 코인, 레벨 표시 영역
    [SerializeField] private GameObject topCenterUI; // 타이머, 웨이브 표시 영역

    private PlayerInventory playerInventory;

    private void Start()
    {
        playerInventory = FindObjectOfType<PlayerInventory>();

        if (statSelectionUI != null)
            statSelectionUI.gameObject.SetActive(false);

        if (shopUI != null)
            shopUI.gameObject.SetActive(false);
    }

    // called when a wave ends
    public void OnWaveEnd()
    {
        Debug.Log("Wave ended - Opening StatSelectionUI");

        if (statSelectionUI == null || playerInventory == null)
            return;

        statSelectionUI.gameObject.SetActive(true);
        statSelectionUI.Open(playerInventory.waveLevelUpCount, OnStatSelectionComplete);
    }

    // called when stat selection is complete
    private void OnStatSelectionComplete()
    {
        Debug.Log("Stat selection complete - Opening ShopUI");

        if (statSelectionUI != null)
            statSelectionUI.gameObject.SetActive(false);

        if (shopUI != null)
        {
            shopUI.gameObject.SetActive(true);

            // pause the game while shop is open
            Time.timeScale = 0f;
        }

        playerInventory.ResetWaveLevelUpCount();

        if (topLeftUI != null)
            topLeftUI.SetActive(false);
        if (topCenterUI != null)
            topCenterUI.SetActive(false);

        playerInventory.ResetWaveLevelUpCount();
    }

    // called when Go button is pressed in shop
    public void OnGoNextWave()
    {
        Debug.Log("Go button clicked - Starting next wave");

        if (shopUI != null)
            shopUI.gameObject.SetActive(false);

        // 다시 활성화
        if (topLeftUI != null)
            topLeftUI.SetActive(true);
        if (topCenterUI != null)
            topCenterUI.SetActive(true);

        Time.timeScale = 1f;
        StartCoroutine(StartNextWaveDelayed());
    }

    private System.Collections.IEnumerator StartNextWaveDelayed()
    {
        yield return null; // wait one frame
        if (gameWaveUI != null)
        {
            gameWaveUI.StartWave();
        }
    }
}
