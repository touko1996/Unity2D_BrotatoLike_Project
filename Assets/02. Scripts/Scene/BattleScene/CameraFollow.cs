using UnityEngine;
using System.Collections;

/// <summary>
/// [CameraFollow]
/// ------------------------------------------------------------
/// 플레이어(또는 지정 타겟)를 따라가는 카메라 제어 스크립트.
/// - 부드러운 추적 (Lerp)
/// - 지정된 영역 내에서만 이동
/// - 카메라 흔들림(Shake) 효과 지원
/// ------------------------------------------------------------
/// </summary>
public class CameraFollow : MonoBehaviour
{
    [Header("타겟 설정")]
    [SerializeField] private Transform target; // 추적할 대상

    [Header("카메라 이동 제한 (바운더리)")]
    [SerializeField] private float minX = -10f;
    [SerializeField] private float maxX = 10f;
    [SerializeField] private float minY = -10f;
    [SerializeField] private float maxY = 10f;

    [Header("부드러운 추적 설정")]
    [SerializeField] private float followSmoothSpeed = 5f;

    [Header("카메라 흔들림 설정")]
    [SerializeField] private float defaultShakeDuration = 0.2f;  // 기본 흔들림 지속시간
    [SerializeField] private float defaultShakeMagnitude = 0.2f; // 기본 흔들림 세기

    private float camHalfHeight;
    private float camHalfWidth;
    private Vector3 originalPosition;
    private bool isShaking = false;
    private Coroutine shakeCoroutine;

    private Camera mainCam;

    // 전역 접근용 인스턴스
    public static CameraFollow Instance { get; private set; }

    private void Awake()
    {
        // 싱글톤 초기화 (중복 방지)
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        mainCam = GetComponent<Camera>();
    }

    private void Start()
    {
        if (mainCam == null)
            mainCam = GetComponent<Camera>();

        camHalfHeight = mainCam.orthographicSize;
        camHalfWidth = camHalfHeight * mainCam.aspect;
        originalPosition = transform.position;
    }

    private void LateUpdate()
    {
        if (isShaking) return; // 흔들림 중에는 위치 고정
        if (target == null) return;

        // 목표 위치 계산
        Vector3 desiredPos = new Vector3(target.position.x, target.position.y, transform.position.z);

        // 카메라 이동 범위 제한 (Clamp) - 값이 특정 범위를 벗어나지 않도록 안전하게 제한
        float clampedX = Mathf.Clamp(desiredPos.x, minX + camHalfWidth, maxX - camHalfWidth);
        float clampedY = Mathf.Clamp(desiredPos.y, minY + camHalfHeight, maxY - camHalfHeight);
        Vector3 clampedPos = new Vector3(clampedX, clampedY, desiredPos.z);

        // 부드러운 이동 (Lerp)
        transform.position = Vector3.Lerp(transform.position, clampedPos, followSmoothSpeed * Time.deltaTime);
        originalPosition = transform.position;
    }

    /// <summary>
    /// 카메라 흔들림 시작 (기본 세기/지속시간 또는 인자 지정)
    /// </summary>
    public void ShakeCamera(float duration = -1f, float magnitude = -1f)
    {
        if (!gameObject.activeInHierarchy) return;
        if (isShaking) return;

        if (duration <= 0f) duration = defaultShakeDuration;
        if (magnitude <= 0f) magnitude = defaultShakeMagnitude;

        shakeCoroutine = StartCoroutine(ShakeRoutine(duration, magnitude));
    }

    /// <summary>
    /// 카메라 흔들림 중단 (플레이어 사망 시 호출)
    /// </summary>
    public void StopShake()
    {
        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
            shakeCoroutine = null;
        }

        isShaking = false;
        transform.position = originalPosition;
    }

    /// <summary>
    /// 실제 흔들림 코루틴
    /// </summary>
    private IEnumerator ShakeRoutine(float duration, float magnitude)
    {
        isShaking = true;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            // TimeScale이 0일 때는 흔들림 중단 (게임 일시정지 예외 처리)
            if (Time.timeScale == 0f)
                break;

            elapsed += Time.deltaTime;
            float offsetX = Random.Range(-1f, 1f) * magnitude;
            float offsetY = Random.Range(-1f, 1f) * magnitude;

            transform.position = originalPosition + new Vector3(offsetX, offsetY, 0f);
            yield return null;
        }

        transform.position = originalPosition;
        isShaking = false;
    }
}
