using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "Player Stats")]
public class PlayerStats : ScriptableObject
{
    [Header("Config")]
    public int Level;

    [Header("Health")]
    public float Health;
    public float MaxHealth;

    //[Header("Mana")]
    //public float Mana;
    //public float MaxMana;

    //[Header("Attack")]
    //public float BaseDamage;
    //public float CriticalChance;
    //public float CriticalDamage;

    //[Header("Armor")]
    //public float Armor;
    //public float MaxArmor;
}
