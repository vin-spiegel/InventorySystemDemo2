using Game.Inventory;
using UnityEngine;

// ReSharper disable ParameterHidesMember
// ReSharper disable Unity.PerformanceCriticalCodeInvocation
// ReSharper disable MemberCanBeMadeStatic.Local

namespace Inventory
{
    public class ItemGrid : MonoBehaviour
    {
        [SerializeField] private int width, height;
    
        private InventoryContainer _inventoryContainer;

        private void Start()
        {
            _inventoryContainer = new InventoryContainer(width, height, this.name);
            ((RectTransform)transform).sizeDelta = new Vector2(width, height) * GameConfig.TileSize;    
        }

        public bool PlaceItem(InventoryItem inventoryItem, int posX, int posY, ref InventoryItem overlapItem)
        {
            if (!BoundaryCheck(posX, posY, inventoryItem.Width, inventoryItem.Height))
            {
                return false;
            }

            if (!OverlapCheck(posX, posY, inventoryItem.Width, inventoryItem.Height, ref overlapItem))
            {
                overlapItem = null;
                return false;
            }

            if (overlapItem)
            {
                _inventoryContainer.Remove(overlapItem);
            }

            return PlaceItem(inventoryItem, posX, posY);
        }

        // TODO: 다양한 모양의 위치 정보를 가진 아이템도 고려 할 것. 현재는 직사각형이라고 상정중
        public bool PlaceItem(InventoryItem inventoryItem, int posX, int posY)
        {
            _inventoryContainer.Put(inventoryItem, posX, posY);
            
            inventoryItem.transform.localPosition = new Vector2
            {
                x = posX * GameConfig.TileSize + GameConfig.TileSize * inventoryItem.Width / 2.0f,
                y = -(posY * GameConfig.TileSize + GameConfig.TileSize * inventoryItem.Height / 2.0f),
            };
            
            return true;
        }

        private bool OverlapCheck(int posX, int posY, int width, int height, ref InventoryItem overlapItem)
        {
            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    var item = _inventoryContainer.Get(posX + x, posY + y);
                    
                    if (!item) 
                        continue;
                    
                    if (!overlapItem)
                    {
                        overlapItem = item;
                    }
                    else
                    {
                        if (overlapItem != item)
                        {
                            return false;
                        }
                    }
                }
            }
        
            return true;
        }

        private bool PositionCheck(int posX, int posY)
        {
            if (posX < 0 || posY < 0)
                return false;

            return posX < width && posY < height;
        }

        public bool BoundaryCheck(int posX, int posY, int width, int height)
        {
            return PositionCheck(posX, posY) && PositionCheck(posX + width - 1,posY + height - 1);
        }

        public bool PickUpItem(int x, int y, out InventoryItem item)
        {
            item = _inventoryContainer.Get(x, y);

            if (!item)
                return false;
        
            _inventoryContainer.Remove(item);
        
            return item;
        }

        private InventoryItem GetItem(int x, int y)
        {
            return _inventoryContainer.Get(x,y);
        }

        public InventoryItem GetItem(Vector2Int position)
        {
            return GetItem(position.x, position.y);
        }

        public Vector2Int? FindSpaceForObject(InventoryItem itemToInsert)
        {
            return _inventoryContainer.GetSpaceForPutItem(itemToInsert);
        }
    }
}
