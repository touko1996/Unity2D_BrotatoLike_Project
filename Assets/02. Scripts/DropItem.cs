using UnityEngine;

public class DropItem : MonoBehaviour
{
    private bool isCollected = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isCollected) return;

        if (other.CompareTag("Player"))
        {
            isCollected = true;
            PlayerStats playerStats = other.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                PlayerInventory inventory = other.GetComponent<PlayerInventory>();
                if (inventory != null)
                    inventory.AddReward(1, 1); // 경험치 +1, 재화 +1
            }

            Debug.Log("아이템 획득! 경험치 +1, 재화 +1");
            Destroy(gameObject); // 플레이어가 주웠을 때만 사라짐
        }
    }
}
