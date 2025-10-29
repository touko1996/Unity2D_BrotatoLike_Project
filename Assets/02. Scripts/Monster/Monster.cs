using UnityEngine;

public class Monster : MonoBehaviour
{
    [Header("�⺻ �ɷ�ġ")]
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

    protected virtual void Update()
    {
        if (player == null) return;
        Move();
    }

    protected virtual void Move()
    {
        Vector2 dir = (player.position - transform.position).normalized;
        rb.MovePosition(rb.position + dir * moveSpeed * Time.deltaTime);

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
        Debug.Log($"{gameObject.name} ȸ���� (+{amount}), ���� HP: {hp}");
    }
    protected virtual void Die()
    {
        Debug.Log($"{gameObject.name} ���");

        // ������ ���
        if (dropItemPrefab != null)
        {
            Instantiate(dropItemPrefab, transform.position, Quaternion.identity);
        }

        gameObject.SetActive(false);
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Debug.Log($"{gameObject.name}��(��) �÷��̾�� {contactDamage} ������!");
            // ���� PlayerStats.ReceiveDamage(contactDamage) ȣ�� ����
        }
    }
}
