using UnityEngine;

namespace Inventory
{
    public class InventoryContainer
    {
        private readonly InventoryItem[,] _inventoryItems;

        public InventoryContainer(int row, int column, string name)
        {
            this.Name = name;
            _inventoryItems = new InventoryItem[row, column];
        }

        public string Name { get; set; }

        public int Row => _inventoryItems.GetLength(0);
        public int Column => _inventoryItems.GetLength(1);
        
        public bool Put(InventoryItem item, int x, int y)
        {
            for (var ix = 0; ix < item.Width; ix++)
            {
                for (var iy = 0; iy < item.Height; iy++)
                {
                    _inventoryItems[x + ix, y + iy] = item;
                }
            }
            
            item.onGridPositionX = x;
            item.onGridPositionY = y;
            
            Debug.Log($"<color=cyan>Item {item.name}</color> Placed At <color=cyan>({x}, {y})</color>");

            return true;
        }

        public InventoryItem Get(int x, int y)
        {
            return _inventoryItems[x, y];
        }

        public InventoryItem Pop(int x, int y)
        {
            var item = _inventoryItems[x, y];

            if (item)
            {
                Remove(item);
                Debug.Log($"Successfully removed item at position (X:{x}, Y:{y})");
            }
            else
            {
                Debug.LogError($"No item to remove at position (X:{x}, Y:{y})");
            }

            return item;
        }

        public void Remove(InventoryItem item)
        {
            for (var ix = 0; ix < item.Width; ix++)
            {
                for (var iy = 0; iy < item.Height; iy++)
                {
                    var removeX = item.onGridPositionX + ix;
                    var removeY = item.onGridPositionY + iy;
                    _inventoryItems[removeX, removeY] = null;
                }
            }
        }
        
        public Vector2Int? GetSpaceForPutItem(InventoryItem itemToInsert)
        {
            // Search for a free spot by starting from the top-left corner and moving right and then down
            for (var y = 0; y < _inventoryItems.GetLength(0); y++)
            {
                for (var x = 0; x < _inventoryItems.GetLength(1); x++)
                {
                    if (IsAreaFree(x, y, itemToInsert.Width, itemToInsert.Height))
                    {
                        return new Vector2Int(x, y);
                    }
                }
            }

            return null;
        }
    
        private bool IsAreaFree(int posX, int posY, int width, int height)
        {
            if (_inventoryItems == null || posX < 0 || posY < 0 || 
                posX + width > _inventoryItems.GetLength(0) || 
                posY + height > _inventoryItems.GetLength(1))
            {
                return false;
            }
        
            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    var item = _inventoryItems[posX + x, posY + y];
                    if (item)
                    {
                        Debug.Log($"Grid position X:{posX + x}, Y:{posY + y} is occupied by item {item.itemData.name}.");
                        return false;
                    }
                }
            }
        
            return true;
        }
    }
}