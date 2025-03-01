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
    public float Health;
    public float MaxHealth;

    [Header("Armor")]
    public float Armor;

    [Header("Shield")]
    public float Shield;
    public float MaxShield;

    [Header("Dodge")]
    public float Dodge;
}
