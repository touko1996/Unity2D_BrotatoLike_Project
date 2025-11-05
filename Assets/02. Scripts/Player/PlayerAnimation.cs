using UnityEngine;

// 플레이어 숨쉬기 및 이동 애니메이션
public class PlayerAnimation : MonoBehaviour
{
    [Header("애니메이션 설정")]
    [SerializeField] private float scaleAmount = 0.05f;  // 크기 변화 폭
    [SerializeField] private float idleSpeed = 4f;       // 정지 시 숨쉬기 속도
    [SerializeField] private float runSpeed = 8f;        // 이동 시 숨쉬기 속도
    [SerializeField] private bool isRunning = false;     // 이동 중 여부

    private Vector3 _originalScale;
    private float _breathTimer = 0f;

    private void Start()
    {
        _originalScale = transform.localScale;
    }

    private void Update()
    {
        float currentSpeed = isRunning ? runSpeed : idleSpeed;

        // TimeScale이 0이어도 계속 작동하도록 변경
        _breathTimer += Time.unscaledDeltaTime * currentSpeed;

        float newY = _originalScale.y + Mathf.Sin(_breathTimer) * scaleAmount;
        transform.localScale = new Vector3(_originalScale.x, newY, _originalScale.z);
    }

    public void SetRunning(bool running)
    {
        isRunning = running;
    }
}
