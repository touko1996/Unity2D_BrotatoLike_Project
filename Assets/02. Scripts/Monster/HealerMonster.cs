using System.Collections;
using UnityEngine;

/// <summary>
/// [HealerMonster]
/// ------------------------------------------------------------
/// 힐러 몬스터는 전투보다는 보조형 AI 역할을 가진 몬스터로,
/// 플레이어가 가까이 오면 도망치고,
/// 플레이어가 멀리 있으면 PerlinWander를 통해 부드럽게 배회하며 돌아다닌다.
///
/// 또한 일정 주기마다 주변의 다른 몬스터를 힐(회복)하는 기능을 수행한다.
///
/// 구조 요약:
/// 1. PerlinWander 컴포넌트를 이용해 배회 이동
/// 2. 플레이어와 일정 거리 이하로 가까워지면 도망 (wander 비활성)
/// 3. 일정 시간마다 주변 몬스터 HP 회복
/// 4. 힐 시 Outline 효과 표시
///
/// PerlinWander 연동 방식:
/// - 이동 로직은 HealerMonster가 아니라 PerlinWander에서 담당
/// - HealerMonster는 단순히 언제 배회할지, 멈출지를 제어만 함
///   (wander.StartWander(), wander.StopWander())
///
/// 알고리즘 포인트:
/// Perlin Noise 기반 이동 알고리즘을 활용하여
/// 단순 랜덤 이동이 아닌, 부드럽고 생명체 같은 움직임을 구현함.
/// ------------------------------------------------------------
/// </summary>
public class HealerMonster : Monster
{
    [Header("힐러 몬스터 설정")]
    [Tooltip("플레이어가 이 거리 안으로 들어오면 도망 시작")]
    [SerializeField] private float fleeRange = 4f;

    [Tooltip("도망칠 때의 속도")]
    [SerializeField] private float fleeSpeed = 2f;

    [Tooltip("힐 발동 주기 (초 단위)")]
    [SerializeField] private float healInterval = 4f;

    [Tooltip("회복시킬 HP 양")]
    [SerializeField] private float healAmount = 5f;

    [Tooltip("Outline(윤곽선) 효과 유지 시간")]
    [SerializeField] private float outlineShowTime = 0.4f;

    private float healTimer; // 힐 주기 타이머
    private SpriteRenderer outlineRenderer; // 힐 시 표시되는 외곽선 스프라이트
    private PerlinWander wander; // Perlin 기반 배회 제어용

    protected override void Start()
    {
        base.Start();

        // PerlinWander 컴포넌트를 찾고 배회 시작
        wander = GetComponent<PerlinWander>();
        if (wander != null)
        {
            wander.StartWander();
        }

        // Outline 자식 오브젝트 탐색
        var outline = transform.Find("Outline");
        if (outline != null)
            outlineRenderer = outline.GetComponent<SpriteRenderer>();

        // 초기에는 아웃라인 꺼둠
        if (outlineRenderer != null)
            outlineRenderer.enabled = false;
    }

    protected override void FixedUpdate()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(player.position, transform.position);

        // 플레이어가 가까우면 도망 모드
        if (distanceToPlayer <= fleeRange)
        {
            // 배회 정지
            if (wander != null) wander.StopWander();

            // 플레이어 반대 방향으로 이동
            Vector2 dir = (transform.position - player.position).normalized;
            rb.MovePosition(rb.position + dir * fleeSpeed * Time.deltaTime);

            // 방향 전환
            spriteRenderer.flipX = dir.x < 0f;
        }
        else
        {
            // 플레이어가 멀면 배회 재개
            if (wander != null) wander.StartWander();
        }

        // 일정 주기마다 힐 발동
        healTimer -= Time.deltaTime;
        if (healTimer <= 0)
        {
            HealAllMonsters();
            healTimer = healInterval;
            StartCoroutine(OutlineEffectSimple());
        }
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

        Debug.Log($"{gameObject.name}이 주변 몬스터를 회복시켰습니다 (+{healAmount})");
    }

    private IEnumerator OutlineEffectSimple()
    {
        if (outlineRenderer == null) yield break;

        outlineRenderer.enabled = true;
        yield return new WaitForSeconds(outlineShowTime);
        outlineRenderer.enabled = false;
    }
}
