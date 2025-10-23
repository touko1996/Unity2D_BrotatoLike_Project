using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [Header("스탯 설정")]
    public float speed = 2f;
    public float hp = 30f;

    private Transform player;
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>(); // 플립용
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

        // 플레이어 방향 계산
        Vector2 dir = (player.position - transform.position).normalized;

        // 이동
        if (rb != null)
        {
            rb.MovePosition(rb.position + dir * speed * Time.deltaTime);
        }
        else
        {
            transform.position += (Vector3)dir * speed * Time.deltaTime;
        }

        // --- 좌우 방향에 따라 플립 적용 ---
        if (player.position.x > transform.position.x)
        {
            sr.flipX = false; // 플레이어가 오른쪽에 있음
        }
        else if (player.position.x < transform.position.x)
        {
            sr.flipX = true;  // 플레이어가 왼쪽에 있음
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
            // 체력 감소 같은 로직을 나중에 추가할 수 있음
        }
    }
}