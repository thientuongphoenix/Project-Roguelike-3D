using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopScrollManager : MonoBehaviour
{
    public ScrollRect scrollRect;
    public RectTransform content;
    public List<RectTransform> itemSlots;
    private int currentIndex = 0;

    /// <summary>
    /// Chuyển đến vật phẩm tiếp theo
    /// </summary>
    public void NextItem()
    {
        if (currentIndex < itemSlots.Count - 1)
        {
            currentIndex++;
            ScrollToItem(currentIndex);
        }
    }

    /// <summary>
    /// Chuyển đến vật phẩm trước đó
    /// </summary>
    public void PreviousItem()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            ScrollToItem(currentIndex);
        }
    }

    /// <summary>
    /// Lướt tới vật phẩm theo index
    /// </summary>
    private void ScrollToItem(int index)
    {
        float itemWidth = itemSlots[index].rect.width;
        float targetPosX = -index * itemWidth;
        content.anchoredPosition = new Vector2(targetPosX, content.anchoredPosition.y);
    }

    private void Start()
    {
        // Lấy danh sách các vật phẩm trong content
        itemSlots = new List<RectTransform>();
        foreach (Transform child in content)
        {
            itemSlots.Add(child.GetComponent<RectTransform>());
        }
    }
}
