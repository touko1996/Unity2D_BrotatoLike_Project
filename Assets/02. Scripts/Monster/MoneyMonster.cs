using UnityEngine;

/// <summary>
/// [MoneyMonster]
/// ------------------------------------------------------------
/// �Ӵ� ���ʹ� �÷��̾ �������� �ʰ�,
/// PerlinWander�� ���� �ε巴�� ��ȸ�� �ϴ� �����̴�.
///
/// �� Ư¡
/// - Perlin Noise ������� �ڿ������� ���� �̵��� ����.
/// - �÷��̾�� �浹 �� �θ� Monster�� OnCollisionEnter2D�� ����
///   �������� ������.
/// - ���� �ݰ� ������ ��� �� ������ ���� �� ����Ѵ�.
///
/// �� �̵� ����
/// ���� �̵��� Monster�� moveSpeed�� �ƴ϶�
/// PerlinWander�� moveSpeed�� ���� �����ȴ�.
/// ------------------------------------------------------------
/// </summary>
public class MoneyMonster : Monster
{
    [Header("�Ӵ� ���� ����")]
    [Tooltip("�ּ� ��� ����")]
    [SerializeField] private int minCoinDrop = 3;

    [Tooltip("�ִ� ��� ����")]
    [SerializeField] private int maxCoinDrop = 7;

    [Tooltip("����� ���� ������")]
    [SerializeField] private GameObject coinPrefab;

    [Tooltip("������ ���� �ݰ�")]
    [SerializeField] private float dropRadius = 1.5f;

    private PerlinWander wander;

    protected override void Start()
    {
        base.Start();

        // PerlinWander ������Ʈ�� ã�Ƽ� �ڵ� ��ȸ ����
        wander = GetComponent<PerlinWander>();
        if (wander != null)
        {
            wander.StartWander();
        }
    }

    protected override void Update()
    {
        // �÷��̾� ���� ������ ������ ��Ȱ��ȭ
        // PerlinWander�� �ڵ����� �̵��� ó���ϹǷ� Update�� ����д�.
    }

    protected override void Die()
    {
        Debug.Log($"{gameObject.name} ��� - ���� ���!");

        int coinCount = Random.Range(minCoinDrop, maxCoinDrop + 1);

        for (int i = 0; i < coinCount; i++)
        {
            if (coinPrefab == null) continue;

            // DropRadius ���� �� ���� ��ġ�� ���� ����
            Vector2 offset = Random.insideUnitCircle * dropRadius;
            Instantiate(coinPrefab, transform.position + (Vector3)offset, Quaternion.identity);
        }

        gameObject.SetActive(false);
    }

    // �浹 �� �θ�(Monster)�� OnCollisionEnter2D�� �ڵ����� ȣ���
    // �� �÷��̾�� contactDamage�� ����
}
