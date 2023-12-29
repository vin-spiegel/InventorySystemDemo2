using Game.Inventory;
using UnityEngine;

namespace Inventory
{
    public static class InventoryItemExtensions
    {
        public static Vector2 GetLocalPosition(this InventoryItem item)
        {
            return new Vector2
            {
                x = item.itemData.x * GameConfig.TileSize + GameConfig.TileSize * item.Width / 2.0f,
                y = -(item.itemData.y * GameConfig.TileSize + GameConfig.TileSize * item.Height / 2.0f),
            };
        }
        
        public static Vector2 GetLocalPosition(this InventoryItem item, int x, int y)
        {
            return new Vector2
            {
                x = x * GameConfig.TileSize + GameConfig.TileSize * item.Width / 2.0f,
                y = -(y * GameConfig.TileSize + GameConfig.TileSize * item.Height / 2.0f),
            };
        }
    }

    public static class ItemDataExtensions
    {
        public static bool CheckBoundary(this ItemData itemData, int toPlaceX, int toPlaceY, int gridWidth, int gridHeight)
        {
            return IsWithinBounds(toPlaceX, toPlaceY, gridWidth, gridHeight) && 
                   IsWithinBounds(toPlaceX + itemData.width - 1,toPlaceY + itemData.height - 1, gridWidth, gridHeight);
        }
        
        private static bool IsWithinBounds(int posX, int posY, int gridWidth, int gridHeight)
        {
            if (posX < 0 || posY < 0)
                return false;

            return posX < gridWidth && posY < gridHeight;
        }
    }
}