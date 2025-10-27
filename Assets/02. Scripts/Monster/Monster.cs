using UnityEngine;

// �⺻ ���� �̵� �� ü�� ó��
public class Monster : MonoBehaviour
{
    [Header("���� ����")]
    public float moveSpeed = 2f;
    public float hp = 30f;

    private Transform _player;
    private Rigidbody2D _rigidbody2D;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        if (_rigidbody2D != null)
        {
            _rigidbody2D.gravityScale = 0;
            _rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

    private void Start()
    {
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            _player = playerObj.transform;
        }
    }

    private void Update()
    {
        if (_player == null) return;

        // �÷��̾� ���� ���
        Vector2 direction = (_player.position - transform.position).normalized;

        // �̵� ó��
        if (_rigidbody2D != null)
            _rigidbody2D.MovePosition(_rigidbody2D.position + direction * moveSpeed * Time.deltaTime);
        else
            transform.position += (Vector3)direction * moveSpeed * Time.deltaTime;

        // �¿� ���⿡ ���� ��������Ʈ ����
        _spriteRenderer.flipX = _player.position.x < transform.position.x;
    }

    // ������ ó��
    public void ReceiveDamage(float damage)
    {
        hp -= damage;
        if (hp <= 0)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // �÷��̾� �浹 �� ���� (���� ����)
        }
    }
}
