using TMPro;
using UnityEngine;

public class WaveSystem : MonoBehaviour
{
    public TextMeshProUGUI waveText; // Hiển thị số Wave
    public TextMeshProUGUI timerText; // Hiển thị đồng hồ đếm ngược

    private int waveNumber = 1; // Wave bắt đầu từ 1
    private float timeRemaining = 60f; // Bắt đầu đếm từ 60 giây
    private bool isCounting = true; // Kiểm tra có đang đếm không

    void Start()
    {
        UpdateUI();
        StartCoroutine(CountdownTimer());
    }

    private System.Collections.IEnumerator CountdownTimer()
    {
        while (true)
        {
            if (isCounting)
            {
                if (timeRemaining > 0)
                {
                    timeRemaining -= 1f;
                }
                else
                {
                    IncreaseWave();
                }

                UpdateUI();
            }
            yield return new WaitForSeconds(1f);
        }
    }

    void IncreaseWave()
    {
        waveNumber++;
        timeRemaining = 60f; // Reset thời gian về 60 giây

        FindFirstObjectByType<UpgradeManager>().ShowUpgradeOptions(); // Gọi hệ thống nâng cấp
    }

    void UpdateUI()
    {
        waveText.text = "" + waveNumber;
        timerText.text = Mathf.CeilToInt(timeRemaining) + "S";
    }
}
