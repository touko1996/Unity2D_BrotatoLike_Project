using UnityEngine;

// ���� ���� ����ü (�÷��̾�Ը� ������)
public class CowardBullet: MonoBehaviour
{
    [Header("Bullet Settings")]
    public float lifetime = 2f;       // �Ѿ� ���ӽð�
    public float damage = 10f;        // �Ѿ� ������

    private void Start()
    {
        // ������ �ð� �� �ڵ� �ı�
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // �÷��̾�Ը� ����
        if (other.CompareTag("Player"))
        {
            PlayerStats playerStats = other.GetComponent<PlayerStats>();
            if (playerStats != null)
                playerStats.TakeDamage(damage);

            Destroy(gameObject);
        }
    }
}
