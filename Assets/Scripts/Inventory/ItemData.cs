using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

// ReSharper disable InconsistentNaming
// ReSharper disable ParameterHidesMember

namespace Inventory
{
    [CreateAssetMenu]
    public class ItemData : ScriptableObject, ISerializationCallbackReceiver, IDataChangeNotifier
    {
        [Tooltip("UUID를 새로 만드려면 필드를 지워주세요.")]
        public string dataId;
        public new string name;
        [Multiline] public string description;
        public int width = 1;
        public int height = 1;
        [FormerlySerializedAs("itemIcon")] public Sprite sprite;
        public int[,] shape = new int[5, 5];
        [SerializeField, TextArea] public string shapeJson;

        // todo: 아이템 데이터 캐시 따로 분리해서 만들기
        private static Dictionary<string, ItemData> itemLookupCache;

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            if (string.IsNullOrWhiteSpace(dataId))
            {
                dataId = Guid.NewGuid().ToString();
            }
        }
        
        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            if (!string.IsNullOrEmpty(shapeJson))
            {
                shape = JsonConvert.DeserializeObject<int[,]>(shapeJson);
            }
        }

        internal void OnItemDataChanged()
        {
            ItemDataChanged?.Invoke(this);
        }

        public event Action<ItemData> ItemDataChanged;
    }
}