using Game.Inventory;
using UnityEngine;

namespace Inventory
{
    [RequireComponent(typeof(Inventory))]
    public class InventoryView : MonoBehaviour
    {
        public void Initialize(int row, int column)
        {
            ((RectTransform)transform).sizeDelta = new Vector2(row, column) * GameConfig.TileSize;    
        }
    }
}