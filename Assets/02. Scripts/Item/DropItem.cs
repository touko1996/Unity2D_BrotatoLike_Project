using UnityEngine;

public class DropItem : MonoBehaviour
{
    [Header("Reward Values")]
    public int coinValue = 1;
    public float expValue = 1f;

    [Header("Magnet Settings")]
    public float attractRange = 4f;      // 플레이어를 끌어당기기 시작하는 거리
    public float moveSpeed = 10f;        // 플레이어를 향해 이동하는 속도
    public float pickupDistance = 0.5f;  // 자동으로 먹히는 거리

    private bool isCollected = false;
    private Transform player;

    private void Start()
    {
        player = GameObject.FindWithTag("Player")?.transform;
    }

    private void Update()
    {
        if (isCollected || player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        // 플레이어가 범위 안으로 들어오면 따라가기 시작
        if (distance <= attractRange)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            transform.position += (Vector3)(direction * moveSpeed * Time.deltaTime);

            // 플레이어 근처로 도착하면 자동 획득 처리
            if (distance <= pickupDistance)
            {
                Collect();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isCollected) return;

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

        // 코인 획득 사운드 재생
        AudioManager.Instance.PlaySFX(AudioManager.Instance.sfxCoin, 0.9f);

        Debug.Log("아이템 획득! 경험치 +" + expValue + ", 재화 +" + coinValue);
        Destroy(gameObject);
    }
}
