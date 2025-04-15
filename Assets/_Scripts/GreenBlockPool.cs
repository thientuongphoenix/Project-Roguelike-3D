using UnityEngine;

public class GreenBlockPool : MonoBehaviour
{
    [Header("Pool Settings")]
    public GameObject greenBlockPrefab; // Prefab của green block
    public int initialPoolSize = 10; // Số lượng block ban đầu trong pool
    private string poolKey = "GreenBlock";

    private void Start()
    {
        // Khởi tạo pool cho green block
        PoolManager.Instance.CreatePool(poolKey, greenBlockPrefab.GetComponent<MonoBehaviour>(), initialPoolSize);
    }

    /// <summary>
    /// Lấy một green block từ pool
    /// </summary>
    public GameObject SpawnGreenBlock(Vector3 position)
    {
        if (greenBlockPrefab == null)
        {
            Debug.LogWarning("⚠ Green block prefab chưa được gán!");
            return null;
        }

        MonoBehaviour block = PoolManager.Instance.GetObject<MonoBehaviour>(poolKey, position, Quaternion.identity);
        return block?.gameObject;
    }

    /// <summary>
    /// Trả green block về pool
    /// </summary>
    public void ReturnToPool(GameObject block)
    {
        if (block != null)
        {
            PoolManager.Instance.ReturnObject(poolKey, block.GetComponent<MonoBehaviour>());
        }
    }
}
