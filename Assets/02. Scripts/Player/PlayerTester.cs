using UnityEngine;

// �׽�Ʈ�� ��ũ��Ʈ (���� & �нú� �ڵ� ����)
public class PlayerTester : MonoBehaviour
{
    [Header("�׽�Ʈ ���� ������")]
    [SerializeField] private WeaponData[] testWeapons;

    [Header("�׽�Ʈ �нú� ������")]
    [SerializeField] private PassiveItem[] testPassives;

    [Header("���� ��Ʈ ������Ʈ")]
    [SerializeField] private GameObject weaponRoot;

    private PlayerStats _playerStats;

    private void Start()
    {
        _playerStats = GetComponent<PlayerStats>();
        if (_playerStats == null)
        {
            Debug.LogError("PlayerStats ������Ʈ�� �����ϴ�.");
            return;
        }

        if (weaponRoot == null)
        {
            Debug.LogError("WeaponRoot�� �������� �ʾҽ��ϴ�.");
            return;
        }

        // �׽�Ʈ�� ���� ����
        foreach (WeaponData weaponData in testWeapons)
        {
            if (weaponData != null)
                weaponData.ApplyEffect(weaponRoot);
        }

        // �׽�Ʈ�� �нú� ����
        foreach (PassiveItem passive in testPassives)
        {
            if (passive != null)
                passive.ApplyEffect(gameObject);
        }

        // ���� ���� �α� ���
        Debug.Log($"���ݷ�: {_playerStats.currentDamage}");
        Debug.Log($"�����Ÿ�: {_playerStats.currentRange}");
        Debug.Log($"���ݼӵ�: {_playerStats.currentAttackSpeed}");
        Debug.Log($"�̵��ӵ�: {_playerStats.currentMoveSpeed}");
    }
}
