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
        StartWave(); // 게임 시작 시 첫 웨이브 자동 시작
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
            timerText.text = Mathf.CeilToInt(remainingTime).ToString();

        if (waveText != null)
            waveText.text = "Wave " + currentWave.ToString();
    }

    public void StartWave()
    {
        Debug.Log("Wave started: " + currentWave);
        FindObjectOfType<MonsterSpawner>()?.SetWave(currentWave);

        isWaveActive = true;
        remainingTime = waveDuration;

        UpdateUI();
    }

    private void EndWave()
    {
        Debug.Log("Wave ended: " + currentWave);
        isWaveActive = false;
        remainingTime = 0f;

        // 몬스터 스폰 중단
        FindObjectOfType<MonsterSpawner>()?.StopSpawning();

        // 플레이어 체력 풀 회복
        PlayerStats playerStats = FindObjectOfType<PlayerStats>();
        if (playerStats != null)
        {
            playerStats.currentHp = playerStats.maxHp;
            Debug.Log("[Wave End] HP recovered");
        }

        UpdateUI();

        // 상점 열기 (현재 웨이브 전달)
        UI_ShopManager shopManager = FindObjectOfType<UI_ShopManager>();
        if (shopManager != null)
        {
            shopManager.OnWaveEnd(currentWave);
        }

        currentWave++;
    }

    public int GetCurrentWave()
    {
        return currentWave;
    }
}
