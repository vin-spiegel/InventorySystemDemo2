using System.Collections.Generic;
using Game.Inventory;
using UnityEngine;
// ReSharper disable Unity.InefficientPropertyAccess

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
                _inventoryHighlight.SetParent(value);
                
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

    private InventoryHighlight _inventoryHighlight;
    
    private void Start()
    {
        var canvas = GetComponent<Canvas>();
        _scaleFactor = canvas.scaleFactor;
        
        _inventoryHighlight = GetComponent<InventoryHighlight>();
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

        if (!_selectedItemGrid)
        {
            _inventoryHighlight.SetActive(false);
            return;
        }
        
        HandleHighlight();
        
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

    private InventoryItem _itemToHighlight;
    private Vector2Int _oldPosition;

    private void HandleHighlight()
    {
        var positionOnGrid = GetTileGridPosition();
        
        if (_oldPosition == positionOnGrid)
            return;            

        if (!_selectedItem)
        {
            _itemToHighlight = _selectedItemGrid.GetItem(positionOnGrid.x, positionOnGrid.y);
            
            if (_itemToHighlight)
            {
                _inventoryHighlight.SetActive(true);
                _inventoryHighlight.SetSize(_itemToHighlight);
                _inventoryHighlight.SetPosition(_selectedItemGrid, _itemToHighlight);
            }
            else
            {
                _inventoryHighlight.SetActive(false);
            }
        }
        else
        {
            _inventoryHighlight.SetActive(_selectedItemGrid.BoundaryCheck(
                positionOnGrid.x, 
                positionOnGrid.y,
                _selectedItem.Width, 
                _selectedItem.Height));
            
            _inventoryHighlight.SetSize(_selectedItem);
            _inventoryHighlight.SetPosition(_selectedItemGrid, _selectedItem, positionOnGrid.x, positionOnGrid.y);
        }
    }

    private void LeftMouseButtonPressed()
    {
        var pos = GetTileGridPosition();

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

    private Vector2Int GetTileGridPosition()
    {
        var position = Input.mousePosition;

        if (_selectedItem)
        {
            position.x -= (_selectedItem.Width - 1) * GameConfig.TileSize / 2.0f;
            position.y += (_selectedItem.Height - 1) * GameConfig.TileSize / 2.0f;
        }

        return _selectedItemGrid.GetTileGridPosition(position, _scaleFactor);
    }
}
