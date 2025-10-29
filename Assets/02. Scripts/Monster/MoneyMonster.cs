using UnityEngine;

public class MoneyMonster : Monster
{
    [Header("�Ӵ� ���� ����")]
    [SerializeField] private int minCoinDrop = 3;        // �ּ� ��� ����
    [SerializeField] private int maxCoinDrop = 7;        // �ִ� ��� ����
    [SerializeField] private float wanderRadius = 5f;    // ��ȸ ����
    [SerializeField] private float changeDirectionTime = 3f; // ���� ���� �ֱ�
    [SerializeField] private GameObject coinPrefab;      // ����� ���� ������
    [SerializeField] private float dropRadius = 1.5f;    // ���� ���� �ݰ�

    private Vector2 wanderDirection;
    private float directionTimer;

    protected override void Start()
    {
        base.Start();
        ChooseNewDirection();
    }

    protected override void Update()
    {
        // �ܼ� ��ȸ�� �� (�÷��̾� �߰� X)
        directionTimer -= Time.deltaTime;
        if (directionTimer <= 0)
            ChooseNewDirection();

        rb.MovePosition(rb.position + wanderDirection * moveSpeed * Time.deltaTime);
        spriteRenderer.flipX = wanderDirection.x < 0;
    }

    private void ChooseNewDirection()
    {
        wanderDirection = Random.insideUnitCircle.normalized;
        directionTimer = changeDirectionTime;
    }

    protected override void Die()
    {
        Debug.Log($"{gameObject.name} ��� - ���� ���!");

        int coinCount = Random.Range(minCoinDrop, maxCoinDrop + 1);

        for (int i = 0; i < coinCount; i++)
        {
            if (coinPrefab == null) continue;

            // DropRadius ���� �� ���� ��ġ�� ��� ����
            Vector2 offset = Random.insideUnitCircle * dropRadius;
            Instantiate(coinPrefab, transform.position + (Vector3)offset, Quaternion.identity);
        }

        gameObject.SetActive(false);
    }
}
