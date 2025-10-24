using UnityEngine;

// ���� ������ ���� ũ�� ��ȭ ȿ��
public class MonsterAnimation : MonoBehaviour
{
    [Header("ũ�� ��ȭ ����")]
    [SerializeField] private float scaleAmount = 0.05f;
    [SerializeField] private float speed = 8f;

    private Vector3 _originalScale;

    private void Start()
    {
        _originalScale = transform.localScale;
    }

    private void Update()
    {
        float newY = _originalScale.y + Mathf.Sin(Time.time * speed) * scaleAmount;
        transform.localScale = new Vector3(_originalScale.x, newY, _originalScale.z);
    }
}
