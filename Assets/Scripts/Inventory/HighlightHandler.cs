using Game.Inventory;
using UnityEngine;

namespace Inventory
{
    public class HighlightHandler
    {
        private readonly RectTransform _highlighter;
        private Vector2Int _oldPosition;
        private InventoryItem _itemToHighlight;

        public HighlightHandler(RectTransform highlighter)
        {
            _highlighter = highlighter;
        }
        
        public void HandleHighlight(Vector2Int? posOnGrid, ItemGrid selectedItemGrid, InventoryItem selectedItem)
        {
            if (!selectedItemGrid || posOnGrid == null)
            {
                SetActive(false);
                return;
            }

            var positionOnGrid = posOnGrid.Value;
            
            if (_oldPosition == positionOnGrid)
                return;            
            
            if (!selectedItem)
            {
                _itemToHighlight = selectedItemGrid.GetItem(positionOnGrid);
                
                if (_itemToHighlight)
                {
                    SetSize(_itemToHighlight);
                    SetPosition(selectedItemGrid, _itemToHighlight);
                }
                
                SetActive(_itemToHighlight);
            }
            else
            {
                SetActive(selectedItemGrid.BoundaryCheck(
                    positionOnGrid.x, 
                    positionOnGrid.y,
                    selectedItem.Width, 
                    selectedItem.Height));
            
                SetSize(selectedItem);
                SetPosition(selectedItemGrid, selectedItem, positionOnGrid.x, positionOnGrid.y);
            }
        }

        private void SetActive(bool value)
        {
            _highlighter.gameObject.SetActive(value);
        }

        private void SetSize(InventoryItem targetItem)
        {
            _highlighter.sizeDelta = new Vector2
            {
                x = targetItem.Width * GameConfig.TileSize,
                y = targetItem.Height * GameConfig.TileSize,
            };
        }
        
        public void SetParent(ItemGrid targetGrid)
        {
            _highlighter.SetParent(targetGrid.transform);
        }

        private void SetPosition(ItemGrid targetGrid, InventoryItem targetItem)
        {
            _highlighter.localPosition = targetGrid.CalculatePositionOnGrid(targetItem, targetItem.onGridPositionX, targetItem.onGridPositionY);
        }

        private void SetPosition(ItemGrid targetGrid, InventoryItem targetItem, int posX, int posY)
        {
            _highlighter.localPosition = targetGrid.CalculatePositionOnGrid(targetItem, posX, posY);
        }
    }
}