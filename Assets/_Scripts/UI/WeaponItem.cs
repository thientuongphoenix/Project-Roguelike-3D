using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponItem : MonoBehaviour
{
    public WeaponStats weaponStats; // Dữ liệu vũ khí
    private ShopScrollSnap shopManager; // Tham chiếu đến hệ thống shop
    public Image icon; // Ảnh hiển thị trong shop
    private Image itemBackground; // Dùng để đổi màu khi chọn
    public TextMeshProUGUI priceText; // Text hiển thị giá

    void Start()
    {
        if (weaponStats != null && icon != null)
        {
            icon.sprite = weaponStats.weaponIcon;
        }

        itemBackground = GetComponent<Image>();
        GetComponent<Button>().onClick.AddListener(OnItemClick);

        if (weaponStats != null && priceText != null)
        {
            priceText.text = weaponStats.weaponPrice + " Block"; // Hiển thị giá vũ khí
        }
    }

    /// <summary>
    /// Gán reference đến ShopScrollSnap.
    /// </summary>
    public void SetShopManager(ShopScrollSnap manager)
    {
        shopManager = manager;
    }

    /// <summary>
    /// Khi bấm vào item, thông báo cho ShopScrollSnap.
    /// </summary>
    private void OnItemClick()
    {
        if (shopManager != null)
        {
            shopManager.SelectWeapon(this);
        }
    }

    /// <summary>
    /// Đổi màu item khi được chọn hoặc bỏ chọn.
    /// </summary>
    public void SetSelected(bool isSelected)
    {
        if (itemBackground != null)
        {
            itemBackground.color = isSelected ? Color.yellow : Color.white; // Đổi màu khi được chọn
        }
    }
}
