using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class ShopScrollSnap : MonoBehaviour
{
    public ScrollRect scrollRect; // Scroll Rect của cửa hàng
    public RectTransform content; // Content chứa các vật phẩm
    public float snapSpeed = 10f; // Tốc độ dừng
    public float threshold = 0.1f; // Ngưỡng để dừng lại
    public Button buyButton; // Nút mua vũ khí
    public WeaponManager weaponManager; // Hệ thống vũ khí của player

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
    }

    void Update()
    {
        //if (!isDragging)
        //{
        //    // Scroll đến vị trí gần nhất
        //    content.anchoredPosition = Vector2.Lerp(
        //        content.anchoredPosition,
        //        new Vector2(-itemTransforms[selectedItemIndex].anchoredPosition.x, content.anchoredPosition.y),
        //        Time.deltaTime * snapSpeed
        //    );
        //}
    }

    public void OnDragStart()
    {
        isDragging = true;
    }

    //public void OnDragEnd()
    //{
    //    isDragging = false;
    //    float closestDistance = float.MaxValue;

    //    for (int i = 0; i < itemTransforms.Count; i++)
    //    {
    //        float distance = Mathf.Abs(content.anchoredPosition.x + itemTransforms[i].anchoredPosition.x);
    //        if (distance < closestDistance)
    //        {
    //            closestDistance = distance;
    //            selectedItemIndex = i;
    //        }
    //    }
    //}

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
        if (weaponManager != null && selectedWeaponItem != null)
        {
            weaponManager.AddWeapon(selectedWeaponItem.weaponStats);
            Debug.Log($"Mua vũ khí: {selectedWeaponItem.weaponStats.weaponName}");
        }
    }
}
