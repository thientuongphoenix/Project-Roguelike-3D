using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public GameObject weaponPrefab; // Prefab vũ khí
    private List<Transform> _weaponSpawnPoints; // Danh sách điểm spawn
    private List<GameObject> _activeWeapons = new List<GameObject>(); // Danh sách vũ khí đang có

    private CreateWeaponAttachmentPoint spawnPointManager;
    public float spawnOffset = 0.5f; // Khoảng cách giữa nhân vật và vũ khí

    void Start()
    {
        // Lấy reference tới script tạo điểm gắn vũ khí
        spawnPointManager = GetComponent<CreateWeaponAttachmentPoint>();

        if (spawnPointManager == null)
        {
            Debug.LogError("Không tìm thấy CreateWeaponAttachmentPoint trên Object!");
            return;
        }

        // Tạo điểm spawn và lấy danh sách vị trí
        _weaponSpawnPoints = spawnPointManager.GenerateSpawnPoints();

        // Bắt đầu game có 1 vũ khí
        AddWeapon();
    }

    // Thêm vũ khí vào vị trí trống tiếp theo
    public void AddWeapon()
    {
        if (_activeWeapons.Count >= _weaponSpawnPoints.Count)
        {
            Debug.Log("Không thể thêm vũ khí, đã đạt giới hạn tối đa!");
            return;
        }

        int nextIndex = _activeWeapons.Count;
        Transform spawnTransform = _weaponSpawnPoints[nextIndex]; // Fix lỗi: Khai báo spawnTransform

        // Tính vector hướng từ nhân vật đến điểm spawn
        Vector3 direction = (spawnTransform.position - transform.position).normalized;

        // Dịch ra xa thêm một khoảng spawnOffset
        Vector3 finalSpawnPosition = spawnTransform.position + direction * spawnOffset; // Fix lỗi: đổi tên biến để tránh trùng

        GameObject newWeapon = Instantiate(weaponPrefab, finalSpawnPosition, weaponPrefab.transform.rotation, transform);
        _activeWeapons.Add(newWeapon);
    }

    // Xóa vũ khí khỏi danh sách
    public void RemoveWeapon(int index)
    {
        if (index < 0 || index >= _activeWeapons.Count) return;

        Destroy(_activeWeapons[index]);
        _activeWeapons.RemoveAt(index);
    }
}
