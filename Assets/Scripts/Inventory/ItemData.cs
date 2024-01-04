using System;
using UnityEngine;
// ReSharper disable InconsistentNaming

namespace Inventory
{
    [CreateAssetMenu]
    public class ItemData : ScriptableObject
    {
        public string dataId = Guid.NewGuid().ToString();
        public new string name;
        public int width = 1;
        public int height = 1;

        public Sprite itemIcon;
        public int[,] shape;
    }
}