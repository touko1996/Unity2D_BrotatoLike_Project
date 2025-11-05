using System.Collections.Generic;

public static class PlayerPrefsData
{
    public static int lastWave = 0;
    public static List<string> weaponNames = new List<string>();
    public static List<string> passiveNames = new List<string>();

    public static void SaveFromInventory(PlayerInventory inventory)
    {
        weaponNames.Clear();
        passiveNames.Clear();

        if (inventory == null)
            return;

        foreach (var w in inventory.GetOwnedWeapons())
        {
            weaponNames.Add(w.itemName);
        }

        foreach (var p in inventory.GetOwnedPassives())
        {
            passiveNames.Add(p.itemName);
        }
    }
}
