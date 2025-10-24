using UnityEngine;

// 플레이어 숨쉬기 및 이동 애니메이션
public class PlayerAnimation : MonoBehaviour
{
    [Header("애니메이션 설정")]
    [SerializeField] private float scaleAmount = 0.05f; // 크기 변화 폭
    [SerializeField] private float speed = 8f;          // 기본 숨쉬기 속도
    [SerializeField] private bool isRunning = false;    // 이동 중 여부

    private Vector3 _originalScale;

    private void Start()
    {
        _originalScale = transform.localScale;
    }

    private void Update()
    {
        // 이동 중이면 숨쉬기 속도 2배
        float currentSpeed = isRunning ? speed * 2f : speed;
        float newY = _originalScale.y + Mathf.Sin(Time.time * currentSpeed) * scaleAmount;

        transform.localScale = new Vector3(_originalScale.x, newY, _originalScale.z);
    }

    // 외부에서 이동 여부를 전달받음
    public void SetRunning(bool running)
    {
        isRunning = running;
    }
}
