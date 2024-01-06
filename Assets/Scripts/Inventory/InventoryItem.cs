using Game.Inventory;
using UnityEngine;
using UnityEngine.UI;
// ReSharper disable UnassignedField.Global

namespace Inventory
{
    public class InventoryItemDataProxy
    {
        private readonly ItemData _data;

        internal bool Rotated { get; set; }
        internal int Height => !Rotated ? _data.height : _data.width;
        internal int Width => !Rotated ? _data.height : _data.width;
        internal int X { get; set; }
        internal int Y { get; set; }
        internal int[,] Shape => _data.shape;

        public InventoryItemDataProxy(ItemData data)
        {
            _data = data;
        }
    }

    public class InventoryItem : MonoBehaviour
    {
        public ItemData itemData;

        public int[,] Shape => itemData.shape;
        
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

        public int onGridPositionX;
        public int onGridPositionY;

        public bool rotated;
    
        public void Initialize(ItemData item)
        {
            itemData = item;

            GetComponent<Image>().sprite = item.sprite;

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
