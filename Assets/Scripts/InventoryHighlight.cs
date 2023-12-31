using UnityEngine;

namespace Game.Inventory
{
    public class InventoryHighlight : MonoBehaviour
    {
        [SerializeField] private RectTransform highlighter;

        public void SetActive(bool value)
        {
            highlighter.gameObject.SetActive(value);
        }

        public void SetSize(InventoryItem targetItem)
        {
            highlighter.sizeDelta = new Vector2()
            {
                x = targetItem.Width * GameConfig.TileSize,
                y = targetItem.Height * GameConfig.TileSize,
            };
        }
        
        public void SetParent(ItemGrid targetGrid)
        {
            highlighter.SetParent(targetGrid.transform);
        }
        
        public void SetAsFirstSibling()
        {
            highlighter.SetAsFirstSibling();
        }
        
        public void SetPosition(ItemGrid targetGrid, InventoryItem targetItem)
        {
            highlighter.localPosition = targetGrid.CalculatePositionOnGrid(targetItem, targetItem.onGridPositionX, targetItem.onGridPositionY);
        }

        public void SetPosition(ItemGrid targetGrid, InventoryItem targetItem, int posX, int posY)
        {
            highlighter.localPosition = targetGrid.CalculatePositionOnGrid(targetItem, posX, posY);
        }

    }
}