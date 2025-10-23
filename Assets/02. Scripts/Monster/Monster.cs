using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [Header("���� ����")]
    public float speed = 2f;
    public float hp = 30f;

    private Transform player;
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>(); // �ø���
        if (rb != null)
        {
            rb.gravityScale = 0;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

    void Start()
    {
        player = GameObject.FindWithTag("Player")?.transform;
    }

    void Update()
    {
        if (player == null) return;

        // �÷��̾� ���� ���
        Vector2 dir = (player.position - transform.position).normalized;

        // �̵�
        if (rb != null)
        {
            rb.MovePosition(rb.position + dir * speed * Time.deltaTime);
        }
        else
        {
            transform.position += (Vector3)dir * speed * Time.deltaTime;
        }

        // --- �¿� ���⿡ ���� �ø� ���� ---
        if (player.position.x > transform.position.x)
        {
            sr.flipX = false; // �÷��̾ �����ʿ� ����
        }
        else if (player.position.x < transform.position.x)
        {
            sr.flipX = true;  // �÷��̾ ���ʿ� ����
        }
    }

    public void TakeDamage(float dmg)
    {
        hp -= dmg;
        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            // ü�� ���� ���� ������ ���߿� �߰��� �� ����
        }
    }
}