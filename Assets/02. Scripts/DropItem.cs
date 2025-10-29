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
                    inventory.AddReward(1, 1); // ����ġ +1, ��ȭ +1
            }

            Debug.Log("������ ȹ��! ����ġ +1, ��ȭ +1");
            Destroy(gameObject); // �÷��̾ �ֿ��� ���� �����
        }
    }
}
