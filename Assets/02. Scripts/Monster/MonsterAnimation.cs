using UnityEngine;

// ���� ������ ���� ũ�� ��ȭ ȿ��
public class MonsterAnimation : MonoBehaviour
{
    [Header("ũ�� ��ȭ ����")]
    [SerializeField] private float scaleAmount = 0.05f; //ũ�� ��ȭ��
    [SerializeField] private float speed = 8f; //���ϴ� �ӵ�

    private Vector3 _originalScale;

    private void Start()
    {
        _originalScale = transform.localScale;
    }

    private void Update()
    {
        float newY = _originalScale.y + Mathf.Sin(Time.time * speed) * scaleAmount; //Mathf.Sin�� -1���� 1���� �ε巴�� �ݺ��ϴ� ��Լ�
        transform.localScale = new Vector3(_originalScale.x, newY, _originalScale.z);
    }
}
