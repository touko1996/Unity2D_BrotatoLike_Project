using UnityEngine;
using System.Collections;

public class ChargingMonster : Monster
{
    [Header("��¡ ���� ����")]
    [SerializeField] private float chargeRange = 5f;     // ���� �ߵ� �Ÿ�
    [SerializeField] private float chargeSpeed = 30f;    // ���� �ӵ�
    [SerializeField] private float chargeDelay = 1f;     // ���� �� ���� �ð�
    [SerializeField] private float chargeTime = 1.2f;    // ���� ���ӽð�
    [SerializeField] private Color chargeWarningColor = Color.red; // ���� ����
    [SerializeField] private float flashSpeed = 8f;      // ��¦�� �ӵ�

    private bool isCharging = false;
    private bool isWaiting = false;
    private Color defaultColor;

    protected override void Start()
    {
        base.Start();
        if (spriteRenderer != null)
            defaultColor = spriteRenderer.color;
    }

    protected override void Update()
    {
        // Time.timeScale�� 0�� ��(���� ����â ��) ����
        if (Time.timeScale == 0f) return;
        if (player == null) return;

        float distance = Vector2.Distance(player.position, transform.position);

        // ���� �غ� ����
        if (!isCharging && !isWaiting && distance <= chargeRange)
        {
            StartCoroutine(Charge());
        }

        // ���� ��/�غ� �� �ƴ� ���� �⺻ �̵�
        if (!isCharging && !isWaiting)
            base.Move();
    }

    private IEnumerator Charge()
    {
        if (isCharging || isWaiting)
            yield break; // �ߺ� ���� ����

        // ���� ����
        isWaiting = true;
        float timer = 0f;

        while (timer < chargeDelay)
        {
            if (spriteRenderer != null)
            {
                float t = Mathf.PingPong(Time.time * flashSpeed, 1f);
                spriteRenderer.color = Color.Lerp(defaultColor, chargeWarningColor, t);
            }

            timer += Time.deltaTime;
            yield return null;
        }

        // ���� ����
        if (spriteRenderer != null)
            spriteRenderer.color = defaultColor;

        isWaiting = false;
        isCharging = true;

        Vector2 dir = (player.position - transform.position).normalized;
        timer = 0f;

        while (timer < chargeTime)
        {
            rb.MovePosition(rb.position + dir * chargeSpeed * Time.deltaTime);

            // ���� �߿��� ���� ����
            if (player != null)
            {
                float deltaX = player.position.x - transform.position.x;
                spriteRenderer.flipX = deltaX < 0;
            }

            timer += Time.deltaTime;
            yield return null;
        }

        isCharging = false;
    }

    //  �θ��� OnCollisionEnter2D ���� (contactDamage ��� ���� �۵�)
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision); // �⺻ ���� ������ ȣ��

        // ���� �� �浹 �� ���� ����
        if (collision.collider.CompareTag("Player") && isCharging)
        {
            isCharging = false;
            Debug.Log($"{gameObject.name} ���� �� �÷��̾�� �浹 �� ���� ����");
        }
    }
}
