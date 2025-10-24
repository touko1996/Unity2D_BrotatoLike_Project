using UnityEngine;

// �÷��̾� �̵� �� ���� ��ȯ
public class PlayerMove : MonoBehaviour
{
    [Header("�̵� ����")]
    [SerializeField] private float moveSpeed = 5.0f;

    private Rigidbody2D _rigidbody2D;
    private Vector2 _moveInput;
    private SpriteRenderer _spriteRenderer;
    private PlayerAnimation _playerAnimation;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _playerAnimation = GetComponent<PlayerAnimation>();
    }

    private void Update()
    {
        // �̵� �Է�
        _moveInput.x = Input.GetAxisRaw("Horizontal");
        _moveInput.y = Input.GetAxisRaw("Vertical");
        _moveInput.Normalize();

        // �¿� ���⿡ ���� ��������Ʈ �ø�
        if (_moveInput.x > 0)
            _spriteRenderer.flipX = false;
        else if (_moveInput.x < 0)
            _spriteRenderer.flipX = true;

        // �̵� ������ �Ǻ�
        bool isMoving = Mathf.Abs(_moveInput.x) > 0.1f || Mathf.Abs(_moveInput.y) > 0.1f;
        _playerAnimation.SetRunning(isMoving);
    }

    private void FixedUpdate()
    {
        // ���� �̵� ó�� (������ ������)
        _rigidbody2D.MovePosition(_rigidbody2D.position + _moveInput * moveSpeed * Time.fixedDeltaTime);
    }
}
