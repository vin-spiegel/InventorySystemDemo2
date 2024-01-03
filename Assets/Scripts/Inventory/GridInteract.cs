using UnityEngine;
using UnityEngine.EventSystems;

namespace Inventory
{
    [RequireComponent(typeof(ItemGrid))]
    public class GridInteract : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private InventoryController _inventoryController;
        private ItemGrid _itemGrid;
    
        private void Awake()
        {
            _inventoryController = FindObjectOfType(typeof(InventoryController)) as InventoryController;
            _itemGrid = GetComponent<ItemGrid>();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _inventoryController.SelectedItemGrid = _itemGrid;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _inventoryController.SelectedItemGrid = null;
        }
    }
}
