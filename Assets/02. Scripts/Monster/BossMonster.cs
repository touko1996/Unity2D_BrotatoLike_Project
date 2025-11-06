using UnityEngine;
using System.Collections;

public class BossMonster : Monster
{
    [Header("Boss Attack Settings")]
    [SerializeField] private GameObject attackPrefab;
    [SerializeField] private float attackRange = 6f;
    [SerializeField] private float attackDelay = 0.5f;
    [SerializeField] private float attackCooldown = 1.8f;
    [SerializeField] private float attackDamage = 15f;

    [Header("Boss Flash Material")]
    [SerializeField] private Material bossFlashMat; // 인스펙터에서 직접 할당 가능

    private bool canAttack = true;
    private float maxHpCache;

    protected override void Start()
    {
        base.Start();
        maxHpCache = hp;

        // 보스 HP UI 초기화
        UI_BossHP bossUI = FindObjectOfType<UI_BossHP>(true);
        if (bossUI != null)
            bossUI.InitBoss(this);
    }

    protected override void FixedUpdate()
    {
        if (player == null) return;
        if (Time.timeScale == 0f) return;

        float dist = Vector2.Distance(player.position, transform.position);

        if (canAttack && dist <= attackRange)
            StartCoroutine(AttackPattern());
        else
            Move();
    }

    private IEnumerator AttackPattern()
    {
        canAttack = false;

        Vector2 dir = (player.position - transform.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Vector2 spawnPos = (Vector2)transform.position + dir * (attackRange * 0.7f);

        GameObject warning = Instantiate(attackPrefab, spawnPos, Quaternion.Euler(0f, 0f, angle));

        SpriteRenderer sr = warning.GetComponent<SpriteRenderer>();
        Color baseColor = sr.color;

        // 1단계: 반투명 경고
        sr.color = new Color(baseColor.r, baseColor.g, baseColor.b, 0.3f);

        // 2단계: 0.5초 동안 서서히 진하게
        float t = 0f;
        while (t < attackDelay)
        {
            t += Time.deltaTime;
            float a = Mathf.Lerp(0.3f, 1f, t / attackDelay);
            sr.color = new Color(baseColor.r, baseColor.g, baseColor.b, a);
            yield return null;
        }

        // 3단계: Collider 활성화 후 데미지 판정
        Collider2D col = warning.GetComponent<Collider2D>();
        if (col != null) col.enabled = true;

        yield return new WaitForSeconds(0.2f);

        // 4단계: 사라짐
        Destroy(warning);

        // 5단계: 쿨타임 후 재공격 가능
        yield return new WaitForSeconds(attackCooldown + Random.Range(0.2f, 0.5f));
        canAttack = true;
    }

    public override void ReceiveDamage(float damage)
    {
        hp -= damage;

        if (hp > 0f && spriteRenderer != null && bossFlashMat != null)
            StartCoroutine(FlashWhiteOnce());

        if (hp <= 0f)
            Die();
    }

    private IEnumerator FlashWhiteOnce()
    {
        spriteRenderer.material = bossFlashMat;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.material = originalMat; // 부모(Monster)의 originalMat 사용
    }

    protected override void Die()
    {
        base.Die();

        // 보스 체력바 숨기기
        UI_BossHP bossUI = FindObjectOfType<UI_BossHP>(true);
        if (bossUI != null)
            bossUI.Hide();

        // 웨이브 강제 종료
        UI_GameWave waveUI = FindObjectOfType<UI_GameWave>();
        if (waveUI != null)
            waveUI.ForceEndWave();
    }

    public void DealDamageToPlayer(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerStats ps = other.GetComponent<PlayerStats>();
        if (ps != null)
            ps.TakeDamage(attackDamage);
    }

    public float GetCurrentHp() => hp;
    public float GetMaxHp() => maxHpCache;
}
