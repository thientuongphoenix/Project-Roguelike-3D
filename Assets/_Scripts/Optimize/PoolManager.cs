using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Lớp quản lý tất cả các Object Pool trong game
/// Sử dụng Singleton pattern để dễ dàng truy cập từ mọi nơi
/// </summary>
public class PoolManager : MonoBehaviour
{
    /// <summary>
    /// Instance duy nhất của PoolManager
    /// </summary>
    public static PoolManager Instance;

    /// <summary>
    /// Dictionary lưu trữ tất cả các pool theo key
    /// </summary>
    private Dictionary<string, object> pool = new Dictionary<string, object>();

    /// <summary>
    /// Khởi tạo Instance khi game bắt đầu
    /// </summary>
    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// Tạo một pool mới cho một loại prefab
    /// </summary>
    /// <typeparam name="T">Kiểu MonoBehaviour của prefab</typeparam>
    /// <param name="key">Key để định danh pool</param>
    /// <param name="prefab">Prefab cần pool</param>
    /// <param name="initialSize">Số lượng object ban đầu</param>
    public void CreatePool<T>(string key, T prefab, int initialSize)
        where T : MonoBehaviour
    {
        if (!pool.ContainsKey(key))
        {
            var objectPool = new ObjectPoolGeneric<T>(prefab, initialSize, transform);
            pool.Add(key, objectPool);
        }
    }

    /// <summary>
    /// Lấy một object từ pool theo key
    /// </summary>
    /// <typeparam name="T">Kiểu MonoBehaviour của object cần lấy</typeparam>
    /// <param name="key">Key của pool</param>
    /// <param name="position">Vị trí cần đặt object</param>
    /// <param name="rotation">Góc xoay của object</param>
    /// <returns>Object được lấy từ pool hoặc null nếu không tìm thấy pool</returns>
    public T GetObject<T>(string key, Vector3 position, Quaternion rotation)
        where T : MonoBehaviour
    {
        if (pool.ContainsKey(key))
        {
            var objectPool = (ObjectPoolGeneric<T>)pool[key];
            return objectPool.GetObject(position, rotation);
        }
        return null;
    }

    /// <summary>
    /// Trả một object về pool
    /// </summary>
    /// <typeparam name="T">Kiểu MonoBehaviour của object cần trả</typeparam>
    /// <param name="key">Key của pool</param>
    /// <param name="obj">Object cần trả về pool</param>
    public void ReturnObject<T>(string key, T obj)
        where T : MonoBehaviour
    {
        if (pool.ContainsKey(key))
        {
            var objectPool = (ObjectPoolGeneric<T>)pool[key];
            objectPool.ReturnObject(obj);
        }
    }
}
