using System.Collections;
using UnityEngine;
using TMPro;

/// <summary>
/// [UI_GameWave]
/// ------------------------------------------------------------
/// 웨이브(전투 라운드)의 흐름을 관리하는 UI 컨트롤러
/// - 웨이브 타이머 표시 및 갱신
/// - 몬스터 스폰 제어 및 종료 처리
/// - 코인 흡수 완료 후 상점/스탯 선택 UI 전환
/// - 웨이브 종료 시 플레이어 HP 회복
/// ------------------------------------------------------------
/// </summary>
public class UI_GameWave : MonoBehaviour
{
    [Header("웨이브 타이머 설정")]
    [SerializeField] private float waveDuration = 30f;   // 웨이브 지속 시간
    [SerializeField] private TMP_Text timerText;         // 남은 시간 표시
    [SerializeField] private TMP_Text waveText;          // 현재 웨이브 표시

    private float remainingTime;                         // 남은 시간
    private bool isWaveActive = false;                   // 웨이브 진행 중 여부
    private int currentWave = 1;                         // 현재 웨이브 번호

    private PlayerInventory playerInventory;             // 플레이어 인벤토리
    private float coinRemainder = 0f;                    // 코인 소수점 누적
    private bool spawnStopped = false;                   // 스폰 중단 여부

    private void Start()
    {
        playerInventory = FindObjectOfType<PlayerInventory>();
        StartWave();
    }

    private void Update()
    {
        if (!isWaveActive) return;

        remainingTime -= Time.deltaTime;

        // 종료 2초 전 스폰 조기 중단 
        if (!spawnStopped && remainingTime <= 2f)
        {
            FindObjectOfType<MonsterSpawner>()?.StopSpawningEarly();
            spawnStopped = true;
        }

        // 웨이브 종료 조건
        if (remainingTime <= 0f)
        {
            EndWave();
            return;
        }

        UpdateUI();
    }

    /// <summary>
    /// 남은 시간과 웨이브 표시 갱신
    /// </summary>
    private void UpdateUI()
    {
        if (timerText != null)
            timerText.text = Mathf.CeilToInt(remainingTime).ToString(); //Mathf.CeilToInt은 소수점을 올림 처리해서 정수로 변환하는 함수

        if (waveText != null)
            waveText.text = $"Wave {currentWave}";
    }

    /// <summary>
    /// 웨이브 시작 처리
    /// </summary>
    public void StartWave()
    {

        // 몬스터 스폰 시작
        FindObjectOfType<MonsterSpawner>()?.SetWave(currentWave);

        isWaveActive = true;
        spawnStopped = false;
        remainingTime = waveDuration;
        UpdateUI();

        // 보스 웨이브일 경우 보스 HP UI 표시
        UI_BossHP bossUI = FindObjectOfType<UI_BossHP>(true);
        if (currentWave == 10)
        {
            bossUI?.gameObject.SetActive(true);
        }
        else
        {
            bossUI?.Hide();
        }
    }

    /// <summary>
    /// 웨이브 종료 처리
    /// </summary>
    private void EndWave()
    {

        isWaveActive = false;
        remainingTime = 0f;

        // 남은 몬스터 페이드아웃 후 비활성화
        foreach (Monster monster in FindObjectsOfType<Monster>())
        {
            if (monster != null)
                StartCoroutine(FadeOutAndDisable(monster));
        }

        // 필드의 코인 자동 흡수
        StartCoroutine(AbsorbAllCoins());

        // 플레이어 체력 전부 회복
        PlayerStats playerStats = FindObjectOfType<PlayerStats>();
        if (playerStats != null)
        {
            playerStats.currentHp = playerStats.maxHp;
        }

        UpdateUI();
    }

