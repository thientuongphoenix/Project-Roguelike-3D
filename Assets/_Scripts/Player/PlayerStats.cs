using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "Player Stats")]
public class PlayerStats : ScriptableObject
{
    [Header("Config")]
    public float Level;
    public float TotalExp;

    [Header("MoveSpeed")]
    public float Speed;

    [Header("Health")]
    public float Health;
    public float MaxHealth;

    [Header("Armor")]
    public float Armor;

    [Header("Shield")]
    public float Shield;
    public float MaxShield;
    public float ShieldRegen;

    [Header("Dodge")]
    public float Dodge;

    [Header("Luck")]
    public float Luck;

    [Header("Pickup Range")]
    public float PickupRange;

    [Header("%XP Gain")]
    public float PercentXPGain;

    [Header("%Damage")]
    public float PercentDamage;

    [Header("AttackSpeed")]
    public float AttackSpeed;

    [Header("Critical Chance")]
    public float CritChance;

    [Header("Critical Damage")]
    public float CritDamage;

    [Header("Life Steal")]
    public float LifeSteal;

    [Header("Range")]
    public float Range;

    [Header("DetectionRange")]
    public float DetectionRange;
}
