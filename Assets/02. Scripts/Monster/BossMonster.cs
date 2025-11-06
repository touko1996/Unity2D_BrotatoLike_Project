using UnityEngine;
using System.Collections;

public class BossMonster : Monster
{
    [Header("보스 공격 관련 설정")]
    [SerializeField] private GameObject bossAttackPrefab;         // 보스 공격 프리팹
    [SerializeField] private float bossAttackRange = 6f;          // 공격 사거리
    [SerializeField] private float bossAttackChargeTime = 0.5f;   // 공격 전 경고 시간
    [SerializeField] private float bossAttackCooldown = 1.8f;     // 다음 공격까지 대기 시간
    [SerializeField] private float bossAttackDamage = 15f;        // 공격 데미지

    [Header("보스 피격시 색상")]
    [SerializeField] private Material bossHitFlashMaterial;           

    private bool canAttack = true;                                // 공격 가능 여부
    private float bossMaxHp;                                      // 보스의 최대 체력 저장용

    protected override void Start()
    {
        base.Start();
        bossMaxHp = currentHealth; 

        // 보스 체력바 UI 초기화
        UI_BossHP bossHpUI = FindObjectOfType<UI_BossHP>(true);
        if (bossHpUI != null)
            bossHpUI.InitBoss(this);
    }

    protected override void FixedUpdate()
    {
        if (playerTransform == null || Time.timeScale == 0f) return;

        float distanceToPlayer = Vector2.Distance(playerTransform.position, transform.position);

        // 공격 가능 상태이면서 사거리 내에 플레이어가 있을 때 공격 시작
        if (canAttack && distanceToPlayer <= bossAttackRange)
        {
            StartCoroutine(BossAttackPattern());
        }
        else
        {
            Move();
        }
    }

    // 보스 공격 패턴 (경고 → 타격 → 쿨타임)
    private IEnumerator BossAttackPattern()
    {
        canAttack = false;

        // 플레이어 방향 계산
        Vector2 direction = (playerTransform.position - transform.position).normalized;
        float attackAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // 공격 경고 이펙트 생성 위치
        Vector2 spawnPosition = (Vector2)transform.position + direction * (bossAttackRange * 0.7f);

        GameObject attackWarning = Instantiate(bossAttackPrefab, spawnPosition, Quaternion.Euler(0f, 0f, attackAngle));

        SpriteRenderer sr = attackWarning.GetComponent<SpriteRenderer>();
        Color baseColor = sr.color;

        // 1단계: 반투명 경고 색상
        sr.color = new Color(baseColor.r, baseColor.g, baseColor.b, 0.3f);

        // 2단계: 점점 진하게 변하면서 공격 예고
        float elapsed = 0f;
        while (elapsed < bossAttackChargeTime)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(0.3f, 1f, elapsed / bossAttackChargeTime);
            sr.color = new Color(baseColor.r, baseColor.g, baseColor.b, alpha);
            yield return null;
        }

        // 3단계: 콜라이더 활성화 (공격 판정)
        Collider2D col = attackWarning.GetComponent<Collider2D>();
        if (col != null)
            col.enabled = true;

        // 짧은 시간 후 이펙트 제거
        yield return new WaitForSeconds(0.2f);
        Destroy(attackWarning);

        // 4단계: 쿨타임 후 다시 공격 가능
        yield return new WaitForSeconds(bossAttackCooldown + Random.Range(0.2f, 0.5f));
        canAttack = true;
    }

    // 피격 시 처리
    public override void ReceiveDamage(float damage)
    {
        currentHealth -= damage; // 부모의 currentHealth 사용

        // 체력이 남아있으면 피격 반짝임
        if (currentHealth > 0f && spriteRenderer != null && bossHitFlashMaterial != null)
            StartCoroutine(FlashWhiteOnce());

        // 체력이 0 이하일 경우 사망 처리
        if (currentHealth <= 0f)
            Die();
    }

    // 피격시 짧은 반짝임 효과
    private IEnumerator FlashWhiteOnce()
    {
        spriteRenderer.material = bossHitFlashMaterial;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.material = originalMaterial;
    }

    // 사망 처리
    protected override void Die()
    {
        base.Die();

        // 보스 체력 UI 숨기기
        UI_BossHP bossHpUI = FindObjectOfType<UI_BossHP>(true);
        if (bossHpUI != null)
            bossHpUI.Hide();

        // 웨이브 강제 종료
        UI_GameWave waveUI = FindObjectOfType<UI_GameWave>();
        if (waveUI != null)
            waveUI.ForceEndWave();
    }

    // 공격이 플레이어에 닿았을 때 데미지 적용
    public void DealDamageToPlayer(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerStats playerStats = other.GetComponent<PlayerStats>();
        if (playerStats != null)
            playerStats.TakeDamage(bossAttackDamage);
    }

    // UI용 현재 체력 반환
    public float GetCurrentHp() => currentHealth;

    // UI용 최대 체력 반환
    public float GetMaxHp() => bossMaxHp;
}
