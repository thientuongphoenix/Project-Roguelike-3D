using TMPro;
using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    public TextMeshProUGUI blockText; // Hiển thị số lượng Block trên UI
    private int blockCount = 0; // Số lượng Block đã nhặt

    private void Start()
    {
        UpdateUI();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Block")) // Kiểm tra nếu va chạm với Block
        {
            blockCount++;
            UpdateUI();
            Destroy(other.gameObject); // Hủy Block khỏi game
        }
    }

    private void UpdateUI()
    {
        blockText.text = "" + blockCount;
    }
}
