using UnityEngine;

// 몬스터 숨쉬는 듯한 크기 변화 효과
public class MonsterAnimation : MonoBehaviour
{
    [Header("크기 변화 설정")]
    [SerializeField] private float scaleAmount = 0.05f; //크기 변화폭
    [SerializeField] private float speed = 8f; //변하는 속도

    private Vector3 _originalScale;

    private void Start()
    {
        _originalScale = transform.localScale;
    }

    private void Update()
    {
        float newY = _originalScale.y + Mathf.Sin(Time.time * speed) * scaleAmount; //Mathf.Sin은 -1부터 1까지 부드럽게 반복하는 곡선함수
        transform.localScale = new Vector3(_originalScale.x, newY, _originalScale.z);
    }
}
