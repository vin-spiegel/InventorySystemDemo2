using Editor;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

namespace Inventory.Editor
{
    [CustomEditor(typeof(ItemData))]
    public class ItemDataEditor : UnityEditor.Editor
    {
        private const float MinSize = 32;
        private bool _itemDataChanged;
        private ItemData _itemData;
        private ItemDataDrawer _itemDataDrawer;

        private void OnEnable()
        {
            _itemData = target as ItemData;
            if (_itemData)
            {
                _itemData.ItemDataChanged += UpdateChanges;
                _itemDataDrawer = new ItemDataDrawer(_itemData);
            }
        }
        
        private void UpdateChanges(ItemData itemData)
        {
            _itemDataChanged = true;
            _itemData = itemData;
        }
      
        public override void OnInspectorGUI()
        {
            if (_itemData.shape == null)
                return;
            
            DrawDefaultInspector();

            EditorGUILayout.Space(10);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField($"Shape Table{(_itemDataChanged ? "*" : "")}");
        
            using (new EditorGUI.DisabledScope(!ArrayUtils.CanTrimArray(_itemData.shape)))
            {
                if (GUILayout.Button("Trim", GUILayout.Width(80)))
                {
                    _itemData.SetShape(ArrayUtils.TrimArray(_itemData.shape));
                }
            }
            GUILayout.Space(10);
            
            using (new EditorGUI.DisabledScope(!_itemDataChanged)) 
            {
                if(GUILayout.Button("Save", GUILayout.Width(80)))
                {
                    _itemData.shapeJson = JsonConvert.SerializeObject(_itemData.shape);
                    EditorUtility.SetDirty(_itemData);
                    AssetDatabase.SaveAssets();
                    _itemDataChanged = false;
                }
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space(10);
            
            {
                EditorGUI.BeginChangeCheck();
        
                var row = EditorGUILayout.IntSlider("Rows", _itemData.shape.GetLength(0), 1, 10);
                var col = EditorGUILayout.IntSlider("Columns", _itemData.shape.GetLength(1), 1, 10);

                if (EditorGUI.EndChangeCheck())
                {
                    var newShape = ArrayUtils.ResizeArray(_itemData.shape, row, col);
                    _itemData.SetShape(newShape);
                }
            }
            
            EditorGUILayout.Space(10);
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
        
            var rows = _itemData.shape.GetLength(0);
            var cols = _itemData.shape.GetLength(1);
            var gridRect = GUILayoutUtility.GetRect(
                MinSize * cols, 
                MinSize * rows, 
                GUILayout.Width(MinSize * cols),
                GUILayout.Height(MinSize * rows));
            var textureDrew = false;
            
            for (var i = 0; i < rows; i++)
            {
                for (var j = 0; j < cols; j++)
                {
                    var x = gridRect.x + j * MinSize;
                    var y = gridRect.y + i * MinSize;
                    var cellRect = new Rect(x, y, MinSize, MinSize);

                    HandleMouseEvent(cellRect, i, j);
                    _itemDataDrawer.DrawShape(cellRect, i, j, ref textureDrew);
                    _itemDataDrawer.DrawGridLines(cellRect);
                }
            }

            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }
        
        private void HandleMouseEvent(Rect cellRect, int i, int j)
        {
            var e = Event.current;
            
            if (e.type is not EventType.MouseDown || !cellRect.Contains(e.mousePosition)) 
                return;
            
            var shapeStatus = e.button == 1 ? 2 : 1;
            _itemData.SetShape(i, j, _itemData.shape[i, j] != shapeStatus ? shapeStatus : 0);
            Repaint();
        }
    }
}
