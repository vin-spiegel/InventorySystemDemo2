using UnityEngine;

namespace Inventory
{
    public class InventoryHandler
    {
        private readonly InventoryController _inventoryController;

        public InventoryHandler()
        {
            _inventoryController = Object.FindObjectOfType<InventoryController>();
        }
    }
}