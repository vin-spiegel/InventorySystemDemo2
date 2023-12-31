using Game.Inventory;
using UnityEngine;
// ReSharper disable ParameterHidesMember
// ReSharper disable Unity.PerformanceCriticalCodeInvocation
// ReSharper disable MemberCanBeMadeStatic.Local

public class ItemGrid : MonoBehaviour
{
    [SerializeField] private int width, height;
    
    private Vector2 _positionOnGrid = Vector2.zero;
    private Vector2Int _tileGridPosition = Vector2Int.zero;
    private InventoryItem[,] _inventoryItemSlot;
    
    private void Start()
    {
        _inventoryItemSlot = new InventoryItem[width, height];
        ((RectTransform)transform).sizeDelta = new Vector2(width, height) * GameConfig.TileSize;    
    }

    public Vector2Int GetTileGridPosition(Vector2 mousePosition, float scaleFactor = 1f)
    {
        var position = transform.position;
        _positionOnGrid.x = mousePosition.x - position.x;
        _positionOnGrid.y = position.y - mousePosition.y;
        
        _tileGridPosition.x = (int)(_positionOnGrid.x / (GameConfig.TileSize * scaleFactor));
        _tileGridPosition.y = (int)(_positionOnGrid.y / (GameConfig.TileSize * scaleFactor));
        
        return _tileGridPosition;
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
            ClearGridReference(overlapItem);
        }

        return PlaceItem(inventoryItem, posX, posY);
    }

    // TODO: 다양한 모양의 위치 정보를 가진 아이템도 고려 할 것. 현재는 직사각형이라고 상정중
    public bool PlaceItem(InventoryItem inventoryItem, int posX, int posY)
    {
        for (var x = 0; x < inventoryItem.Width; x++)
        {
            for (var y = 0; y < inventoryItem.Height; y++)
            {
                _inventoryItemSlot[posX + x, posY + y] = inventoryItem;
            }
        }

        inventoryItem.onGridPositionX = posX;
        inventoryItem.onGridPositionY = posY;

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
                if (!_inventoryItemSlot[posX + x, posY + y]) 
                    continue;
                
                if (!overlapItem)
                {
                    overlapItem = _inventoryItemSlot[posX + x, posY + y];
                }
                else
                {
                    if (overlapItem != _inventoryItemSlot[posX + x, posY + y])
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
        item = _inventoryItemSlot[x, y];

        if (!item)
            return false;
        
        ClearGridReference(item);
        
        return item;
    }

    private void ClearGridReference(InventoryItem item)
    {
        for (var ix = 0; ix < item.Width; ix++)
        {
            for (var iy = 0; iy < item.Height; iy++)
            {
                _inventoryItemSlot[item.onGridPositionX + ix, item.onGridPositionY + iy] = null;
            }
        }
    }

    public Vector2 CalculatePositionOnGrid(InventoryItem item, int x, int y)
    {
        return new Vector2
        {
            x = x * GameConfig.TileSize + GameConfig.TileSize * item.Width / 2.0f,
            y = -(y * GameConfig.TileSize + GameConfig.TileSize * item.Height / 2.0f),
        };
    }

    public InventoryItem GetItem(int x, int y)
    {
        return _inventoryItemSlot[x, y];
    }

    public Vector2Int? FindSpaceForObject(InventoryItem itemToInsert)
    {
        var itemWidth = itemToInsert.Width;
        var itemHeight = itemToInsert.Height;
    
        // Search for a free spot by starting from the top-left corner and moving right and then down
        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                if (IsAreaFree(x, y, itemWidth, itemHeight))
                {
                    return new Vector2Int(x, y);
                }
            }
        }

        return null;
    }
    
    private bool IsAreaFree(int posX, int posY, int width, int height)
    {
        if (_inventoryItemSlot == null || posX < 0 || posY < 0 || 
            posX + width > _inventoryItemSlot.GetLength(0) || 
            posY + height > _inventoryItemSlot.GetLength(1))
        {
            return false;
        }
        
        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++)
            {
                if (_inventoryItemSlot[posX + x, posY + y])
                {
                    return false;
                }
            }
        }
        
        return true;
    }
}
