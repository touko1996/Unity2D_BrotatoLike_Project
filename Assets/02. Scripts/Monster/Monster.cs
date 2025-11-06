using UnityEngine;
using System.Collections;

public class Monster : MonoBehaviour
{
    [Header("기본 스탯")]
    [SerializeField] protected float maxHealth = 50f;           // 최대 체력
    [SerializeField] protected float moveSpeed = 5f;            // 이동 속도
    [SerializeField] protected float contactDamage = 5f;        // 플레이어 접촉 시 피해량
    [SerializeField] private GameObject dropItemPrefab;         // 드랍 아이템 프리팹

    [Header("피격 효과 설정")]
    [SerializeField] private Material hitFlashMaterial;         // 피격 시 잠깐 바뀌는 하얀색 머티리얼

    protected Transform playerTransform;                        // 플레이어 Transform
    protected SpriteRenderer spriteRenderer;                    // 몬스터 SpriteRenderer
    protected Rigidbody2D rigidBody;                            // 몬스터 Rigidbody

    protected Material originalMaterial;                        // 원래 머티리얼
    protected Color originalColor;                              // 원래 색상
    protected float currentHealth;                              // 현재 체력

    protected virtual void Start()
    {
        playerTransform = GameObject.FindWithTag("Player")?.transform;
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        rigidBody = GetComponent<Rigidbody2D>();

        if (spriteRenderer != null)
        {
            originalMaterial = spriteRenderer.sharedMaterial;
            originalColor = spriteRenderer.color;
        }

        currentHealth = maxHealth;
    }

    protected virtual void FixedUpdate()
    {
        if (playerTransform == null) return;
        Move();
    }
    
    //기본 추격 이동 로직
    protected virtual void Move()
    {
        Vector2 moveDirection = (playerTransform.position - transform.position).normalized;
        rigidBody.MovePosition(rigidBody.position + moveDirection * moveSpeed * Time.fixedDeltaTime);

        // 좌우 반전
        if (moveDirection.x > 0)
            spriteRenderer.flipX = false;
        else if (moveDirection.x < 0)
            spriteRenderer.flipX = true;
    }
    
    // 피해를 받을 때 호출
    public virtual void ReceiveDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log($"{gameObject.name} hit! HP: {currentHealth}");

        if (currentHealth > 0 && spriteRenderer != null && hitFlashMaterial != null)
            StartCoroutine(FlashWhiteOnce());

        if (currentHealth <= 0)
            Die();
    }

    private IEnumerator FlashWhiteOnce()
    {
        if (spriteRenderer == null || hitFlashMaterial == null)
            yield break;

        if (originalMaterial == null)
            originalMaterial = spriteRenderer.sharedMaterial;

        spriteRenderer.sharedMaterial = hitFlashMaterial;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.sharedMaterial = originalMaterial;
    }

    // 몬스터 사망 처리
    protected virtual void Die()
    {
        if (dropItemPrefab != null)
            Instantiate(dropItemPrefab, transform.position, Quaternion.identity);

        Vector2 knockbackDir = Vector2.zero;
        if (playerTransform != null)
            knockbackDir = (transform.position - playerTransform.position).normalized;

        StartCoroutine(PlayDeathEffect(knockbackDir));
    }

    // 사망 시 넉백 + 회전 + 축소 연출
    public IEnumerator PlayDeathEffect(Vector2 knockbackDir)
    {
        float duration = 0.4f;
        float timer = 0f;

        Vector3 startScale = transform.localScale;
        Vector3 startPos = transform.position;
        float knockbackPower = 2f;
        float rotationSpeed = 720f;

        if (rigidBody != null)
        {
            rigidBody.velocity = Vector2.zero;
            rigidBody.isKinematic = true;
        }

        Vector3 targetPos = startPos + (Vector3)(knockbackDir * knockbackPower);
        float moveTime = duration * 0.4f;
        timer = 0f;

        while (timer < moveTime)
        {
            timer += Time.unscaledDeltaTime;
            float t = timer / moveTime;
            transform.position = Vector3.Lerp(startPos, targetPos, t);
            transform.Rotate(0f, 0f, rotationSpeed * Time.unscaledDeltaTime);
            yield return null;
        }

        float shrinkTime = duration * 0.6f;
        timer = 0f;
        Vector3 shrinkStart = transform.localScale;

        while (timer < shrinkTime)
        {
            timer += Time.unscaledDeltaTime;
            float t = timer / shrinkTime;
            transform.localScale = Vector3.Lerp(shrinkStart, Vector3.zero, t);
            transform.Rotate(0f, 0f, rotationSpeed * Time.unscaledDeltaTime);
            yield return null;
        }

        gameObject.SetActive(false);
    }

    //체력 회복 (힐러 몬스터 등에서 사용)
    public virtual void Heal(float amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            PlayerStats playerStats = collision.collider.GetComponent<PlayerStats>();
            if (playerStats != null)
                playerStats.TakeDamage(contactDamage);
        }
    }

    // 웨이브 스케일링 적용 (체력, 공격력, 이동속도 조정)
    public virtual void ApplyWaveScaling(float hpMultiplier, float damageMultiplier, float speedMultiplier)
    {
        maxHealth *= hpMultiplier;
        contactDamage *= damageMultiplier;
        moveSpeed *= speedMultiplier;
        currentHealth = maxHealth;
    }

    /// <summary>
    /// 스폰 시 초기화 (색상, 체력 등)
    /// ChargingMonster 등 자식 클래스에서 override 가능
    /// </summary>
    protected virtual void OnEnable()
    {
        ResetMonsterState();
    }

    protected virtual void ResetMonsterState()
    {
        if (spriteRenderer != null)
        {
            Color restored = spriteRenderer.color;
            restored.a = 1f;
            spriteRenderer.color = restored;
        }

        if (originalMaterial != null)
        { 
            spriteRenderer.sharedMaterial = originalMaterial;
        }

        currentHealth = maxHealth;

        if (rigidBody != null)
            rigidBody.velocity = Vector2.zero;
    }
}
