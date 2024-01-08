using UnityEngine;

namespace Inventory
{
    public class HighlightHandler
    {
        private readonly HighlightPool _pool;
        private Vector2Int _oldPosition;

        public HighlightHandler(HighlightPool pool)
        {
            _pool = pool;
        }
        
        public void HandleHighlight(Vector2Int? posOnGrid, ItemGrid selectedItemGrid, InventoryItem selectedItem)
        {
            if (!selectedItemGrid || posOnGrid == null)
                return;

            var positionOnGrid = posOnGrid.Value;
            
            if (_oldPosition == positionOnGrid)
                return;

            if (selectedItem)
            {
                HighlightItem(selectedItemGrid, selectedItem, positionOnGrid.x, positionOnGrid.y);
                _oldPosition = positionOnGrid;
            }
            else
            {
                var itemToHighlight = selectedItemGrid.GetItem(positionOnGrid);

                if (itemToHighlight)
                {
                    HighlightItem(selectedItemGrid, itemToHighlight, itemToHighlight.gridX, itemToHighlight.gridY);
                    _oldPosition = positionOnGrid;
                }
                else
                {
                    _pool.Clear();
                }
            }
        }

        // TODO: BoundaryCheck해서 벗어나면 빨간색으로 표기
        // Container 만들어서 SetParent로 풀 관리하기. 현재는 그냥 그리드에 다 포함시키는 중
        private void HighlightItem(ItemGrid targetGrid, InventoryItem item, int gridX, int gridY)
        {
            _pool.Clear();
            
            for (var x = 0; x < item.Shape.GetLength(1); x++)
            {
                for (var y = 0; y < item.Shape.GetLength(0); y++)
                {
                    var val = item.Shape[y, x];
                    
                    if (val == 0)
                        continue;
                    
                    var highlight = _pool.Get();
                    highlight.transform.SetParent(targetGrid.transform);
                    highlight.transform.localPosition = item.CalculatePositionOnGridWithShape(gridX + x, gridY + y);
                    
                    var image = _pool.GetHighlightImage(highlight);
                    if (val == 1)
                    {
                        image.color = new Color(1f, 1f, 1f, 0.5f);
                    }
                    else if (val == 2)
                    {
                        image.color = new Color(1f, 0.9f, 0.1f, 0.5f);
                    }
                }
            }
            
        }
    }
}