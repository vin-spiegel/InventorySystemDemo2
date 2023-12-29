using Game.Inventory;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

// ReSharper disable ParameterHidesMember
// ReSharper disable Unity.PerformanceCriticalCodeInvocation
// ReSharper disable MemberCanBeMadeStatic.Local

namespace Inventory
{
    public class Inventory : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [FormerlySerializedAs("width")] public int row;
        [FormerlySerializedAs("height")] public int column;

        private InventoryContainer _container;
        private InventoryView _view;
        private InventoryController _inventoryController;

        private void Start()
        {
            _container = new InventoryContainer(row, column, this.name);
            _view = this.GetComponent<InventoryView>();
            _view.Initialize(row, column);
            _inventoryController = FindObjectOfType(typeof(InventoryController)) as InventoryController;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _inventoryController.ChangeInventory(_container, transform);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _inventoryController.ChangeInventory(null, null);
        }
    }
}
