using Game.Inventory;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
// ReSharper disable UnassignedField.Global

namespace Inventory
{
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

        public int gridX;
        public int gridY;

        public bool rotated;

        private Vector2 _firstOne;

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

                _firstOne = FindFirstOne();
            }
        }
        
        private Vector2 FindFirstOne()
        {
            for (var x = 0; x < Shape.GetLength(1); x++)
            {
                for (var y = 0; y < Shape.GetLength(0); y++)
                {
                    if (Shape[y, x] == 1)
                    {
                        return new Vector2(x, y);
                    }
                }
            }

            return Vector2.zero;
        }
        
        private int _rotation;
        
        public Vector2 CalculatePositionOnGridWithShape(int x, int y)
        {
            return new Vector2(
                (x - _firstOne.x) * GameConfig.TileSize + GameConfig.TileSize / 2.0f, 
                -((y - _firstOne.y) * GameConfig.TileSize + GameConfig.TileSize / 2.0f));
        }

        public void Rotate()
        {
            _rotation -= 90;
            if(_rotation <= -360) 
            {
                _rotation = 0;
            }
            
            rotated = !rotated;
            transform.rotation = Quaternion.Euler(0, 0, _rotation);
        }
    }
}
