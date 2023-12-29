using Game.Inventory;
using UnityEngine;
// ReSharper disable MemberCanBeMadeStatic.Global

namespace Inventory
{
    public class ItemPlacementHandler
    {
        private InventoryItem _overlapItem;
            
        public void HandlePlaceItem(ref InventoryContainer container, ref InventoryItem selectedItem, Vector2Int currentGridPosition)
        {
            if (container == null)
                return;
            
            if (!selectedItem)
            {
                if (PickUpItem(ref container, currentGridPosition.x, currentGridPosition.y, out var item))
                {
                    // Debug.Log("Pickup Item");
                    item.transform.SetAsLastSibling();
                    selectedItem = item;
                }
            }
            else
            {
                if (!PlaceItemWithCheckOverlap(ref container, selectedItem, currentGridPosition.x, currentGridPosition.y)) 
                    return;
            
                selectedItem = null;
                
                if (_overlapItem)
                {
                    selectedItem = _overlapItem;
                    selectedItem.transform.SetAsLastSibling();
                    _overlapItem = null;
                }
            }
        }

        private bool PickUpItem(ref InventoryContainer container, int x, int y, out InventoryItem item)
        {
            item = container.Pop(x, y);

            return item;
        }

        public bool InsertItem(InventoryContainer container, InventoryItem item)
        {
            var posOnGrid = container.GetSpaceForPutItem(item);
            
            if (posOnGrid != null)
            {
                PlaceItem(ref container, item, posOnGrid.Value.x, posOnGrid.Value.y);
                return true;
            }

            return false;
        }
        
        private bool PlaceItemWithCheckOverlap(ref InventoryContainer container, InventoryItem inventoryItem, int posX, int posY)
        {
            // if (!inventoryItem.itemData.CheckBoundary(posX, posY, container.Row, container.Column))
            // {
            //     Debug.Log(1);
            //     return false;
            // }
            //
            // if (!CheckOverlap(container, posX, posY, inventoryItem.Width, inventoryItem.Height))
            // {
            //     Debug.Log(2);
            //     _overlapItem = null;
            //     return false;
            // }
            //
            // if (_overlapItem)
            // {
            //     Debug.Log(3);
            //     container.Remove(_overlapItem);
            // }

            return PlaceItem(ref container, inventoryItem, posX, posY);
        }

        private static bool PlaceItem(ref InventoryContainer container, InventoryItem inventoryItem, int posX, int posY)
        {
            if (container.Put(inventoryItem, posX, posY))
            {
                inventoryItem.transform.localPosition = new Vector2
                {
                    x = posX * GameConfig.TileSize + GameConfig.TileSize * inventoryItem.Width / 2.0f,
                    y = -(posY * GameConfig.TileSize + GameConfig.TileSize * inventoryItem.Height / 2.0f),
                };
                return true;
            }

            return false;
        }
        
        private bool CheckOverlap(InventoryContainer container, int posX, int posY, int width, int height)
        {
            return false;
            
            // for (var x = 0; x < width; x++)
            // {
            //     for (var y = 0; y < height; y++)
            //     {
            //         var item = container.Get(posX + x, posY + y);
            //         if (item) 
            //         {
            //             Debug.Log($"Overlap item: {item.itemData.name}");
            //             _overlapItem = item;
            //             return false;
            //         }
            //         //
            //         // if (!item) 
            //         //     continue;
            //         //
            //         // if (!_overlapItem)
            //         // {
            //         //     _overlapItem = item;
            //         //     Debug.Log($"overlap item: {_overlapItem.itemData.name} {item.itemData.name}");
            //         // }
            //         // else
            //         // {
            //         //     if (_overlapItem != item)
            //         //     {
            //         //         return false;
            //         //     }
            //         // }
            //     }
            // }
            //
            // return true;
        }
    }
}