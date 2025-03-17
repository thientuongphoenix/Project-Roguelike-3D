using UnityEngine;
using UnityEngine.UI;

public class WeaponItem : MonoBehaviour
{
    public WeaponStats weaponStats; // Dữ liệu vũ khí
    public Image icon; // Ảnh hiển thị trong shop

    void Start()
    {
        if (weaponStats != null && icon != null)
        {
            icon.sprite = weaponStats.weaponIcon;
        }
    }
}
