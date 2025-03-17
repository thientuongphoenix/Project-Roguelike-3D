using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public WeaponStats weaponStats;
    //public GameObject weaponPrefab; // Prefab vũ khí
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
        Transform spawnTransform = _weaponSpawnPoints[nextIndex]; // Fix lỗi: Khai báo spawnTransform, dòng này dùng để biết vị trí tiếp theo gắn vũ khí

        // Tính vector hướng từ nhân vật đến điểm spawn
        Vector3 direction = (spawnTransform.position - transform.position).normalized;

        // Dịch ra xa thêm một khoảng spawnOffset
        Vector3 finalSpawnPosition = spawnTransform.position + direction * spawnOffset; // Fix lỗi: đổi tên biến để tránh trùng

        GameObject newWeapon = Instantiate(weaponStats.weaponPrefab, finalSpawnPosition, weaponStats.weaponPrefab.transform.rotation, transform);
        _activeWeapons.Add(newWeapon);
    }

    /// <summary>
    /// Thêm vũ khí vào vị trí trống tiếp theo
    /// </summary>
    /// <param name="weaponStats">WeaponStats của vũ khí cần thêm</param>
    public void AddWeapon(WeaponStats weaponStats)
    {
        if (_activeWeapons.Count >= _weaponSpawnPoints.Count)
        {
            Debug.Log("Không thể thêm vũ khí, đã đạt giới hạn tối đa!");
            return;
        }

        int nextIndex = _activeWeapons.Count;
        Transform spawnTransform = _weaponSpawnPoints[nextIndex]; // Vị trí spawn kế tiếp

        // Dịch ra xa thêm một khoảng spawnOffset
        Vector3 direction = (spawnTransform.position - transform.position).normalized;
        Vector3 finalSpawnPosition = spawnTransform.position + direction * spawnOffset;

        GameObject newWeapon = Instantiate(weaponStats.weaponPrefab, finalSpawnPosition, Quaternion.identity, transform);
        _activeWeapons.Add(newWeapon);
    }

    /// <summary>
    /// Hàm để gọi từ Button UI (nhận WeaponStats)
    /// </summary>
    /// <param name="weaponStats">Vũ khí cần thêm</param>
    public void OnAddWeaponButtonClick(WeaponStats weaponStats)
    {
        AddWeapon(weaponStats);
    }

    // Xóa vũ khí khỏi danh sách
    public void RemoveWeapon(int index)
    {
        if (index < 0 || index >= _activeWeapons.Count) return;

        Destroy(_activeWeapons[index]);
        _activeWeapons.RemoveAt(index);
    }
}
