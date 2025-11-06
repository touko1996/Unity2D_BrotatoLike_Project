using UnityEngine;

/// <summary>
/// [Bullet]
/// --------------------------------------------------------------------
/// 플레이어 무기에서 발사되는 탄환.
/// - WeaponShooter에서 방향, 데미지, 속도를 전달받음.
/// - 일정 시간 후 자동 파괴.
/// - 몬스터와 충돌 시 데미지를 주고 DamageText를 출력.
/// --------------------------------------------------------------------
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    [Header("기본 설정")]
    [Tooltip("탄환의 생존 시간 (초 단위)")]
    [SerializeField] private float lifetime = 2f;

    [Tooltip("데미지 텍스트 프리팹")]
    [SerializeField] private GameObject damageTextPrefab;

    private float moveSpeed;               // WeaponData에서 전달받을 속도
    [HideInInspector] public float damage; // WeaponShooter에서 전달

    private Vector2 moveDirection;      
    private Rigidbody2D bulletRb;       

    private void Awake()
    {
        bulletRb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        // 일정 시간이 지나면 탄환 자동 파괴
        Destroy(gameObject, lifetime);
    }

    /// <summary>
    /// WeaponShooter에서 발사 방향 설정
    /// </summary>
    public void SetDirection(Vector2 direction)
    {
        moveDirection = direction.normalized;

        // 탄환의 시각적 회전 처리 (탄두 방향 정렬)
        float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    /// <summary>
    /// WeaponData.projectileSpeed 값을 전달받아 이동 속도 설정
    /// </summary>
    public void SetSpeed(float speed)
    {
        moveSpeed = speed;
    }

    private void FixedUpdate()
    {
        // Rigidbody를 이용한 프레임 독립적인 이동 처리
        if (bulletRb != null)
            bulletRb.velocity = moveDirection * moveSpeed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 몬스터와 충돌 시 데미지 처리
        if (!other.CompareTag("Monster"))
            return;

        Monster targetMonster = other.GetComponent<Monster>();
        if (targetMonster == null)
            return;

        // 몬스터에게 데미지 적용
        targetMonster.ReceiveDamage(damage);

        // 데미지 텍스트 생성
        if (damageTextPrefab != null)
            CreateDamageText(targetMonster);

        // 탄환 제거
        Destroy(gameObject);
    }

    /// <summary>
    /// 몬스터 피격 시 DamageText를 출력
    /// </summary>
    private void CreateDamageText(Monster target)
    {
        // DamageCanvas 오브젝트 탐색
        GameObject canvasObj = GameObject.Find("DamageCanvas");
        Transform parent = canvasObj != null ? canvasObj.transform : null;

        // DamageText 프리팹 생성 (몬스터 머리 위에 출력)
        GameObject dmgTextObj = Instantiate(
            damageTextPrefab,
            target.transform.position + Vector3.up * 0.3f,
            Quaternion.identity,
            parent
        );

        // 텍스트 설정
        DamageText dmgText = dmgTextObj.GetComponent<DamageText>();
        if (dmgText != null)
            dmgText.SetText(damage);
    }
}
