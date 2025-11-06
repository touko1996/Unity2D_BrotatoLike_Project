using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerStats : MonoBehaviour
{
    [Header("Current Stats")]
    public float currentDamage = 10f;
    public float currentRange = 5f;
    public float currentAttackSpeed = 1f;
    public float currentMoveSpeed = 5f;

    [Header("HP")]
    public float maxHp = 10f;
    public float currentHp = 10f;

    [Header("Damage Text")]
    [Tooltip("피격 시 표시할 데미지 텍스트 프리팹")]
    [SerializeField] private GameObject damageTextPrefab;

    private bool isDead = false;
    private UI_PlayerStatus uiStatus;

    private void Start()
    {
        currentHp = maxHp;
        uiStatus = FindObjectOfType<UI_PlayerStatus>();
        UpdateHpUI();
    }

    // ---------------------------------------------
    // 스탯 변화 (패시브 아이템용)
    // ---------------------------------------------
    public void AddStatModifier(float dmg, float range, float atkSpeed, float move)
    {
        currentDamage += dmg;
        currentRange += range;
        currentAttackSpeed += atkSpeed;
        currentMoveSpeed += move;
    }

    public void RemoveStatModifier(float dmg, float range, float atkSpeed, float move)
    {
        currentDamage -= dmg;
        currentRange -= range;
        currentAttackSpeed -= atkSpeed;
        currentMoveSpeed -= move;
    }

    // ---------------------------------------------
    // 체력 변화
    // ---------------------------------------------
    public void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHp -= amount;
        if (currentHp < 0f) currentHp = 0f;

        // 카메라 흔들기
        if (CameraFollow.Instance != null)
            CameraFollow.Instance.ShakeCamera();

        // 플레이어 머리 위 데미지 텍스트 표시
        ShowDamageText(amount);

        UpdateHpUI();

        if (currentHp <= 0f)
            Die();
    }

    private void ShowDamageText(float damage)
    {
        if (damageTextPrefab == null) return;

        GameObject canvasObj = GameObject.Find("DamageCanvas");
        Transform parent = canvasObj != null ? canvasObj.transform : null;

        Vector3 textPos = transform.position + Vector3.up * 1.2f;
        GameObject dmgTextObj = Instantiate(damageTextPrefab, textPos, Quaternion.identity, parent);

        DamageText dmgText = dmgTextObj.GetComponent<DamageText>();
        if (dmgText != null)
            dmgText.SetText(-damage, Color.red); // 빨간색으로 ‘-5’ 표시
    }

    public void Heal(float amount)
    {
        if (isDead) return;

        currentHp += amount;
        if (currentHp > maxHp) currentHp = maxHp;
        UpdateHpUI();
    }

    // ---------------------------------------------
    // 사망 처리
    // ---------------------------------------------
    private void Die()
    {
        if (isDead)
            return;

        isDead = true;
        Debug.Log("Die() called: Player HP reached 0");

        // 카메라 흔들림 중단
        if (CameraFollow.Instance != null)
            CameraFollow.Instance.StopShake();

        // 게임 정지
        Time.timeScale = 0f;

        // 스폰러 중단
        MonsterSpawner spawner = FindObjectOfType<MonsterSpawner>();
        if (spawner != null)
            spawner.StopSpawning();

        // 웨이브 UI 중단
        UI_GameWave waveUI = FindObjectOfType<UI_GameWave>();
        if (waveUI != null)
            waveUI.enabled = false;

        // 사운드 재생
        AudioManager.Instance.PlayGameOver();

        // 데이터 저장
        SaveWaveAndInventory();

        // 몬스터 사망 연출
        StartCoroutine(TriggerAllMonsterDeathEffects());

        // 플레이어 본인 사망 연출
        StartCoroutine(PlayerDeathEffect());

        // 이 한 줄이 현재 누락된 부분!
        // 일정 시간 후 TimeScale 복원 + 씬 전환
        StartCoroutine(LoadGameOverAfterFreeze(2.5f));
    }


    private void SaveWaveAndInventory()
    {
        UI_GameWave waveUI = FindObjectOfType<UI_GameWave>();
        if (waveUI != null)
        {
            PlayerPrefsData.lastWave = waveUI.GetCurrentWave();
            Debug.Log("Wave data saved: " + PlayerPrefsData.lastWave);
        }

        PlayerInventory inventory = GetComponent<PlayerInventory>();
        if (inventory != null)
        {
            PlayerPrefsData.SaveFromInventory(inventory);
            Debug.Log("Inventory data copied to PlayerPrefsData");
        }
    }

    // ---------------------------------------------
    // 모든 몬스터 사망 연출
    // ---------------------------------------------
    private IEnumerator TriggerAllMonsterDeathEffects()
    {
        Monster[] monsters = FindObjectsOfType<Monster>();

        foreach (Monster m in monsters)
        {
            if (m.gameObject.activeInHierarchy)
            {
                Vector2 knockDir = Vector2.zero;
                if (m != null && transform != null)
                    knockDir = (m.transform.position - transform.position).normalized;

                m.StartCoroutine(m.DeathEffect(knockDir));
            }
        }

        yield break;
    }

    // ---------------------------------------------
    // 플레이어 사망 연출
    // ---------------------------------------------
    private IEnumerator PlayerDeathEffect()
    {
        float duration = 0.6f;
        float timer = 0f;
        Vector3 startScale = transform.localScale;
        float rotationSpeed = 720f;

        // 애니메이션, 이동, 물리 중지
        PlayerMove move = GetComponent<PlayerMove>();
        if (move != null) move.enabled = false;

        PlayerAnimation anim = GetComponent<PlayerAnimation>();
        if (anim != null) anim.enabled = false;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.isKinematic = true;
        }

        Quaternion startRot = transform.rotation;

        while (timer < duration)
        {
            timer += Time.unscaledDeltaTime;
            transform.rotation = startRot * Quaternion.Euler(0f, 0f, rotationSpeed * (timer / duration));
            transform.localScale = Vector3.Lerp(startScale, Vector3.zero, timer / duration);
            yield return null;
        }

        Destroy(gameObject, 0.05f); // 완전히 제거
    }


    private IEnumerator LoadGameOverAfterFreeze(float delay)
    {
        float timer = 0f;
        while (timer < delay)
        {
            timer += Time.unscaledDeltaTime; // 멈춘 시간에서도 카운트
            yield return null;
        }

        Time.timeScale = 1f; // 복원
        SceneManager.LoadScene("GameOverScene");
    }

    private void UpdateHpUI()
    {
        if (uiStatus != null)
            uiStatus.UpdateHPUI(currentHp, maxHp);
    }
}
