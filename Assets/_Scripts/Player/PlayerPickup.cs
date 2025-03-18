using TMPro;
using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    public PlayerStats playerStats;
    public TextMeshProUGUI blockText; // Hiển thị số lượng Block trên UI
    private int blockCount = 0; // Số lượng Block đã nhặt

    private PlayerHealth playerHealth; // Tham chiếu đến PlayerHealth

    private void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();
        UpdateUI();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Block")) // Kiểm tra nếu va chạm với Block
        {
            blockCount++;
            UpdateUI();
            if (playerHealth != null)
            {
                playerHealth.Heal(10); // Hồi 10 máu
            }
            Destroy(other.gameObject); // Hủy Block khỏi game
        }
    }

    private void UpdateUI()
    {
        blockText.text = "" + blockCount;
    }

    /// <summary>
    /// Lấy số lượng block hiện có.
    /// </summary>
    public int GetBlockCount()
    {
        return blockCount;
    }

    /// <summary>
    /// Trừ block khi mua vật phẩm và cập nhật UI.
    /// </summary>
    public void SpendBlock(int amount)
    {
        blockCount -= amount;
        UpdateUI();
    }
}
