using System.Collections.Generic;
using Game.Inventory;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

// ReSharper disable Unity.InefficientPropertyAccess

namespace Inventory
{
    public class InventoryController : MonoBehaviour
    {
        private ItemGrid _selectedItemGrid;

        public ItemGrid SelectedItemGrid
        {
            get => _selectedItemGrid;
            set
            {
                _selectedItemGrid = value;
            
                if (value)
                {
                    // _highlightHandler.SetParent(value);
                
                    if (_selectedItem)
                    {
                        _selectedItem.transform.SetParent(value.transform);
                    }
                }
            }
        }

        private float _scaleFactor;
    
        private InventoryItem _selectedItem;
        private InventoryItem _overlapItem;
    
        [SerializeField] private List<ItemData> items = new();
        [SerializeField] private GameObject itemPrefab;
        [SerializeField] private GameObject highlightPrefab;

        private HighlightHandler _highlightHandler;
        private Camera _mainCamera;

        private void Awake()
        {
            _mainCamera = Camera.main;
        }

        private void Start()
        {
            var canvas = GetComponent<Canvas>();
            _scaleFactor = canvas.scaleFactor;
            _highlightHandler = new HighlightHandler(new HighlightPool(highlightPrefab));
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

            _highlightHandler.HandleHighlight(GetTileGridPosition(), _selectedItemGrid, _selectedItem);

            if (Input.GetMouseButtonDown(0))
            {
                LeftMouseButtonPressed();
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
            if (_selectedItem || !_selectedItemGrid)
                return;
        
            var item = CreateItem();
            _selectedItem = item;
        }

        // todo: ItemSpawner로 만들 것
        private InventoryItem CreateItem()
        {
            var item = Instantiate(itemPrefab).GetComponent<InventoryItem>();
            item.Initialize(items[Random.Range(0, items.Count)]);
            item.transform.SetParent(_selectedItemGrid.transform);
            item.transform.SetAsLastSibling();
            item.transform.localScale = Vector3.one;
            return item;
        }

        private void InsertRandomItem()
        {
            if (!_selectedItemGrid)
                return;
        
            var item = CreateItem();
            if (item)
            {
                InsertItem(item);
            }
        }

        private void InsertItem(InventoryItem item)
        {
            var posOnGrid = _selectedItemGrid.FindSpaceForObject(item);

            if (posOnGrid != null)
            {
                _selectedItemGrid.PlaceItem(item, posOnGrid.Value.x, posOnGrid.Value.y);
            }
            else
            {
                Destroy(item.gameObject);
            }
        }

        private void LeftMouseButtonPressed()
        {
            var vec = GetTileGridPosition();
            if (vec == null)
                return;
            
            var pos = vec.Value;

            if (!_selectedItem)
            {
                if (_selectedItemGrid.PickUpItem(pos.x, pos.y, out var item))
                {
                    item.transform.SetAsLastSibling();
                    _selectedItem = item;
                }
            }
            else
            {
                if (!_selectedItemGrid.PlaceItem(_selectedItem, pos.x, pos.y, ref _overlapItem)) 
                    return;
            
                _selectedItem = null;
                
                if (_overlapItem)
                {
                    _selectedItem = _overlapItem;
                    _selectedItem.transform.SetAsLastSibling();
                    _overlapItem = null;
                }
            }
        }

        private Vector2Int? GetTileGridPosition()
        {
            if (!_selectedItemGrid)
                return null;
            
            // var position = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            var position = Input.mousePosition;

            if (_selectedItem)
            {
                position.x -= (_selectedItem.Width - 1) * GameConfig.TileSize / 2.0f;
                position.y += (_selectedItem.Height - 1) * GameConfig.TileSize / 2.0f;
            }

            return GetTileGridPosition(position);
        }

        private Vector2Int GetTileGridPosition(Vector2 mousePosition)
        {
            var position = _selectedItemGrid.transform.position;

            return new Vector2Int(
                (int)((mousePosition.x - position.x) / (GameConfig.TileSize * _scaleFactor)),
                (int)((position.y - mousePosition.y) / (GameConfig.TileSize * _scaleFactor))
            );
        }
    }
}
