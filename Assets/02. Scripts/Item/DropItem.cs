using UnityEngine;

public class DropItem : MonoBehaviour
{
    [Header("코인 정보")]
    public int coinValue = 1;
    public float expValue = 1f;

    [Header("자석 효과")]
    public float attractRange = 4f;
    public float moveSpeed = 10f;
    public float pickupDistance = 0.5f;

    private bool isCollected = false;
    private bool isMagnetAbsorbed = false;
    private Transform player;

    private void Start()
    {
        player = GameObject.FindWithTag("Player")?.transform;
    }

    private void Update()
    {
        if (isCollected || player == null || isMagnetAbsorbed) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= attractRange)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            transform.position += (Vector3)(direction * moveSpeed * Time.deltaTime);

            if (distance <= pickupDistance)
                Collect();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isCollected || isMagnetAbsorbed) return;

        if (other.CompareTag("Player"))
            Collect();
    }

    private void Collect()
    {
        if (isCollected) return;
        isCollected = true;

        if (player != null)
        {
            PlayerInventory inven = player.GetComponent<PlayerInventory>();
            if (inven != null)
                inven.AddReward(coinValue, expValue);
        }

        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayCoinSFX();

        Destroy(gameObject);
    }

    public void SetMagnetAbsorbed()
    {
        isMagnetAbsorbed = true;
    }
}
