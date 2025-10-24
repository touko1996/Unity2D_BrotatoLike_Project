using UnityEngine;

// 플레이어 이동 및 방향 전환
public class PlayerMove : MonoBehaviour
{
    [Header("이동 설정")]
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
        // 이동 입력
        _moveInput.x = Input.GetAxisRaw("Horizontal");
        _moveInput.y = Input.GetAxisRaw("Vertical");
        _moveInput.Normalize();

        // 좌우 방향에 따라 스프라이트 플립
        if (_moveInput.x > 0)
            _spriteRenderer.flipX = false;
        else if (_moveInput.x < 0)
            _spriteRenderer.flipX = true;

        // 이동 중인지 판별
        bool isMoving = Mathf.Abs(_moveInput.x) > 0.1f || Mathf.Abs(_moveInput.y) > 0.1f;
        _playerAnimation.SetRunning(isMoving);
    }

    private void FixedUpdate()
    {
        // 실제 이동 처리 (프레임 독립적)
        _rigidbody2D.MovePosition(_rigidbody2D.position + _moveInput * moveSpeed * Time.fixedDeltaTime);
    }
}
