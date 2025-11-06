using UnityEngine;
using System.Collections;

public class ChargingMonster : Monster
{
    [Header("차지 몬스터 설정")]
    [SerializeField] private float chargeTriggerRange = 5f;   // 돌진을 시작할 인식 거리
    [SerializeField] private float chargeMoveSpeed = 30f;     // 돌진 중 이동 속도
    [SerializeField] private float preChargeDelay = 1f;       // 돌진 전 대기(경고) 시간
    [SerializeField] private float chargeDuration = 1.3f;     // 돌진 유지 시간
    [SerializeField] private Color preChargeColor = Color.red; // 돌진 전 반짝임 색상
    [SerializeField] private float preChargeFlashSpeed = 8f;   // 경고 반짝임 속도

    private bool isCharging = false;     
    private bool isPreparing = false;             
    private Coroutine chargeCoroutine;   // 돌진 코루틴 캐싱용 (중복 실행 방지용)

    protected override void Start()
    {
        base.Start();
        if (spriteRenderer != null)
            originalColor = spriteRenderer.color;
    }

    // 매 프레임마다 이동 및 돌진 여부를 판정
    protected override void FixedUpdate()
    {
        if (Time.timeScale == 0f) return;
        if (playerTransform == null) return;

        float distanceToPlayer = Vector2.Distance(playerTransform.position, transform.position);

        // 플레이어가 인식 범위 안에 있고 돌진 중이 아닐 때 돌진 시작
        if (!isCharging && !isPreparing && distanceToPlayer <= chargeTriggerRange)
        {
            chargeCoroutine = StartCoroutine(PerformCharge());
        }

        // 돌진 중이 아니고 준비 중도 아닐 때만 기본 이동 유지
        if (!isCharging && !isPreparing)
            base.Move();
    }

    // 실제 돌진 수행 코루틴
    private IEnumerator PerformCharge()
    {
        // 돌진 중복 실행 방지
        if (isCharging || isPreparing)
            yield break;

        // 돌진 전 준비 단계
        isPreparing = true;
        float elapsed = 0f;

        // 돌진 전 붉은 반짝임 경고 연출
        while (elapsed < preChargeDelay)
        {
            if (spriteRenderer != null)
            {
                // Mathf.PingPong을 이용한 반복 반짝임 (0~1 사이 반복)
                float t = Mathf.PingPong(Time.time * preChargeFlashSpeed, 1f);
                spriteRenderer.color = Color.Lerp(originalColor, preChargeColor, t);
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        // 색상 복원 후 돌진 시작
        if (spriteRenderer != null)
            spriteRenderer.color = originalColor;

        isPreparing = false;
        isCharging = true;

        // 플레이어가 사라졌다면 중단
        if (playerTransform == null)
        {
            ResetChargeState();
            yield break;
        }

        Vector2 chargeDirection = (playerTransform.position - transform.position).normalized;
        elapsed = 0f;

        // 돌진 구간
        while (elapsed < chargeDuration)
        {
            // 방향이 유효하지 않으면 중단
            if (float.IsNaN(chargeDirection.x) || float.IsNaN(chargeDirection.y))
                break;

            // Rigidbody2D를 이용해 빠르게 이동
            rigidBody.MovePosition(rigidBody.position + chargeDirection * chargeMoveSpeed * Time.deltaTime);

            // 돌진 중에도 플레이어 방향으로 스프라이트 반전 유지
            if (playerTransform != null)
            {
                float deltaX = playerTransform.position.x - transform.position.x;
                spriteRenderer.flipX = deltaX < 0;
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        // 돌진 종료 후 상태 초기화
        ResetChargeState();
    }

    // 플레이어 또는 벽과 충돌 시 처리
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);

        // 돌진 중 충돌 시 즉시 중단
        if (isCharging)
            StopChargeImmediately();
    }

    // 돌진 강제 중단
    private void StopChargeImmediately()
    {
        if (chargeCoroutine != null)
        {
            StopCoroutine(chargeCoroutine);
            chargeCoroutine = null;
        }

        ResetChargeState();
    }

    // 돌진 상태 및 색상 초기화
    private void ResetChargeState()
    {
        isCharging = false;
        isPreparing = false;

        if (spriteRenderer != null)
            spriteRenderer.color = originalColor;

        if (rigidBody != null)
            rigidBody.velocity = Vector2.zero;
    }

    
    protected override void ResetMonsterState()
    {
        base.ResetMonsterState(); 
        ResetChargeState();       
    }
}
