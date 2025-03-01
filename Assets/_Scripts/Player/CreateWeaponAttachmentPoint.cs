using System.Collections.Generic;
using UnityEngine;

public class CreateWeaponAttachmentPoint : MonoBehaviour
{
    [SerializeField] private float _maxWeapons = 4; // Số lượng vũ khí tối đa gắn được
    private List<Transform> _weaponSpawnPoints = new List<Transform>(); // Danh sách các điểm spawn

    private void Start()
    {
        GenerateSpawnPoints();
    }

    public List<Transform> GenerateSpawnPoints()
    {
        _weaponSpawnPoints.Clear(); // Xóa các điểm cũ nếu có

        SphereCollider sphereCollider = GetComponent<SphereCollider>();
        if (sphereCollider == null)
        {
            Debug.LogError("Không tìm thấy SphereCollider!");
            return null;
        }

        float colliderRadius = sphereCollider.radius * sphereCollider.transform.lossyScale.x;
        Vector3 colliderCenter = sphereCollider.transform.TransformPoint(sphereCollider.center);

        for (int i = 0; i < _maxWeapons; i++)
        {
            float angle = i * Mathf.PI * 2f / _maxWeapons;
            Vector3 spawnPos = colliderCenter + new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * colliderRadius;

            GameObject spawnPoint = new GameObject($"SpawnPoint_{i}");
            spawnPoint.transform.position = spawnPos;
            spawnPoint.transform.parent = transform; // Gán làm con của nhân vật

            _weaponSpawnPoints.Add(spawnPoint.transform); // Lưu vào danh sách
        }

        return _weaponSpawnPoints;
    }

    // Vẽ Gizmos để dễ quan sát trong Editor
    private void OnDrawGizmos()
    {
        if (_weaponSpawnPoints == null || _weaponSpawnPoints.Count == 0) return;

        Gizmos.color = Color.red;
        foreach (var spawnPoint in _weaponSpawnPoints)
        {
            if (spawnPoint != null)
            {
                Gizmos.DrawSphere(spawnPoint.position, 0.05f); // Vẽ hình cầu nhỏ
            }
        }
    }
}
