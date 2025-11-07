using UnityEngine;

// 몬스터 숨쉬는 듯한 크기 변화 효과
public class MonsterAnimation : MonoBehaviour
{
    [Header("크기 변화 설정")]
    [SerializeField] private float scaleAmount = 0.03f; // 크기 변화폭
    [SerializeField] private float speed = 8f;          // 변하는 속도

    private Vector3 _originalScale;
    private bool _isPaused = false; // 숨쉬기 멈춤 여부

    private void Start()
    {
        _originalScale = transform.localScale;
    }

    private void Update()
    {
        if (_isPaused) return; // 멈춰있다면 애니메이션 중단

        float newY = _originalScale.y + Mathf.Sin(Time.time * speed) * scaleAmount;
        transform.localScale = new Vector3(_originalScale.x, newY, _originalScale.z);
    }

    // 외부에서 숨쉬기 멈추기 / 재개시키기
    public void SetPaused(bool paused)
    {
        _isPaused = paused;
    }
}
