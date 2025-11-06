using UnityEngine;
using System.Collections;

public class ChargingMonster : Monster
{
    [Header("차징 몬스터 설정")]
    [SerializeField] private float chargeRange = 5f;     // 돌진 발동 거리
    [SerializeField] private float chargeSpeed = 30f;    // 돌진 속도
    [SerializeField] private float chargeDelay = 1f;     // 돌진 전 멈춤 시간
    [SerializeField] private float chargeTime = 1.2f;    // 돌진 지속시간
    [SerializeField] private Color chargeWarningColor = Color.red; // 전조 색상
    [SerializeField] private float flashSpeed = 8f;      // 반짝임 속도

    private bool isCharging = false;
    private bool isWaiting = false;
    private Color defaultColor;
    private Coroutine chargeRoutine;

    protected override void Start()
    {
        base.Start();
        if (spriteRenderer != null)
            defaultColor = spriteRenderer.color;
    }

    protected override void FixedUpdate()
    {
        // Time.timeScale이 0일 때(스탯 선택창 등) 멈춤
        if (Time.timeScale == 0f) return;
        if (player == null) return;

        float distance = Vector2.Distance(player.position, transform.position);

        // 돌진 준비 조건
        if (!isCharging && !isWaiting && distance <= chargeRange)
        {
            chargeRoutine = StartCoroutine(Charge());
        }

        // 돌진 중/준비 중 아닐 때만 기본 이동
        if (!isCharging && !isWaiting)
            base.Move();
    }

    private IEnumerator Charge()
    {
        if (isCharging || isWaiting)
            yield break; // 중복 돌진 방지

        isWaiting = true;
        float timer = 0f;

        // 전조 반짝임
        while (timer < chargeDelay)
        {
            if (spriteRenderer != null)
            {
                float t = Mathf.PingPong(Time.time * flashSpeed, 1f);
                spriteRenderer.color = Color.Lerp(defaultColor, chargeWarningColor, t);
            }

            timer += Time.deltaTime;
            yield return null;
        }

        // 색상 복원
        if (spriteRenderer != null)
            spriteRenderer.color = defaultColor;

        isWaiting = false;
        isCharging = true;

        if (player == null)
        {
            ResetState();
            yield break;
        }

        Vector2 dir = (player.position - transform.position).normalized;
        timer = 0f;

        // 돌진
        while (timer < chargeTime)
        {
            if (float.IsNaN(dir.x) || float.IsNaN(dir.y)) break;

            rb.MovePosition(rb.position + dir * chargeSpeed * Time.deltaTime);

            // 방향 유지
            if (player != null)
            {
                float deltaX = player.position.x - transform.position.x;
                spriteRenderer.flipX = deltaX < 0;
            }

            timer += Time.deltaTime;
            yield return null;
        }

        ResetState();
    }

    // 부모의 OnCollisionEnter2D 유지 (contactDamage 계산 정상 작동)
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);

        // 돌진 중 충돌 시 즉시 종료
        if (isCharging)
        {
            Debug.Log($"{gameObject.name} 돌진 중 충돌 발생 → 돌진 종료");
            StopCharge();
        }
    }

    // 돌진 강제 중단
    private void StopCharge()
    {
        if (chargeRoutine != null)
        {
            StopCoroutine(chargeRoutine);
            chargeRoutine = null;
        }

        ResetState();
    }

    // 상태 및 색상 초기화
    private void ResetState()
    {
        isCharging = false;
        isWaiting = false;

        if (spriteRenderer != null)
            spriteRenderer.color = defaultColor;

        if (rb != null)
            rb.velocity = Vector2.zero;
    }

    private new void OnEnable()
    {
        ResetState();
    }

}
