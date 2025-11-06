using System.Collections.Generic;

/// <summary>
/// [PlayerPrefsData]
/// ------------------------------------------------------------
/// 게임 종료 후, 플레이어의 마지막 기록(웨이브, 무기, 패시브)을
/// 임시로 저장하는 정적 데이터 클래스.
/// 
/// 실제 PlayerPrefs를 사용하는 건 아니지만,
/// 다음 씬(예: GameOverScene)에서 참조할 수 있도록
/// 런타임 동안 데이터를 유지한다.
/// ------------------------------------------------------------
/// </summary>
public static class PlayerPrefsData
{
    // 마지막으로 생존한 웨이브 번호
    public static int lastWave = 0;

    // 보유 무기 이름 목록
    public static List<string> ownedWeaponNames = new List<string>();

    // 보유 패시브 아이템 이름 목록
    public static List<string> ownedPassiveNames = new List<string>();

    /// <summary>
    /// 플레이어 인벤토리 정보를 불러와
    /// 현재 보유한 무기 / 패시브 목록을 저장한다.
    /// </summary>
    public static void SaveFromInventory(PlayerInventory playerInventory)
    {
        // 이전 데이터 초기화
        ownedWeaponNames.Clear();
        ownedPassiveNames.Clear();

        if (playerInventory == null)
            return;

        // 무기 리스트 저장
        foreach (var weapon in playerInventory.GetOwnedWeapons())
        {
            if (weapon != null)
                ownedWeaponNames.Add(weapon.itemName);
        }

        // 패시브 리스트 저장
        foreach (var passive in playerInventory.GetOwnedPassives())
        {
            if (passive != null)
                ownedPassiveNames.Add(passive.itemName);
        }
    }
}
