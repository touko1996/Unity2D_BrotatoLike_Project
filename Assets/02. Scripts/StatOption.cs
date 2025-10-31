using UnityEngine;

[System.Serializable]
public class StatOption
{
    public string statName;        // 스탯 이름 (예: 공격력 증가)
    public string description;     // 스탯 설명
    public System.Action<PlayerStats> applyEffect; // 효과 함수
}
