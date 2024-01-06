using Game.Inventory;
using UnityEngine;

namespace Inventory
{
    public class HighlightHandler
    {
        private readonly GameObject _highlightPrefab;
        private Vector2Int _oldPosition;
        private InventoryItem _itemToHighlight;
        private readonly RectTransform _rt;

        public HighlightHandler(GameObject highlightPrefab)
        {
            _highlightPrefab = highlightPrefab;
            _rt = highlightPrefab.transform as RectTransform;
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

            if (selectedItem)
            {
                OnSelectedItem(positionOnGrid, selectedItemGrid, selectedItem);
                // SetActive(selectedItemGrid.BoundaryCheck(
                //     positionOnGrid.x,
                //     positionOnGrid.y,
                //     selectedItem.Width,
                //     selectedItem.Height));
                //
                // SetSize(selectedItem);
                // SetPosition(selectedItemGrid, selectedItem, positionOnGrid.x, positionOnGrid.y);
            }
            else
            {
                // _itemToHighlight = selectedItemGrid.GetItem(positionOnGrid);
                //
                // if (_itemToHighlight)
                // {
                //     SetSize(_itemToHighlight);
                //     SetPosition(selectedItemGrid, _itemToHighlight);
                // }
                //
                // SetActive(_itemToHighlight);
            }
        }

        private void OnSelectedItem(Vector2Int positionOnGrid, ItemGrid targetGrid, InventoryItem selectedItem)
        {
            for (var x = 0; x < selectedItem.Shape.GetLength(0); x++)
            {
                for (int y = 0; y < selectedItem.Shape.GetLength(1); y++)
                {
                    var val = selectedItem.Shape[x, y];
                    if (val == 1)
                    {
                        Debug.Log("item");
                    }
                    else if (val == 2)
                    {
                        Debug.Log("effect");
                    }
                }
            }
        }

        private void SetActive(bool value)
        {
            _highlightPrefab.gameObject.SetActive(value);
        }

        private void SetSize(InventoryItem targetItem)
        {
            _rt.sizeDelta = new Vector2
            {
                x = targetItem.Width * GameConfig.TileSize,
                y = targetItem.Height * GameConfig.TileSize,
            };
        }
        
        public void SetParent(ItemGrid targetGrid)
        {
            _rt.SetParent(targetGrid.transform);
        }

        private void SetPosition(ItemGrid targetGrid, InventoryItem targetItem)
        {
            _rt.localPosition = targetGrid.CalculatePositionOnGrid(targetItem, targetItem.onGridPositionX, targetItem.onGridPositionY);
        }

        private void SetPosition(ItemGrid targetGrid, InventoryItem targetItem, int posX, int posY)
        {
            _rt.localPosition = targetGrid.CalculatePositionOnGrid(targetItem, posX, posY);
        }
    }
}