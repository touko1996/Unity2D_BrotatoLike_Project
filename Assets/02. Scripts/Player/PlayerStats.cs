using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerStats : MonoBehaviour
{
    [Header("현재 스탯")]
    public float currentDamage = 10f;      // 공격력
    public float currentRange = 5f;        // 사거리
    public float currentAttackSpeed = 1f;  // 공격 속도
    public float currentMoveSpeed = 5f;    // 이동 속도

    [Header("체력")]
    public float maxHp = 10f;              // 최대 체력
    public float currentHp = 10f;          // 현재 체력

    [Header("데미지 텍스트")]
    [SerializeField] private GameObject damageTextPrefab; // 피격 시 표시될 데미지 텍스트 프리팹

    private bool isDead = false;           // 사망 여부
    private UI_PlayerStatus uiStatus;      // 체력 UI 참조

    private void Start()
    {
        currentHp = maxHp;
        uiStatus = FindObjectOfType<UI_PlayerStatus>();
        UpdateHpUI();
    }

    // 패시브 아이템 등으로 스탯이 변경될 때 호출
    public void AddStatModifier(float dmg, float range, float atkSpeed, float move)
    {
        currentDamage += dmg;
        currentRange += range;
        currentAttackSpeed += atkSpeed;
        currentMoveSpeed += move;
    }

    // 데미지를 받을 때 호출
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

    // 피격 시 데미지 텍스트 표시
    private void ShowDamageText(float damage)
    {
        if (damageTextPrefab == null) return;

        GameObject canvasObj = GameObject.Find("DamageCanvas");
        Transform parent = canvasObj != null ? canvasObj.transform : null;

        Vector3 textPos = transform.position + Vector3.up * 1.2f;
        GameObject dmgTextObj = Instantiate(damageTextPrefab, textPos, Quaternion.identity, parent);

        DamageText dmgText = dmgTextObj.GetComponent<DamageText>();
        if (dmgText != null)
            dmgText.SetText(-damage, Color.red); // 플레이어 피격시 빨간색 텍스트
    }

    // 사망 처리
    private void Die()
    {
        if (isDead) return;
        isDead = true;

        // 카메라 흔들림 중단
        if (CameraFollow.Instance != null)
            CameraFollow.Instance.StopShake();

        // 게임 일시정지
        Time.timeScale = 0f;

        // 몬스터 스포너 중단
        MonsterSpawner spawner = FindObjectOfType<MonsterSpawner>();
        if (spawner != null)
            spawner.StopSpawning();

        // 웨이브 UI 비활성화
        UI_GameWave waveUI = FindObjectOfType<UI_GameWave>();
        if (waveUI != null)
            waveUI.enabled = false;

        // 사운드 재생
        AudioManager.Instance.PlayGameOver();

        // 데이터 저장
        SaveWaveAndInventory();

        // 화면 내 모든 몬스터 사망 연출 실행
        StartCoroutine(TriggerAllMonsterDeathEffects());

        // 플레이어 본인 사망 연출
        StartCoroutine(PlayerDeathEffect());

        // 일정 시간 후 씬 전환
        StartCoroutine(LoadGameOverAfterFreeze(2.5f));
    }

    // 현재 웨이브 및 인벤토리 정보 저장
    private void SaveWaveAndInventory()
    {
        UI_GameWave waveUI = FindObjectOfType<UI_GameWave>();
        if (waveUI != null)
        {
            PlayerPrefsData.lastWave = waveUI.GetCurrentWave();
        }

        PlayerInventory inventory = GetComponent<PlayerInventory>();
        if (inventory != null)
        {
            PlayerPrefsData.SaveFromInventory(inventory);
        }
    }

    // 현재 활성화된 모든 몬스터의 사망 이펙트 실행
    private IEnumerator TriggerAllMonsterDeathEffects()
    {
        Monster[] monsters = FindObjectsOfType<Monster>();

        foreach (Monster monster in monsters)
        {
            if (monster.gameObject.activeInHierarchy)
            {
                Vector2 knockDir = Vector2.zero;
                if (monster != null && transform != null)
                    knockDir = (monster.transform.position - transform.position).normalized;

                // 이름 변경된 메서드로 수정
                monster.StartCoroutine(monster.PlayDeathEffect(knockDir));
            }
        }

        yield break;
    }

    // 플레이어 사망 연출 (회전 + 축소)
    private IEnumerator PlayerDeathEffect()
    {
        float duration = 0.6f;
        float timer = 0f;
        Vector3 startScale = transform.localScale;
        float rotationSpeed = 720f;

        // 이동, 애니메이션, 물리 중지
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
            timer += Time.unscaledDeltaTime; // TimeScale 0이어도 진행
            transform.rotation = startRot * Quaternion.Euler(0f, 0f, rotationSpeed * (timer / duration));
            transform.localScale = Vector3.Lerp(startScale, Vector3.zero, timer / duration);
            yield return null;
        }

        Destroy(gameObject, 0.05f); // 오브젝트 제거
    }

    // 사망 후 일정 시간 후에 게임오버 씬으로 이동
    private IEnumerator LoadGameOverAfterFreeze(float delay)
    {
        float timer = 0f;
        while (timer < delay)
        {
            timer += Time.unscaledDeltaTime;
            yield return null;
        }

        Time.timeScale = 1f;
        SceneManager.LoadScene("GameOverScene");
    }

    // 체력 UI 갱신
    private void UpdateHpUI()
    {
        if (uiStatus != null)
            uiStatus.UpdateHPUI(currentHp, maxHp);
    }
}
