using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public int experience = 0;
    public int gold = 0;

    public void AddReward(int exp, int money)
    {
        experience += exp;
        gold += money;
        Debug.Log($"[ȹ��] ����ġ +{exp}, ��� +{money} / ����: EXP {experience}, GOLD {gold}");
    }
}
