using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Header("참조")]
    [SerializeField] private Rigidbody2D _rigidbody2D;
    [SerializeField] private PlayerStats _playerStats;

    private Vector2 _moveInput;

    void Awake()
    {
        // Rigidbody2D 자동 할당
        if (_rigidbody2D == null)
            _rigidbody2D = GetComponent<Rigidbody2D>();

        // PlayerStats 자동 할당
        if (_playerStats == null)
            _playerStats = GetComponent<PlayerStats>();
    }

    void Update()
    {
        // 입력 처리
        _moveInput.x = Input.GetAxisRaw("Horizontal");
        _moveInput.y = Input.GetAxisRaw("Vertical");
        _moveInput.Normalize();
    }

    void FixedUpdate()
    {
        if (_playerStats == null) return;

        // 이동속도를 PlayerStats의 currentMoveSpeed로 통일
        float currentSpeed = _playerStats.currentMoveSpeed;

        // Rigidbody 이동
        _rigidbody2D.MovePosition(
            _rigidbody2D.position + _moveInput * currentSpeed * Time.fixedDeltaTime
        );
    }
}
