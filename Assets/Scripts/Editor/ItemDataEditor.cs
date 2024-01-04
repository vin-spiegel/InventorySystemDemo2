#if UNITY_EDITOR
using System.Threading;
using Inventory;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(ItemData))]
    public class ItemDataEditor : UnityEditor.Editor
    {
        private const float MinSize = 32;
        private readonly Color _activeColor = new(1f, 1f, 1f, 0.5f);
        private readonly Color _alternateColor = new Color(1f, 0.9f, 0.1f, 0.5f);

        public override void OnInspectorGUI()
        {
            var itemData = (ItemData) target;

            DrawDefaultInspector();

            if (itemData.itemIcon && itemData.shape == null)
            {
                var fullWidth = Mathf.CeilToInt(itemData.itemIcon.texture.width / MinSize);  
                var fullHeight = Mathf.CeilToInt(itemData.itemIcon.texture.height / MinSize); 
                itemData.shape = new int[10, 10];
                
                var offsetX = (10 - fullWidth) / 2;
                var offsetY = (10 - fullHeight) / 2;

                for(var i = offsetY; i < offsetY + fullHeight; i++)
                {
                    for(var j = offsetX; j < offsetX + fullWidth; j++)
                    {
                        itemData.shape[i,j] = 1;
                    }
                }
            }

            EditorGUILayout.LabelField("Shape Table");
            var rows = itemData.shape.GetLength(0);
            var cols = itemData.shape.GetLength(1);
        
            EditorGUILayout.Space(10);
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
        
            var gridRect = GUILayoutUtility.GetRect(MinSize * cols, MinSize * rows, GUILayout.Width(MinSize * cols), GUILayout.Height(MinSize * rows));
            var textureDrew = false;
        
            for (var i = 0; i < rows; i++)
            {
                for (var j = 0; j < cols; j++)
                {
                    var x = gridRect.x + j * MinSize;
                    var y = gridRect.y + i * MinSize;
                    var cellRect = new Rect(x, y, MinSize, MinSize);
                    var e = Event.current;

                    if (e.type is EventType.MouseDown && cellRect.Contains(e.mousePosition))
                    {
                        if (e.button == 1 )
                        {
                            itemData.shape[i, j] = itemData.shape[i, j] != 2 ? 2 : 0;
                            Repaint();
                        }
                        else if (e.button == 0)
                        {
                            itemData.shape[i, j] = itemData.shape[i, j] != 1 ? 1 : 0;
                            Repaint();
                        }
                    }

                    if (itemData.shape[i, j] == 1)
                    {
                        if (!textureDrew)
                        {
                            GUI.DrawTexture(new Rect(x, y, itemData.itemIcon.texture.width, itemData.itemIcon.texture.height), itemData.itemIcon.texture);
                            textureDrew = true;
                        }

                        EditorGUI.DrawRect(cellRect, _activeColor);
                    }
                    else if (itemData.shape[i, j] == 2)
                    {
                        EditorGUI.DrawRect(cellRect, _alternateColor);
                    }

                    Handles.BeginGUI();
                    Handles.color = Color.black;
                    Handles.DrawLine(new Vector2(cellRect.xMin, cellRect.yMin), new Vector2(cellRect.xMax, cellRect.yMin));
                    Handles.DrawLine(new Vector2(cellRect.xMax, cellRect.yMin), new Vector2(cellRect.xMax, cellRect.yMax));
                    Handles.DrawLine(new Vector2(cellRect.xMin, cellRect.yMax), new Vector2(cellRect.xMax, cellRect.yMax));
                    Handles.DrawLine(new Vector2(cellRect.xMin, cellRect.yMin), new Vector2(cellRect.xMin, cellRect.yMax));
                    Handles.EndGUI();
                }
            }

            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }
    }
}
#endif