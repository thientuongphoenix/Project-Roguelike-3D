using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Lớp quản lý Object Pooling cho một loại GameObject cụ thể
/// </summary>
/// <typeparam name="T">Kiểu MonoBehaviour của object cần pool</typeparam>
public class ObjectPoolGeneric<T> where T : MonoBehaviour
{
    /// <summary>
    /// Hàng đợi chứa các object đã được tạo sẵn
    /// </summary>
    private Queue<T> pool = new Queue<T>();

    /// <summary>
    /// Prefab gốc để tạo các object mới
    /// </summary>
    private T prefab;

    /// <summary>
    /// Transform cha để chứa các object được tạo
    /// </summary>
    private Transform parent;

    /// <summary>
    /// Khởi tạo Object Pool với số lượng ban đầu
    /// </summary>
    /// <param name="prefab">Prefab cần pool</param>
    /// <param name="initialSize">Số lượng object ban đầu</param>
    /// <param name="parent">Transform cha để chứa các object</param>
    public ObjectPoolGeneric(T prefab, int initialSize, Transform parent)
    {
        this.prefab = prefab;
        this.parent = parent;

        // Tạo số lượng object ban đầu
        for (int i = 0; i < initialSize; i++)
        {
            CreateNewObject();
        }
    }

    /// <summary>
    /// Tạo một object mới và thêm vào pool
    /// </summary>
    /// <returns>Object mới được tạo</returns>
    private T CreateNewObject()
    {
        T newObj = UnityEngine.Object.Instantiate(prefab, parent);
        newObj.gameObject.SetActive(false);
        pool.Enqueue(newObj);
        return newObj;
    }

    /// <summary>
    /// Lấy một object từ pool và kích hoạt nó
    /// </summary>
    /// <param name="position">Vị trí cần đặt object</param>
    /// <param name="rotation">Góc xoay của object</param>
    /// <returns>Object được lấy từ pool</returns>
    public T GetObject(Vector3 position, Quaternion rotation)
    {
        // Nếu pool rỗng, tạo thêm object mới
        if (pool.Count == 0)
        {
            CreateNewObject();
        }

        // Lấy object từ pool và thiết lập vị trí, góc xoay
        T obj = pool.Dequeue();
        obj.transform.position = position;
        obj.transform.rotation = rotation;
        obj.gameObject.SetActive(true);
        return obj;
    }

    /// <summary>
    /// Trả một object về pool
    /// </summary>
    /// <param name="obj">Object cần trả về pool</param>
    public void ReturnObject(T obj)
    {
        obj.gameObject.SetActive(false);
        pool.Enqueue(obj);
    }
}
