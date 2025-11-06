using System.Collections;
using UnityEngine;
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
    private float coinRemainder = 0f;
    private bool spawnStopped = false;

    private void Start()
    {
        playerInventory = FindObjectOfType<PlayerInventory>();
        StartWave();
    }

    private void Update()
    {
        if (!isWaveActive)
            return;

        remainingTime -= Time.deltaTime;

        if (!spawnStopped && remainingTime <= 2f)
        {
            FindObjectOfType<MonsterSpawner>()?.StopSpawningEarly();
            spawnStopped = true;
        }

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
        spawnStopped = false;
        remainingTime = waveDuration;
        UpdateUI();

        // wave 10 에서는 보스 HP UI 보여주기만 한다
        UI_BossHP bossUI = FindObjectOfType<UI_BossHP>(true);
        if (currentWave == 10)
        {
            if (bossUI != null)
                bossUI.gameObject.SetActive(true);
        }
        else
        {
            if (bossUI != null)
                bossUI.Hide();
        }
    }

    private void EndWave()
    {
        Debug.Log("Wave ended: " + currentWave);
        isWaveActive = false;
        remainingTime = 0f;

        foreach (Monster m in FindObjectsOfType<Monster>())
        {
            if (m != null)
                StartCoroutine(FadeOutAndDisable(m));
        }

        StartCoroutine(AbsorbAllCoins());

        PlayerStats playerStats = FindObjectOfType<PlayerStats>();
        if (playerStats != null)
        {
            playerStats.currentHp = playerStats.maxHp;
            Debug.Log("[Wave End] HP recovered");
        }

        UpdateUI();
    }

    private IEnumerator FadeOutAndDisable(Monster monster)
    {
        SpriteRenderer sr = monster.GetComponent<SpriteRenderer>();
        if (sr == null)
        {
            monster.gameObject.SetActive(false);
            yield break;
        }

        Color startColor = sr.color;
        float fadeDuration = 0.8f;
        float timer = 0f;

        while (timer < fadeDuration)
        {
            if (sr == null) yield break;

            float t = timer / fadeDuration;
            sr.color = new Color(startColor.r, startColor.g, startColor.b, Mathf.Lerp(1f, 0f, t));
            timer += Time.deltaTime;
            yield return null;
        }

        sr.color = new Color(startColor.r, startColor.g, startColor.b, 0f);
        monster.gameObject.SetActive(false);
    }

    private IEnumerator AbsorbAllCoins()
    {
        PlayerInventory inventory = FindObjectOfType<PlayerInventory>();
        Transform player = inventory?.transform;
        if (player == null) yield break;

        DropItem[] coins = FindObjectsOfType<DropItem>();

        if (coins.Length == 0)
        {
            OpenShopAfterAbsorption();
            yield break;
        }

        int totalCoins = coins.Length;
        int absorbedCount = 0;

        foreach (DropItem coin in coins)
        {
            if (coin == null) continue;

            coin.SetMagnetAbsorbed();

            StartCoroutine(MoveCoinToPlayer(coin, player, inventory, () =>
            {
                absorbedCount++;
            }));
        }

        while (absorbedCount < totalCoins)
            yield return null;

        OpenShopAfterAbsorption();
    }

    private IEnumerator MoveCoinToPlayer(DropItem coin, Transform player, PlayerInventory inventory, System.Action onAbsorbed)
    {
        float speed = 15f;
        float rotateSpeed = 500f;
        float sparkleInterval = 0.05f;
        GameObject sparklePrefab = Resources.Load<GameObject>("SparkleEffect");
        float sparkleTimer = 0f;

        while (coin != null && Vector2.Distance(coin.transform.position, player.position) > 0.2f)
        {
            if (player == null) yield break;

            coin.transform.position = Vector2.MoveTowards(
                coin.transform.position,
                player.position,
                speed * Time.deltaTime
            );

            coin.transform.Rotate(Vector3.forward * rotateSpeed * Time.deltaTime);

            sparkleTimer += Time.deltaTime;
            if (sparklePrefab != null && sparkleTimer >= sparkleInterval)
            {
                sparkleTimer = 0f;
                GameObject spark = GameObject.Instantiate(sparklePrefab, coin.transform.position, Quaternion.identity);
                GameObject.Destroy(spark, 0.3f);
            }

            yield return null;
        }

        if (coin != null)
        {
            coinRemainder += 0.5f;

            if (coinRemainder >= 1f)
            {
                int addGold = Mathf.FloorToInt(coinRemainder);
                inventory.gold += addGold;
                coinRemainder -= addGold;
            }

            AudioManager.Instance?.PlaySFX(AudioManager.Instance.sfxCoin, 0.8f);
            GameObject.Destroy(coin.gameObject);
        }

        onAbsorbed?.Invoke();
    }

    private void OpenShopAfterAbsorption()
    {
        StartCoroutine(OpenShopDelayed());
    }

    private IEnumerator OpenShopDelayed()
    {
        yield return new WaitForSeconds(0.3f);
        UI_ShopManager shopManager = FindObjectOfType<UI_ShopManager>();
        if (shopManager != null)
            shopManager.OnWaveEnd(currentWave);

        currentWave++;
    }

    public int GetCurrentWave()
    {
        return currentWave;
    }

    public void ForceEndWave()
    {
        EndWave();
    }
}
