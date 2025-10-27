using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Header("����")]
    [SerializeField] private Rigidbody2D _rigidbody2D;
    [SerializeField] private PlayerStats _playerStats;

    private Vector2 _moveInput;

    void Awake()
    {
        // Rigidbody2D �ڵ� �Ҵ�
        if (_rigidbody2D == null)
            _rigidbody2D = GetComponent<Rigidbody2D>();

        // PlayerStats �ڵ� �Ҵ�
        if (_playerStats == null)
            _playerStats = GetComponent<PlayerStats>();
    }

    void Update()
    {
        // �Է� ó��
        _moveInput.x = Input.GetAxisRaw("Horizontal");
        _moveInput.y = Input.GetAxisRaw("Vertical");
        _moveInput.Normalize();
    }

    void FixedUpdate()
    {
        if (_playerStats == null) return;

        // �̵��ӵ��� PlayerStats�� currentMoveSpeed�� ����
        float currentSpeed = _playerStats.currentMoveSpeed;

        // Rigidbody �̵�
        _rigidbody2D.MovePosition(
            _rigidbody2D.position + _moveInput * currentSpeed * Time.fixedDeltaTime
        );
    }
}
