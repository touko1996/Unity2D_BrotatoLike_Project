using UnityEngine;

public class DropItem : MonoBehaviour
{
    [Header("Reward Values")]
    public int coinValue = 1;
    public float expValue = 1f;

    private bool isCollected = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isCollected) return;

        if (collision.CompareTag("Player"))
        {
            isCollected = true;

            PlayerInventory inventory = collision.GetComponent<PlayerInventory>();
            if (inventory != null)
            {
                inventory.AddReward(coinValue, expValue);
            }

            Destroy(gameObject);
        }
    }
}
