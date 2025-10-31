using UnityEngine;
using TMPro;

public class UI_GameWave : MonoBehaviour
{
    [Header("UI Reference")]
    public TMP_Text waveText;
    public TMP_Text timerText;

    [Header("Wave Settings")]
    public int currentWave = 1;
    public float waveDuration = 30f;
    private float remainingTime;
    private bool isWaveActive = false;

    [Header("References")]
    public PlayerInventory playerInventory;
    public StatSelectionUI statSelectionUI;

    private void Start()
    {
        StartWave();
    }

    private void Update()
    {
        Debug.Log($"[WaveTimer] isWaveActive={isWaveActive}, remainingTime={remainingTime:F2}");
        if (!isWaveActive) return;

        remainingTime -= Time.deltaTime;

        if (remainingTime <= 0f)
        {
            Debug.Log("[WaveTimer] EndWave() 호출됨");
            remainingTime = 0f;
            EndWave();
        }

        UpdateTimerUI();
    }


    public void StartWave()
    {
        isWaveActive = true;
        remainingTime = waveDuration;
        UpdateWaveUI();
        playerInventory.ResetWaveLevelUpCount();
    }

    public void EndWave()
    {
        isWaveActive = false;
        Debug.Log("Wave End");

        int selections = playerInventory.waveLevelUpCount;

        if (selections > 0 && statSelectionUI != null)
        {
            statSelectionUI.gameObject.SetActive(true);
            statSelectionUI.Open(selections);
        }
        else
        {
            Debug.Log("No level-up this wave. Proceed to next phase.");
            // 나중에 상점 UI로 넘어가도 됨
        }
    }

    private void UpdateWaveUI()
    {
        if (waveText != null)
            waveText.text = "WAVE " + currentWave;
    }

    private void UpdateTimerUI()
    {
        if (timerText != null)
            timerText.text = Mathf.CeilToInt(remainingTime).ToString();
    }
}
