using System.Collections.Generic;
using Game.Inventory;
using UnityEngine;

// ReSharper disable Unity.InefficientPropertyAccess

namespace Inventory
{
    public class InventoryController : MonoBehaviour
    {
        [SerializeField] private Canvas canvas;
        [SerializeField] private List<ItemData> items = new();
        [SerializeField] private GameObject itemPrefab;
        [SerializeField] private RectTransform highlighter;
        
        private float _scaleFactor;
        private InventoryContainer _currentContainer;
        private InventoryItem _selectedItem;
        private HighlightHandler _highlightHandler;
        private ItemPlacementHandler _itemPlacementHandler;
        private Transform _currentInventoryTransform;

        private void Start()
        {
            _scaleFactor = canvas.scaleFactor;
            _highlightHandler = new HighlightHandler(highlighter);
            _itemPlacementHandler = new ItemPlacementHandler();
        }
    
        private void Update()
        {
            if (_selectedItem)
            {
                _selectedItem.transform.position = Input.mousePosition;
            }
        
            if (Input.GetKeyDown(KeyCode.Q))
            {
                CreateRandomItem();
            }

            if (Input.GetKeyDown(KeyCode.W))
            {
                InsertRandomItem();
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                RotateSelectItem();
            }

            
            if (_currentInventoryTransform)
            {
                var tilePosition = GetTileGridPosition();
                
                // _highlightHandler.HandleHighlight(_currentContainer, _selectedItem, tilePosition);
                if (Input.GetMouseButtonDown(0))
                {
                    Debug.Log("Mouse button pressed. Handling item placement...");
                    _itemPlacementHandler.HandlePlaceItem(ref _currentContainer, ref _selectedItem, tilePosition);
                }
            }
        
        }

        public void ChangeInventory(InventoryContainer container, Transform currentInventoryTransform)
        {
            _currentContainer = container;
            _currentInventoryTransform = currentInventoryTransform;
            
            if (currentInventoryTransform)
            {
                _highlightHandler.ChangeInventory(currentInventoryTransform);
                
                if (_selectedItem)
                {
                    _selectedItem.transform.SetParent(currentInventoryTransform);
                }
            }
        }
        
        private void RotateSelectItem()
        {
            if (!_selectedItem)
                return;

            _selectedItem.Rotate();
        }

        private void CreateRandomItem()
        {
            if (_selectedItem || !_currentInventoryTransform)
                return;
        
            var item = CreateItem();
            _selectedItem = item;
        }

        private InventoryItem CreateItem()
        {
            var item = Instantiate(itemPrefab).GetComponent<InventoryItem>();
            item.Initialize(items[Random.Range(0, items.Count)]);
            item.transform.SetParent(_currentInventoryTransform);
            item.transform.SetAsLastSibling();
            item.transform.localScale = Vector3.one;
            return item;
        }

        private void InsertRandomItem()
        {
            if (!_currentInventoryTransform)
                return;
        
            var item = CreateItem();
            if (item)
            {
                InsertItem(item);
            }
        }

        private void InsertItem(InventoryItem item)
        {
            if (!_itemPlacementHandler.InsertItem(_currentContainer, item))
            {
                Destroy(item.gameObject);
            }
        }

        private Vector2Int GetTileGridPosition()
        {
            var mousePosition = Input.mousePosition;

            if (_selectedItem)
            {
                mousePosition.x -= (_selectedItem.Width - 1) * GameConfig.TileSize / 2.0f;
                mousePosition.y += (_selectedItem.Height - 1) * GameConfig.TileSize / 2.0f;
            }

            var viewPosition = _currentInventoryTransform.position;
            
            return new Vector2Int(
                (int)((mousePosition.x - viewPosition.x) / (GameConfig.TileSize * _scaleFactor)), 
                (int)((viewPosition.y - mousePosition.y) / (GameConfig.TileSize * _scaleFactor)));
        }
    }
}
