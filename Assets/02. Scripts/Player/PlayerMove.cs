using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    
    private Rigidbody2D _rigidbody2D;
    private PlayerStats _playerStats;
    private SpriteRenderer _spriteRenderer;

    private Vector2 _moveInput;

    void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _playerStats = GetComponent<PlayerStats>();
    }

    void Update()
    {
        // �Է� ó��
        _moveInput.x = Input.GetAxisRaw("Horizontal");
        _moveInput.y = Input.GetAxisRaw("Vertical");
        _moveInput.Normalize();
        if(_moveInput.x > 0) _spriteRenderer.flipX = false;
        else if (_moveInput.x < 0) _spriteRenderer.flipX = true;

    }

    void FixedUpdate()
    {
        if (_playerStats == null) return;

        // �̵��ӵ��� PlayerStats�� currentMoveSpeed�� ����
        float currentSpeed = _playerStats.currentMoveSpeed;

        // Rigidbody �̵�
        _rigidbody2D.MovePosition(_rigidbody2D.position + _moveInput * currentSpeed * Time.fixedDeltaTime);
    }
}
