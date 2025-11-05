using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Settings")]
    public float lifetime = 2f;
    [HideInInspector] public float damage;

    [Header("References")]
    public GameObject damageTextPrefab; // 데미지 텍스트 프리팹
    public float bulletSpeed = 10f;     // 이동 속도

    private Vector2 _direction;
    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    // 외부(WeaponShooter)에서 방향 설정
    public void SetDirection(Vector2 dir)
    {
        _direction = dir.normalized;

        // 시각적 방향 정렬
        float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void FixedUpdate()
    {
        if (_rb != null)
            _rb.velocity = _direction * bulletSpeed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Monster"))
        {
            Monster monster = other.GetComponent<Monster>();
            if (monster != null)
            {
                monster.ReceiveDamage(damage);

                // 데미지 텍스트 생성
                if (damageTextPrefab != null)
                {
                    // DamageCanvas 찾기
                    GameObject canvasObj = GameObject.Find("DamageCanvas");
                    Transform parent = canvasObj != null ? canvasObj.transform : null;

                    // Canvas 밑에 생성 (없으면 루트에 생성)
                    GameObject dmgText = Instantiate(
                        damageTextPrefab,
                        monster.transform.position + Vector3.up * 0.3f,
                        Quaternion.identity,
                        parent
                    );

                    // 텍스트 세팅
                    DamageText text = dmgText.GetComponent<DamageText>();
                    if (text != null)
                        text.SetText(damage);
                }
            }

            Destroy(gameObject);
        }
    }
}
