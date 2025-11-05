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
    private PlayerInventory _playerInventory;

    private void Start()
    {
        _playerStats = GetComponent<PlayerStats>();
        _playerInventory = GetComponent<PlayerInventory>();

        if (_playerStats == null || _playerInventory == null)
        {
            Debug.LogError("PlayerStats 또는 PlayerInventory 컴포넌트를 찾을 수 없습니다.");
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
            {
                WeaponData clone = ScriptableObject.Instantiate(weaponData);
                clone.ApplyEffect(weaponRoot);

                // 인벤토리에도 추가 (UI 반영용)
                _playerInventory.ownedItems.Add(clone);
            }
        }

        // 테스트용 패시브 적용
        foreach (PassiveItem passive in testPassives)
        {
            if (passive != null)
            {
                PassiveItem clone = ScriptableObject.Instantiate(passive);
                clone.ApplyEffect(gameObject);
                _playerInventory.ownedItems.Add(clone);
            }
        }

        // UI 갱신 호출
        _playerInventory.OnInventoryChanged?.Invoke();

        Debug.Log("테스트 아이템 적용 완료");
    }
}
