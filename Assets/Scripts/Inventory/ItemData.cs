using System;
using UnityEngine;

namespace Inventory
{
    [CreateAssetMenu]
    public class ItemData : ScriptableObject
    {
        public string dataId = Guid.NewGuid().ToString();
        public new string name;
        public int width = 1;
        public int height = 1;
        public int x;
        public int y;
        public Sprite itemIcon;
    }
}