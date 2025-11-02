using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_GameWave : MonoBehaviour
{
    [Header("Wave Timer Settings")]
    [SerializeField] private float waveDuration = 30f;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text waveText;

    private float remainingTime;
    private bool isWaveActive = false;
    private int currentWave = 1;

    private PlayerInventory playerInventory;

    private void Start()
    {
        playerInventory = FindObjectOfType<PlayerInventory>();
        StartWave(); //게임 시작 시 첫 웨이브 자동 시작
    }

    private void Update()
    {
        if (!isWaveActive)
            return;

        remainingTime -= Time.deltaTime;

        if (remainingTime <= 0f)
        {
            EndWave();
            return;
        }

        UpdateUI();
    }

    private void UpdateUI()
    {
        if (timerText != null)
            timerText.text = "Time: " + Mathf.CeilToInt(remainingTime).ToString();

        if (waveText != null)
            waveText.text = "Wave " + currentWave.ToString();
    }

    public void StartWave()
    {
        Debug.Log("Wave started: " + currentWave);

        isWaveActive = true;        // 반드시 true로 바꿔줘야 Update가 돌기 시작함
        remainingTime = waveDuration;

        UpdateUI();
    }

    private void EndWave()
    {
        Debug.Log("Wave ended: " + currentWave);

        isWaveActive = false;
        remainingTime = 0f;
        UpdateUI();

        UI_ShopManager shopManager = FindObjectOfType<UI_ShopManager>();
        if (shopManager != null)
            shopManager.OnWaveEnd();

        currentWave++;
    }
}
