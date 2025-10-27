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
        // 입력 처리
        _moveInput.x = Input.GetAxisRaw("Horizontal");
        _moveInput.y = Input.GetAxisRaw("Vertical");
        _moveInput.Normalize();
        if(_moveInput.x > 0) _spriteRenderer.flipX = false;
        else if (_moveInput.x < 0) _spriteRenderer.flipX = true;

    }

    void FixedUpdate()
    {
        if (_playerStats == null) return;

        // 이동속도를 PlayerStats의 currentMoveSpeed로 통일
        float currentSpeed = _playerStats.currentMoveSpeed;

        // Rigidbody 이동
        _rigidbody2D.MovePosition(_rigidbody2D.position + _moveInput * currentSpeed * Time.fixedDeltaTime);
    }
}
