using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum StatType
{
    MaxHealth,       // Tăng máu tối đa
    Health,         // Tăng máu
    Speed,        // Tăng tốc độ di chuyển
    Armor,        // Tăng giáp
    Shield,       // Tăng khiên
    Dodge         // Tăng né tránh
}

[System.Serializable]
public struct UpgradeOption
{
    public StatType statType; // Loại stat được nâng cấp
    public int amount; // Giá trị tăng thêm

    public UpgradeOption(StatType type, int value)
    {
        statType = type;
        amount = value;
    }
}

public class UpgradeManager : MonoBehaviour
{
    public PlayerStats playerStats;
    public GameObject upgradeUI; // UI để hiển thị bảng nâng cấp
    public TextMeshProUGUI[] upgradeButtons; // Text hiển thị trên các nút nâng cấp

    private List<UpgradeOption> upgradeOptions = new List<UpgradeOption>();

    void Start()
    {
        upgradeUI.SetActive(false); // Ẩn UI khi game bắt đầu
    }

    public void ShowUpgradeOptions()
    {
        Time.timeScale = 0; // Dừng game
        upgradeUI.SetActive(true);
        GenerateRandomUpgrades();
        DisplayUpgrades();
    }

    void GenerateRandomUpgrades()
    {
        upgradeOptions.Clear();
        for (int i = 0; i < 3; i++) // 3 lựa chọn ngẫu nhiên
        {
            StatType randomStat = (StatType)Random.Range(0, System.Enum.GetValues(typeof(StatType)).Length);
            int randomValue = Random.Range(1, 4); // Giá trị ngẫu nhiên

            upgradeOptions.Add(new UpgradeOption(randomStat, randomValue));
        }
    }

    void DisplayUpgrades()
    {
        for (int i = 0; i < upgradeButtons.Length; i++)
        {
            upgradeButtons[i].text = $"{upgradeOptions[i].statType} +{upgradeOptions[i].amount}";
        }
    }

    public void ApplyUpgrade(int index)
    {
        UpgradeOption chosenUpgrade = upgradeOptions[index];

        // Áp dụng nâng cấp vào PlayerStats
        switch (chosenUpgrade.statType)
        {
            case StatType.MaxHealth:
                playerStats.MaxHealth += chosenUpgrade.amount;
                break;
            case StatType.Health:
                playerStats.Health += chosenUpgrade.amount;
                break;
            case StatType.Speed:
                playerStats.Speed += chosenUpgrade.amount;
                break;
            case StatType.Armor:
                playerStats.Armor += chosenUpgrade.amount;
                break;
            case StatType.Shield:
                playerStats.Shield += chosenUpgrade.amount;
                break;
            case StatType.Dodge:
                playerStats.Dodge += chosenUpgrade.amount;
                break;
        }

        CloseUpgradeUI();
    }

    void CloseUpgradeUI()
    {
        upgradeUI.SetActive(false);
        Time.timeScale = 1; // Tiếp tục game
    }
}