    /// <summary>
    /// 몬스터가 서서히 사라지는 연출
    /// </summary>
    private IEnumerator FadeOutAndDisable(Monster monster)
    {
        SpriteRenderer monsterSr = monster.GetComponent<SpriteRenderer>();
        if (monsterSr == null)
        {
            monster.gameObject.SetActive(false);
            yield break;
        }

        Color startColor = monsterSr.color;
        float fadeDuration = 0.8f;
        float timer = 0f;

        while (timer < fadeDuration)
        {
            if (monsterSr == null) yield break;

            float t = timer / fadeDuration;
            monsterSr.color = new Color(startColor.r, startColor.g, startColor.b, Mathf.Lerp(1f, 0f, t));
            timer += Time.deltaTime;
            yield return null;
        }

        monsterSr.color = new Color(startColor.r, startColor.g, startColor.b, 0f);
        monster.gameObject.SetActive(false);
    }

    /// <summary>
    /// 모든 코인을 플레이어에게 자동 흡수시킴
    /// </summary>
    private IEnumerator AbsorbAllCoins()
    {
        PlayerInventory playerInven = FindObjectOfType<PlayerInventory>();
        Transform player = playerInven?.transform;
        if (player == null) yield break;

        DropItem[] coins = FindObjectsOfType<DropItem>();

        // 코인이 하나도 없을 경우 약간의 지연 후 상점 열기
        if (coins.Length == 0)
        {
            yield return new WaitForSecondsRealtime(0.5f);
            OpenShopAfterAbsorption();
            yield break;
        }

        int totalCoins = coins.Length;
        int absorbedCount = 0;

        // 모든 코인에 대해 자석 흡수 효과 적용
        foreach (DropItem coin in coins)
        {
            if (coin == null) continue;

            coin.SetMagnetAbsorbed();

            StartCoroutine(MoveCoinToPlayer(coin, player, playerInven, () =>
            {
                absorbedCount++;
            }));
        }

        // 모든 코인이 흡수될 때까지 대기
        while (absorbedCount < totalCoins)
            yield return null;

        yield return new WaitForSecondsRealtime(0.5f);
        OpenShopAfterAbsorption();
    }

    /// <summary>
    /// 코인을 플레이어 위치로 이동시키는 코루틴
    /// </summary>
    private IEnumerator MoveCoinToPlayer(DropItem coin, Transform player, PlayerInventory playerInven, System.Action onAbsorbed)
    {
        float moveSpeed = 15f;
        float rotateSpeed = 500f; 

        while (coin != null && Vector2.Distance(coin.transform.position, player.position) > 0.2f)
        {
            if (player == null) yield break;

            // 코인 이동 및 회전
            coin.transform.position = Vector2.MoveTowards(coin.transform.position, player.position, moveSpeed * Time.deltaTime);
            coin.transform.Rotate(Vector3.forward * rotateSpeed * Time.deltaTime);
            

            yield return null;
        }

        // 코인 흡수 완료
        if (coin != null)
        {
            coinRemainder += 0.5f;

            // 1 골드 단위로 반올림 처리
            if (coinRemainder >= 1f)
            {
                int addGold = Mathf.FloorToInt(coinRemainder); //Mathf.FloorToInt는 소수점을 내림 처리해서 정수로 변환하는 함수
                playerInven.gold += addGold;
                coinRemainder -= addGold;
            }

            AudioManager.Instance?.PlayCoinSFX();
            Destroy(coin.gameObject);
        }

        onAbsorbed?.Invoke();
    }

    /// <summary>
    /// 코인 흡수 완료 후 상점 UI 호출
    /// </summary>
    private void OpenShopAfterAbsorption()
    {
        StartCoroutine(OpenShopDelayed());
    }

    /// <summary>
    /// 상점 열기 전 약간의 지연 추가
    /// </summary>
    private IEnumerator OpenShopDelayed()
    {
        yield return new WaitForSecondsRealtime(0.3f);

        UI_ShopManager shopManager = FindObjectOfType<UI_ShopManager>();
        if (shopManager != null)
            shopManager.OnWaveEnd(currentWave);

        currentWave++;
    }

    public int GetCurrentWave() => currentWave;

    public void ForceEndWave() => EndWave();
}
