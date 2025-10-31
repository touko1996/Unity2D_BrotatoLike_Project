using System.Collections;
using UnityEngine;

/// <summary>
/// [HealerMonster]
/// ------------------------------------------------------------
/// ���� ���ʹ� �������ٴ� ������ AI ������ ���� ���ͷ�,
/// �÷��̾ ������ ���� ����ġ��,
/// �÷��̾ �ָ� ������ PerlinWander�� ���� �ε巴�� ��ȸ�ϸ� ���ƴٴѴ�.
///
/// ���� ���� �ֱ⸶�� �ֺ��� �ٸ� ���͸� ��(ȸ��)�ϴ� ����� �����Ѵ�.
///
/// ���� ���:
/// 1. PerlinWander ������Ʈ�� �̿��� ��ȸ �̵�
/// 2. �÷��̾�� ���� �Ÿ� ���Ϸ� ��������� ���� (wander ��Ȱ��)
/// 3. ���� �ð����� �ֺ� ���� HP ȸ��
/// 4. �� �� Outline ȿ�� ǥ��
///
/// PerlinWander ���� ���:
/// - �̵� ������ HealerMonster�� �ƴ϶� PerlinWander���� ���
/// - HealerMonster�� �ܼ��� ���� ��ȸ����, �������� ��� ��
///   (wander.StartWander(), wander.StopWander())
///
/// �˰��� ����Ʈ:
/// Perlin Noise ��� �̵� �˰����� Ȱ���Ͽ�
/// �ܼ� ���� �̵��� �ƴ�, �ε巴�� ����ü ���� �������� ������.
/// ------------------------------------------------------------
/// </summary>
public class HealerMonster : Monster
{
    [Header("���� ���� ����")]
    [Tooltip("�÷��̾ �� �Ÿ� ������ ������ ���� ����")]
    [SerializeField] private float fleeRange = 4f;

    [Tooltip("����ĥ ���� �ӵ�")]
    [SerializeField] private float fleeSpeed = 2f;

    [Tooltip("�� �ߵ� �ֱ� (�� ����)")]
    [SerializeField] private float healInterval = 4f;

    [Tooltip("ȸ����ų HP ��")]
    [SerializeField] private float healAmount = 5f;

    [Tooltip("Outline(������) ȿ�� ���� �ð�")]
    [SerializeField] private float outlineShowTime = 0.4f;

    private float healTimer; // �� �ֱ� Ÿ�̸�
    private SpriteRenderer outlineRenderer; // �� �� ǥ�õǴ� �ܰ��� ��������Ʈ
    private PerlinWander wander; // Perlin ��� ��ȸ �����

    protected override void Start()
    {
        base.Start();

        // PerlinWander ������Ʈ�� ã�� ��ȸ ����
        wander = GetComponent<PerlinWander>();
        if (wander != null)
        {
            wander.StartWander();
        }

        // Outline �ڽ� ������Ʈ Ž��
        var outline = transform.Find("Outline");
        if (outline != null)
            outlineRenderer = outline.GetComponent<SpriteRenderer>();

        // �ʱ⿡�� �ƿ����� ����
        if (outlineRenderer != null)
            outlineRenderer.enabled = false;
    }

    protected override void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(player.position, transform.position);

        // �÷��̾ ������ ���� ���
        if (distanceToPlayer <= fleeRange)
        {
            // ��ȸ ����
            if (wander != null) wander.StopWander();

            // �÷��̾� �ݴ� �������� �̵�
            Vector2 dir = (transform.position - player.position).normalized;
            rb.MovePosition(rb.position + dir * fleeSpeed * Time.deltaTime);

            // ���� ��ȯ
            spriteRenderer.flipX = dir.x < 0f;
        }
        else
        {
            // �÷��̾ �ָ� ��ȸ �簳
            if (wander != null) wander.StartWander();
        }

        // ���� �ֱ⸶�� �� �ߵ�
        healTimer -= Time.deltaTime;
        if (healTimer <= 0)
        {
            HealAllMonsters();
            healTimer = healInterval;
            StartCoroutine(OutlineEffectSimple());
        }
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

        Debug.Log($"{gameObject.name}�� �ֺ� ���͸� ȸ�����׽��ϴ� (+{healAmount})");
    }

    private IEnumerator OutlineEffectSimple()
    {
        if (outlineRenderer == null) yield break;

        outlineRenderer.enabled = true;
        yield return new WaitForSeconds(outlineShowTime);
        outlineRenderer.enabled = false;
    }
}
