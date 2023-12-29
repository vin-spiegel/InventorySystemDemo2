using System;
using Game.Inventory;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory
{
    public class InventoryItem : MonoBehaviour
    {
        public string id = Guid.NewGuid().ToString();
        public ItemData itemData;
        public int Height
        {
            get
            {
                if (!rotated)
                    return itemData.height;
                return itemData.width;
            }
        }
    
        public int Width
        {
            get
            {
                if (!rotated)
                    return itemData.width;
                return itemData.height;
            }
        }

        public bool rotated;
    
        // TODO: 스크립터블 오브젝트로 세팅
        public void Initialize(ItemData item)
        {
            itemData = item;

            GetComponent<Image>().sprite = item.itemIcon;

            var rect = transform as RectTransform;
            if (rect)
            {
                rect.sizeDelta = new Vector2
                {
                    x = Width * GameConfig.TileSize,
                    y = Height * GameConfig.TileSize
                };
            }
        }

        public void Rotate()
        {
            rotated = !rotated;
            transform.rotation = Quaternion.Euler(0, 0, rotated ? 90f : 0f);
        }
    }
}
