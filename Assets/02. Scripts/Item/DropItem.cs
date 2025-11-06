using UnityEngine;

public class DropItem : MonoBehaviour
{
    [Header("Reward Values")]
    public int coinValue = 1;
    public float expValue = 1f;

    [Header("Magnet Settings")]
    public float attractRange = 4f;
    public float moveSpeed = 10f;
    public float pickupDistance = 0.5f;

    private bool isCollected = false;
    private bool isMagnetAbsorbed = false; // 자석 흡수 전용 플래그
    private Transform player;

    private void Start()
    {
        player = GameObject.FindWithTag("Player")?.transform;
    }

    private void Update()
    {
        if (isCollected || player == null || isMagnetAbsorbed) return;
        // 자석 흡수 중이면 이 스크립트에서 따로 이동하지 않음

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= attractRange)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            transform.position += (Vector3)(direction * moveSpeed * Time.deltaTime);

            if (distance <= pickupDistance)
            {
                Collect();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isCollected || isMagnetAbsorbed) return;

        if (other.CompareTag("Player"))
        {
            Collect();
        }
    }

    private void Collect()
    {
        if (isCollected) return;
        isCollected = true;

        PlayerInventory inventory = player.GetComponent<PlayerInventory>();
        if (inventory != null)
            inventory.AddReward(coinValue, expValue);

        AudioManager.Instance.PlaySFX(AudioManager.Instance.sfxCoin, 0.9f);

        Debug.Log("아이템 획득! 경험치 +" + expValue + ", 재화 +" + coinValue);
        Destroy(gameObject);
    }

    // 자석 흡수 시작 시 외부(UI_GameWave)에서 호출
    public void SetMagnetAbsorbed()
    {
        isMagnetAbsorbed = true;
    }
}
