using Game.Inventory;
using UnityEngine;

namespace Inventory
{
    public class HighlightHandler
    {
        private readonly RectTransform _highLighter;
        // private InventoryItem _itemToHighlight;
        private Vector2Int _oldPosition;

        public HighlightHandler(RectTransform highlighter)
        {
            _highLighter = highlighter;
        }
        
        public void ChangeInventory(Transform targetGrid)
        {
            _highLighter.SetParent(targetGrid.transform);
        }
        
        public void HandleHighlight(InventoryContainer container, InventoryItem selectedItem, Vector2Int positionOnGrid)
        {
            if (container == null)
            {
                _highLighter.gameObject.SetActive(false);
                return;
            }

            if (_oldPosition == positionOnGrid)
                return;    
            
            if (!selectedItem)
            {
                var itemToHighlight = container.Get(positionOnGrid.x, positionOnGrid.y);
                
                if (itemToHighlight)
                {
                    // Debug.Log($"On Highlight({container.name}) {itemToHighlight.itemData.name} : ({itemToHighlight.itemData.x},{itemToHighlight.itemData.y}) / {positionOnGrid}");
                    
                    _highLighter.gameObject.SetActive(true);
                    _highLighter.sizeDelta = new Vector2(itemToHighlight.Width, itemToHighlight.Height) * GameConfig.TileSize;
                    _highLighter.localPosition = itemToHighlight.GetLocalPosition();
                }
                else
                {
                    _highLighter.gameObject.SetActive(false);
                }
            }
            else
            {
                _highLighter.gameObject.SetActive(selectedItem.itemData.CheckBoundary(
                    positionOnGrid.x, 
                    positionOnGrid.y,
                    container.Row, 
                    container.Column));

                _highLighter.sizeDelta = new Vector2(selectedItem.Width, selectedItem.Height) * GameConfig.TileSize;
                _highLighter.localPosition = selectedItem.GetLocalPosition(positionOnGrid.x, positionOnGrid.y);
            }
        }
    }
}