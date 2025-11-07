using System;
using UnityEngine;

/// <summary>
/// [StatOption]
/// ------------------------------------------------------------
/// 스탯 선택 UI에서 사용되는 개별 옵션 데이터 구조체
/// - 이름, 설명, 효과(Action)으로 구성
/// - Action<PlayerStats>를 이용해 선택 시 실행할 효과를 직접 정의 가능
/// ------------------------------------------------------------
/// </summary>
[System.Serializable]
public class StatOption
{
    [Header("기본 정보")]
    public string statName;        
    public string description;     

    [Header("효과 처리 함수")]
    public Action<PlayerStats> applyEffect;  // 선택 시 실행될 함수

    // 기본 생성자 (직렬화용)
    public StatOption() { }

    // 전체 초기화 생성자
    public StatOption(string name, string desc, Action<PlayerStats> effect)
    {
        statName = name;
        description = desc;
        applyEffect = effect;
    }
}
