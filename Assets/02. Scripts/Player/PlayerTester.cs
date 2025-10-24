using UnityEngine;

// 테스트용 스크립트 (무기 & 패시브 자동 적용)
public class PlayerTester : MonoBehaviour
{
    [Header("테스트 무기 데이터")]
    [SerializeField] private WeaponData[] testWeapons;

    [Header("테스트 패시브 아이템")]
    [SerializeField] private PassiveItem[] testPassives;

    [Header("무기 루트 오브젝트")]
    [SerializeField] private GameObject weaponRoot;

    private PlayerStats _playerStats;

    private void Start()
    {
        _playerStats = GetComponent<PlayerStats>();
        if (_playerStats == null)
        {
            Debug.LogError("PlayerStats 컴포넌트가 없습니다.");
            return;
        }

        if (weaponRoot == null)
        {
            Debug.LogError("WeaponRoot가 지정되지 않았습니다.");
            return;
        }

        // 테스트용 무기 장착
        foreach (WeaponData weaponData in testWeapons)
        {
            if (weaponData != null)
                weaponData.ApplyEffect(weaponRoot);
        }

        // 테스트용 패시브 적용
        foreach (PassiveItem passive in testPassives)
        {
            if (passive != null)
                passive.ApplyEffect(gameObject);
        }

        // 현재 스탯 로그 출력
        Debug.Log($"공격력: {_playerStats.currentDamage}");
        Debug.Log($"사정거리: {_playerStats.currentRange}");
        Debug.Log($"공격속도: {_playerStats.currentAttackSpeed}");
        Debug.Log($"이동속도: {_playerStats.currentMoveSpeed}");
    }
}
