using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "Player Stats")]
public class PlayerStats : ScriptableObject
{
    [Header("Config")]
    public int Level;
    public int TotalExp;

    [Header("MoveSpeed")]
    public int Speed;

    [Header("Health")]
    public int Health;
    public int MaxHealth;

    [Header("Armor")]
    public int Armor;

    [Header("Shield")]
    public int Shield;
    public int MaxShield;

    [Header("Dodge")]
    public int Dodge;
}
