using UnityEngine;

// 투사체 (총알) 동작 처리
public class Bullet : MonoBehaviour
{
    public float lifetime = 2f;
    [HideInInspector] public float damage;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Monster"))
        {
            Monster monster = other.GetComponent<Monster>();
            if (monster != null)
                monster.ReceiveDamage(damage);

            Destroy(gameObject);
        }
    }
}
