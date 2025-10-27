using UnityEngine;
using System.Collections;

public class ChargingMonster : Monster
{
    [Header("차징 몬스터 설정")]
    [SerializeField] private float chargeRange = 5f;     // 돌진 발동 거리
    [SerializeField] private float chargeSpeed = 30f;    // 돌진 속도
    [SerializeField] private float chargeDelay = 1f;     // 돌진 전 멈춤 시간
    [SerializeField] private float chargeDamage = 20f;   // 돌진 데미지
    [SerializeField] private float chargeTime = 1.2f;    // 돌진 지속시간
    [SerializeField] private Color chargeWarningColor = Color.red; // 전조 색상
    [SerializeField] private float flashSpeed = 8f;      // 반짝임 속도

    private bool isCharging = false;
    private bool isWaiting = false;
    private Color defaultColor;

    protected override void Start()
    {
        base.Start();
        if (spriteRenderer != null)
            defaultColor = spriteRenderer.color;
    }

    protected override void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(player.position, transform.position);

        // 돌진 준비 조건
        if (!isCharging && !isWaiting && distance <= chargeRange)
        {
            StartCoroutine(Charge());
        }

        // 돌진 중/준비 중 아닐 때만 일반 이동
        if (!isCharging && !isWaiting)
            base.Move();
    }

    private IEnumerator Charge()
    {
        // 전조 상태
        isWaiting = true;
        float timer = 0f;

        while (timer < chargeDelay)
        {
            // 스프라이트 반짝임 효과
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

        // 돌진 시작
        isCharging = true;
        Vector2 dir = (player.position - transform.position).normalized;
        timer = 0f;

        while (timer < chargeTime)
        {
            rb.MovePosition(rb.position + dir * chargeSpeed * Time.deltaTime);

            // 돌진 중에도 flip 유지
            if (player != null)
            {
                float deltaX = player.position.x - transform.position.x;
                spriteRenderer.flipX = deltaX < 0;
            }

            timer += Time.deltaTime;
            yield return null;
        }

        isCharging = false;
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            float damage = isCharging ? chargeDamage : contactDamage;
            Debug.Log($"{gameObject.name}이(가) 플레이어에게 {damage} 데미지 (돌진 여부: {isCharging})");
        }
    }
}
