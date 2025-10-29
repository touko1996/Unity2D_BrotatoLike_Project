using System.Collections;
using UnityEngine;

public class HealerMonster : Monster
{
    [Header("힐러 몬스터 설정")]
    [SerializeField] private float wanderRadius = 5f;      // 배회 범위
    [SerializeField] private float changeDirectionTime = 3f; // 방향 변경 주기
    [SerializeField] private float fleeRange = 4f;          // 플레이어 감지 거리
    [SerializeField] private float fleeSpeed = 2f;          // 도망 속도
    [SerializeField] private float healInterval = 4f;       // 힐 주기
    [SerializeField] private float healAmount = 5f;         // 회복량

    [Header("이펙트 설정")]
    [SerializeField] private float outlineShowTime = 0.4f;  // Outline 유지 시간

    private Vector2 wanderDirection;
    private float directionTimer;
    private float healTimer;

    private SpriteRenderer outlineRenderer;

    protected override void Start()
    {
        base.Start();

        // Outline 자식 오브젝트 탐색 (Outline SpriteRenderer 필수)
        var outline = transform.Find("Outline");
        if (outline != null)
            outlineRenderer = outline.GetComponent<SpriteRenderer>();

        if (outlineRenderer != null)
            outlineRenderer.enabled = false; // 기본은 꺼둠

        ChooseNewDirection();
    }

    protected override void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(player.position, transform.position);

        // 플레이어가 가까우면 도망
        if (distanceToPlayer <= fleeRange)
        {
            Vector2 dir = (transform.position - player.position).normalized;
            rb.MovePosition(rb.position + dir * fleeSpeed * Time.deltaTime);
            spriteRenderer.flipX = dir.x < 0;
        }
        else
        {
            // 무작위 배회
            directionTimer -= Time.deltaTime;
            if (directionTimer <= 0)
                ChooseNewDirection();

            rb.MovePosition(rb.position + wanderDirection * moveSpeed * Time.deltaTime);
            spriteRenderer.flipX = wanderDirection.x < 0;
        }

        // 일정 주기마다 힐 발동
        healTimer -= Time.deltaTime;
        if (healTimer <= 0)
        {
            HealAllMonsters();
            healTimer = healInterval;
            StartCoroutine(OutlineEffectSimple()); // 힐 시 Outline 켜기
        }
    }

    private void ChooseNewDirection()
    {
        wanderDirection = Random.insideUnitCircle.normalized;
        directionTimer = changeDirectionTime;
    }

    private void HealAllMonsters()
    {
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");

        foreach (GameObject monsterObj in monsters)
        {
            Monster m = monsterObj.GetComponent<Monster>();
            if (m != null && m != this)
                m.Heal(healAmount);
        }

        Debug.Log($"{gameObject.name}이(가) 주변 몬스터를 회복시켰습니다! (+{healAmount})");
    }

    private IEnumerator OutlineEffectSimple()
    {
        if (outlineRenderer == null) yield break;

        outlineRenderer.enabled = true;               // 켜기
        yield return new WaitForSeconds(outlineShowTime);
        outlineRenderer.enabled = false;              // 끄기
    }
}
