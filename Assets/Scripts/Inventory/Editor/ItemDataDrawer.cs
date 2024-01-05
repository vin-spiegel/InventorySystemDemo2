using UnityEditor;
using UnityEngine;

namespace Inventory.Editor
{
    public class ItemDataDrawer
    {
        private readonly ItemData _itemData;
        private readonly Color _activeColor = new(1f, 1f, 1f, 0.5f);
        private readonly Color _alternateColor = new(1f, 0.9f, 0.1f, 0.5f);
        
        public ItemDataDrawer(ItemData itemData)
        {
            _itemData = itemData;
        }
        
        public void DrawGridLines(Rect cellRect)
        {
            Handles.BeginGUI();
            Handles.color = Color.black;
            Handles.DrawLine(new Vector2(cellRect.xMin, cellRect.yMin), new Vector2(cellRect.xMax, cellRect.yMin));
            Handles.DrawLine(new Vector2(cellRect.xMax, cellRect.yMin), new Vector2(cellRect.xMax, cellRect.yMax));
            Handles.DrawLine(new Vector2(cellRect.xMin, cellRect.yMax), new Vector2(cellRect.xMax, cellRect.yMax));
            Handles.DrawLine(new Vector2(cellRect.xMin, cellRect.yMin), new Vector2(cellRect.xMin, cellRect.yMax));
            Handles.EndGUI();
        }

        public void DrawShape(Rect cellRect, int row, int col, ref bool textureDrew)
        {
            switch (_itemData.shape[row, col])
            {
                case 1:
                    DrawActiveShape(cellRect, row, col, ref textureDrew);
                    break;
                case 2:
                    EditorGUI.DrawRect(cellRect, _alternateColor);
                    break;
            }
        }

        private void DrawActiveShape(Rect cellRect, int row, int col, ref bool textureDrew)
        {
            if (!textureDrew && _itemData.itemIcon)
            {
                GUI.DrawTexture(new Rect(cellRect.x, cellRect.y, 
                    _itemData.itemIcon.texture.width, _itemData.itemIcon.texture.height), _itemData.itemIcon.texture);
                textureDrew = true;
            }

            EditorGUI.DrawRect(cellRect, _activeColor);
        }
    }
}