using System.Collections;
using UnityEngine;

public class HealerMonster : Monster
{
    [Header("���� ���� ����")]
    [SerializeField] private float wanderRadius = 5f;      // ��ȸ ����
    [SerializeField] private float changeDirectionTime = 3f; // ���� ���� �ֱ�
    [SerializeField] private float fleeRange = 4f;          // �÷��̾� ���� �Ÿ�
    [SerializeField] private float fleeSpeed = 2f;          // ���� �ӵ�
    [SerializeField] private float healInterval = 4f;       // �� �ֱ�
    [SerializeField] private float healAmount = 5f;         // ȸ����

    [Header("����Ʈ ����")]
    [SerializeField] private float outlineShowTime = 0.4f;  // Outline ���� �ð�

    private Vector2 wanderDirection;
    private float directionTimer;
    private float healTimer;

    private SpriteRenderer outlineRenderer;

    protected override void Start()
    {
        base.Start();

        // Outline �ڽ� ������Ʈ Ž�� (Outline SpriteRenderer �ʼ�)
        var outline = transform.Find("Outline");
        if (outline != null)
            outlineRenderer = outline.GetComponent<SpriteRenderer>();

        if (outlineRenderer != null)
            outlineRenderer.enabled = false; // �⺻�� ����

        ChooseNewDirection();
    }

    protected override void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(player.position, transform.position);

        // �÷��̾ ������ ����
        if (distanceToPlayer <= fleeRange)
        {
            Vector2 dir = (transform.position - player.position).normalized;
            rb.MovePosition(rb.position + dir * fleeSpeed * Time.deltaTime);
            spriteRenderer.flipX = dir.x < 0;
        }
        else
        {
            // ������ ��ȸ
            directionTimer -= Time.deltaTime;
            if (directionTimer <= 0)
                ChooseNewDirection();

            rb.MovePosition(rb.position + wanderDirection * moveSpeed * Time.deltaTime);
            spriteRenderer.flipX = wanderDirection.x < 0;
        }

        // ���� �ֱ⸶�� �� �ߵ�
        healTimer -= Time.deltaTime;
        if (healTimer <= 0)
        {
            HealAllMonsters();
            healTimer = healInterval;
            StartCoroutine(OutlineEffectSimple()); // �� �� Outline �ѱ�
        }
    }

    private void ChooseNewDirection()
    {
        wanderDirection = Random.insideUnitCircle.normalized;
        directionTimer = changeDirectionTime;
    }

    private void HealAllMonsters()
    {
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");

        foreach (GameObject monsterObj in monsters)
        {
            Monster m = monsterObj.GetComponent<Monster>();
            if (m != null && m != this)
                m.Heal(healAmount);
        }

        Debug.Log($"{gameObject.name}��(��) �ֺ� ���͸� ȸ�����׽��ϴ�! (+{healAmount})");
    }

    private IEnumerator OutlineEffectSimple()
    {
        if (outlineRenderer == null) yield break;

        outlineRenderer.enabled = true;               // �ѱ�
        yield return new WaitForSeconds(outlineShowTime);
        outlineRenderer.enabled = false;              // ����
    }
}
