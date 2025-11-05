using UnityEngine;

public class Monster : MonoBehaviour
{
    [Header("Base Stats")]
    [SerializeField] protected float hp = 50f;
    [SerializeField] protected float moveSpeed = 5f;
    [SerializeField] protected float contactDamage = 5f;
    [SerializeField] private GameObject dropItemPrefab;

    protected Transform player;
    protected SpriteRenderer spriteRenderer;
    protected Rigidbody2D rb;

    protected virtual void Start()
    {
        player = GameObject.FindWithTag("Player")?.transform;
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
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
        if (hp <= 0)
            Die();
    }

    public virtual void Heal(float amount)
    {
        hp += amount;
        Debug.Log($"{gameObject.name} 회복됨 (+{amount}), 현재 HP: {hp}");
    }

    protected virtual void Die()
    {
        Debug.Log($"{gameObject.name} 사망");

        if (dropItemPrefab != null)
            Instantiate(dropItemPrefab, transform.position, Quaternion.identity);

        gameObject.SetActive(false);
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

    // 웨이브별 스케일 조정 함수
    public virtual void SetWaveScaling(float hpScale, float dmgScale, float spdScale)
    {
        hp *= hpScale;
        contactDamage *= dmgScale;
        moveSpeed *= spdScale;

        Debug.Log($"[Monster Scaling] {gameObject.name} → HP x{hpScale:F2}, DMG x{dmgScale:F2}, SPD x{spdScale:F2}");
    }
}
