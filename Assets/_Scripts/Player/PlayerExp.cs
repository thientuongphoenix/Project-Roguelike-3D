using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerExp : MonoBehaviour
{
    public PlayerStats playerStats; // ScriptableObject chứa thông tin nhân vật
    public TextMeshProUGUI levelText; // Hiển thị cấp độ
    public Slider expSlider; // Thanh tiến trình EXP

    private int expToNextLevel; // EXP cần để lên cấp tiếp theo

    void Start()
    {
        playerStats.Level = 1;
        playerStats.TotalExp = 0;
        expToNextLevel = 100; // EXP cần để lên cấp đầu tiên

        UpdateUI();

        // Test: Tự động cộng kinh nghiệm mỗi giây
        InvokeRepeating("AddExp", 1f, 1f);
    }

    void AddExp()
    {
        int expGain = Random.Range(5, 15); // Thêm EXP ngẫu nhiên để test
        playerStats.TotalExp += expGain;
        CheckLevelUp();
        UpdateUI();
    }

    void CheckLevelUp()
    {
        while (playerStats.TotalExp >= expToNextLevel)
        {
            playerStats.TotalExp -= expToNextLevel;
            playerStats.Level++;
            expToNextLevel = Mathf.CeilToInt(expToNextLevel * 1.1f); // Cộng 10%
        }
    }

    void UpdateUI()
    {
        levelText.text = "LVL " + playerStats.Level;
        expSlider.maxValue = expToNextLevel;
        expSlider.value = playerStats.TotalExp;
    }
}
