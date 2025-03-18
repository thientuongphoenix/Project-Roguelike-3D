using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class ShopScrollSnap : MonoBehaviour
{
    public ScrollRect scrollRect; // Scroll Rect của cửa hàng
    public RectTransform content; // Content chứa các vật phẩm
    //public float snapSpeed = 10f; // Tốc độ dừng
    //public float threshold = 0.1f; // Ngưỡng để dừng lại
    public Button buyButton; // Nút mua vũ khí
    public WeaponManager weaponManager; // Hệ thống vũ khí của player

    public PlayerPickup playerPickup; // Tham chiếu đến hệ thống Block của player
    public TextMeshProUGUI currentBlockText;// Hiển thị số block hiện có
    public TextMeshProUGUI statusText; // Text hiển thị thông báo

    //private List<RectTransform> itemTransforms = new List<RectTransform>();
    //private int selectedItemIndex = 0;
    private List<WeaponItem> weaponItems = new List<WeaponItem>(); // Danh sách item vũ khí
    private WeaponItem selectedWeaponItem = null; // Item đang được chọn
    private bool isDragging;

    void Start()
    {
        // Lấy danh sách vị trí các vật phẩm trong Content
        foreach (Transform item in content)
        {
            //itemTransforms.Add(item.GetComponent<RectTransform>());
            WeaponItem weaponItem = item.GetComponent<WeaponItem>();
            if (weaponItem != null)
            {
                weaponItems.Add(weaponItem);
                weaponItem.SetShopManager(this); // Gán reference đến ShopScrollSnap
            }
        }

        // Gán sự kiện cho nút mua
        buyButton.onClick.AddListener(BuyWeapon);

        statusText.gameObject.SetActive(false); // Ẩn thông báo ban đầu
    }

    void Update()
    {
        UpdateCurrentBlockText();
    }

    public void OnDragStart()
    {
        isDragging = true;
    }

    /// <summary>
    /// Được gọi khi người chơi bấm vào một item.
    /// </summary>
    public void SelectWeapon(WeaponItem clickedItem)
    {
        // Bỏ chọn tất cả item trước đó
        foreach (var item in weaponItems)
        {
            item.SetSelected(false);
        }

        // Chọn item mới
        selectedWeaponItem = clickedItem;
        selectedWeaponItem.SetSelected(true);
    }

    /// <summary>
    /// Mua vũ khí đã chọn và trang bị cho player.
    /// </summary>
    public void BuyWeapon()
    {
        if (selectedWeaponItem == null || weaponManager == null || playerPickup == null)
            return;

        int weaponPrice = selectedWeaponItem.weaponStats.weaponPrice;

        // Kiểm tra số lượng block trước khi mua
        if (playerPickup.GetBlockCount() >= weaponPrice)
        {
            // Trừ block và thêm vũ khí
            playerPickup.SpendBlock(weaponPrice);
            weaponManager.AddWeapon(selectedWeaponItem.weaponStats);
            Debug.Log($"Mua vũ khí: {selectedWeaponItem.weaponStats.weaponName}");
        }
        else
        {
            // Hiển thị cảnh báo nếu không đủ block
            StartCoroutine(ShowStatusMessage("You don't have enough blocks!", 1f));
        }

        AudioManager.Instance.PlaySFX(SoundType.ButtonClick);
    }

    /// <summary>
    /// Hiển thị thông báo trong một khoảng thời gian.
    /// </summary>
    private IEnumerator ShowStatusMessage(string message, float duration)
    {
        statusText.text = message;
        statusText.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(duration); // Sử dụng thời gian thực
        statusText.gameObject.SetActive(false);
    }

    /// <summary>
    /// Cập nhật số lượng block hiện có trong shop.
    /// </summary>
    public void UpdateCurrentBlockText()
    {
        if (playerPickup != null && currentBlockText != null)
        {
            currentBlockText.text = $"Block: {playerPickup.GetBlockCount()}";
        }
    }
}
