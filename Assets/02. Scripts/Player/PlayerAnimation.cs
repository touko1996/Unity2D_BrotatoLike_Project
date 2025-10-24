using UnityEngine;

// �÷��̾� ������ �� �̵� �ִϸ��̼�
public class PlayerAnimation : MonoBehaviour
{
    [Header("�ִϸ��̼� ����")]
    [SerializeField] private float scaleAmount = 0.05f; // ũ�� ��ȭ ��
    [SerializeField] private float speed = 8f;          // �⺻ ������ �ӵ�
    [SerializeField] private bool isRunning = false;    // �̵� �� ����

    private Vector3 _originalScale;

    private void Start()
    {
        _originalScale = transform.localScale;
    }

    private void Update()
    {
        // �̵� ���̸� ������ �ӵ� 2��
        float currentSpeed = isRunning ? speed * 2f : speed;
        float newY = _originalScale.y + Mathf.Sin(Time.time * currentSpeed) * scaleAmount;

        transform.localScale = new Vector3(_originalScale.x, newY, _originalScale.z);
    }

    // �ܺο��� �̵� ���θ� ���޹���
    public void SetRunning(bool running)
    {
        isRunning = running;
    }
}
