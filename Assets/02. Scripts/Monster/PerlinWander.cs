using UnityEngine;

/// <summary>
/// Perlin 노이즈를 기반으로 자연스럽게 배회하는 이동 컴포넌트
/// - 단순한 랜덤 이동이 아니라, Perlin Noise를 이용해 "부드럽게 방향이 바뀌는" 움직임을 구현
/// - 주로 HealerMonster 등의 보조형 몬스터가 사용
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class PerlinWander : MonoBehaviour
{
    [Header("이동 설정")]
    [Tooltip("기본 이동 속도")]
    [SerializeField] private float moveSpeed = 2f;

    [Tooltip("노이즈 진행 속도 (값이 클수록 방향이 자주 바뀜)")]
    [SerializeField] private float noiseChangeSpeed = 0.6f;

    [Tooltip("노이즈 세기 (값이 클수록 방향 변화 폭이 커짐)")]
    [SerializeField] private float noiseIntensity = 1f;

    private Rigidbody2D rigidBody;          // 이동용 Rigidbody
    private SpriteRenderer spriteRenderer;  // 방향 반전을 위한 SpriteRenderer
    private float noiseOffsetX;             // X축 노이즈 시작 지점 (랜덤)
    private float noiseOffsetY;             // Y축 노이즈 시작 지점 (랜덤)
    private bool isWandering = true;        // 배회 활성화 여부

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // PerlinNoise의 시드(Offset)를 랜덤으로 설정해 각 개체마다 다른 패턴 생성
        noiseOffsetX = Random.Range(0f, 1000f);
        noiseOffsetY = Random.Range(0f, 1000f);
    }

    private void Update()
    {
        if (!isWandering) return; // 비활성화 상태면 이동하지 않음

        // 시간에 따라 PerlinNoise를 변화시켜 방향 벡터 계산
        float t = Time.time * noiseChangeSpeed;

        // PerlinNoise는 0~1 범위를 반환하므로, (-1 ~ 1) 범위로 변환
        float noiseX = Mathf.PerlinNoise(noiseOffsetX + t, 0f) * 2f - 1f;
        float noiseY = Mathf.PerlinNoise(0f, noiseOffsetY + t) * 2f - 1f;

        Vector2 direction = new Vector2(noiseX, noiseY).normalized;

        // Rigidbody를 이용해 프레임 독립적인 이동 처리
        rigidBody.MovePosition(rigidBody.position + direction * moveSpeed * noiseIntensity * Time.deltaTime);

        // 이동 방향에 따라 스프라이트 좌우 반전
        if (spriteRenderer != null)
        {
            if (direction.x < 0f)
                spriteRenderer.flipX = true;
            else if (direction.x > 0f)
                spriteRenderer.flipX = false;
        }
    }

    /// <summary>
    /// 이동 속도를 실시간으로 변경 (외부에서 호출 가능)
    /// </summary>
    public void SetMoveSpeed(float speed)
    {
        moveSpeed = speed;
    }

    /// <summary>
    /// 배회 시작 (HealerMonster가 플레이어와 멀어질 때 호출)
    /// </summary>
    public void StartWander()
    {
        isWandering = true;
    }

    /// <summary>
    /// 배회 정지 (HealerMonster가 플레이어 근처일 때 호출)
    /// </summary>
    public void StopWander()
    {
        isWandering = false;
    }
}
