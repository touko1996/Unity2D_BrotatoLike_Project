using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;
    private PlayerStats _playerStats;
    private SpriteRenderer _spriteRenderer;
    private PlayerAnimation _playerAnimation; 

    private Vector2 _moveInput;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _playerStats = GetComponent<PlayerStats>();
        _playerAnimation = GetComponent<PlayerAnimation>(); 
    }

    private void Update()
    {
        // 입력 처리
        _moveInput.x = Input.GetAxisRaw("Horizontal");
        _moveInput.y = Input.GetAxisRaw("Vertical");
        _moveInput.Normalize();

        // 좌우 반전 처리
        if (_moveInput.x > 0) _spriteRenderer.flipX = false;
        else if (_moveInput.x < 0) _spriteRenderer.flipX = true;

        // 이동 중 여부 계산
        bool isMoving = _moveInput.sqrMagnitude > 0.01f;

        // PlayerAnimation에 이동 여부 전달
        if (_playerAnimation != null)
            _playerAnimation.SetRunning(isMoving);

       
    }

    private void FixedUpdate()
    {
        if (_playerStats == null) return;

        // PlayerStats의 이동 속도 사용
        float currentSpeed = _playerStats.currentMoveSpeed;

        // Rigidbody 이동
        _rigidbody2D.MovePosition(
            _rigidbody2D.position + _moveInput * currentSpeed * Time.fixedDeltaTime
        );
    }
}
