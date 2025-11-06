using UnityEngine;
using System.Collections;

public class Monster : MonoBehaviour
{
    [Header("Base Stats")]
    [SerializeField] protected float hp = 50f;
    [SerializeField] protected float moveSpeed = 5f;
    [SerializeField] protected float contactDamage = 5f;
    [SerializeField] private GameObject dropItemPrefab;

    [Header("Hit Effect Settings")]
    [SerializeField] private Material whiteFlashMat;

    protected Transform player;
    protected SpriteRenderer spriteRenderer;
    protected Rigidbody2D rb;

    protected Material originalMat;
    private Color originalColor;

    protected float baseHp;

    protected virtual void Start()
    {
        player = GameObject.FindWithTag("Player")?.transform;
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        if (spriteRenderer != null)
        {
            originalMat = spriteRenderer.sharedMaterial;
            originalColor = spriteRenderer.color;
        }
        else
        {
            Debug.LogWarning(gameObject.name + " could not find SpriteRenderer.");
        }
        baseHp = hp;
    }

    protected virtual void FixedUpdate()
    {
        if (player == null) return;
        Move();
    }

    protected virtual void Move()
    {
        Vector2 dir = (player.position - transform.position).normalized;
        rb.MovePosition(rb.position + dir * moveSpeed * Time.fixedDeltaTime);

        if (dir.x > 0)
            spriteRenderer.flipX = false;
        else if (dir.x < 0)
            spriteRenderer.flipX = true;
    }

    public virtual void ReceiveDamage(float damage)
    {
        hp -= damage;
        Debug.Log(gameObject.name + " hit. HP: " + hp);

        if (hp > 0 && spriteRenderer != null && whiteFlashMat != null)
            StartCoroutine(FlashWhite());

        if (hp <= 0)
            Die();
    }

    private IEnumerator FlashWhite()
    {
        if (spriteRenderer == null || whiteFlashMat == null)
            yield break;

        if (originalMat == null)
            originalMat = spriteRenderer.sharedMaterial;

        spriteRenderer.sharedMaterial = whiteFlashMat;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.sharedMaterial = originalMat;
    }

    protected virtual void Die()
    {
        Debug.Log(gameObject.name + " died");

        if (dropItemPrefab != null)
            Instantiate(dropItemPrefab, transform.position, Quaternion.identity);

        Vector2 knockDir = Vector2.zero;
        if (player != null)
            knockDir = (transform.position - player.position).normalized;

        StartCoroutine(DeathEffect(knockDir));
    }

    public IEnumerator DeathEffect(Vector2 knockDir)
    {
        float duration = 0.4f;
        float timer = 0f;

        Vector3 startScale = transform.localScale;
        Vector3 startPos = transform.position;
        float knockPower = 2f;
        float rotationSpeed = 720f;

        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.isKinematic = true;
        }

        Vector3 targetPos = startPos + (Vector3)(knockDir * knockPower);
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

    public virtual void Heal(float amount)
    {
        hp += amount;
        Debug.Log(gameObject.name + " healed. HP: " + hp);
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

    public virtual void SetWaveScaling(float hpScale, float dmgScale, float spdScale)
    {
        hp *= hpScale;
        contactDamage *= dmgScale;
        moveSpeed *= spdScale;

        Debug.Log("[Monster Scaling] " + gameObject.name + " HP x" + hpScale + " DMG x" + dmgScale + " SPD x" + spdScale);
    }
    protected virtual void OnEnable()
    {
        // 스폰 시 투명도 복원
        if (spriteRenderer != null)
        {
            Color c = spriteRenderer.color;
            c.a = 1f;
            spriteRenderer.color = c;
        }
        hp = baseHp;
    }

}
