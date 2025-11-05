using System;
using UnityEngine;

[System.Serializable]
public class StatOption
{
    public string statName;
    public string description;
    public Action<PlayerStats> applyEffect;

    // empty constructor for object initializer
    public StatOption()
    {
    }

    // optional full constructor
    public StatOption(string name, string desc, Action<PlayerStats> effect)
    {
        statName = name;
        description = desc;
        applyEffect = effect;
    }
}
