using UnityEngine;

// 플레이어 숨쉬기 및 이동 애니메이션
public class PlayerAnimation : MonoBehaviour
{
    [Header("애니메이션 설정")]
    [SerializeField] private float scaleAmount = 0.05f;  // 크기 변화 폭
    [SerializeField] private float idleBreathSpeed = 4f;       // 정지 시 숨쉬기 속도
    [SerializeField] private float runBreathSpeed = 8f;        // 이동 시 숨쉬기 속도
    [SerializeField] private bool isRunning = false;     // 이동 중 여부

    private Vector3 originalScale;
    private float breathTimer = 0f;

    private void Start()
    {
        originalScale = transform.localScale;
    }

    private void Update()
    {
        float currentBreathSpeed = isRunning ? runBreathSpeed : idleBreathSpeed;

        // TimeScale의 영향을 받도록 변경 (일시정지 시 애니메이션 멈춤)
        breathTimer += Time.deltaTime * currentBreathSpeed;

        float newY = originalScale.y + Mathf.Sin(breathTimer) * scaleAmount; //Mathf.Sin은 -1에서 1사이 값을 반환, 부드러운 숨쉬기 모션재현
        transform.localScale = new Vector3(originalScale.x, newY, originalScale.z);
    }

    public void SetRunning(bool running)
    {
        isRunning = running;
    }
}
