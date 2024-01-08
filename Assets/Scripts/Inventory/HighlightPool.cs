using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

namespace Inventory
{
    public class HighlightPool : IObjectPool<GameObject>
    {
        private readonly ObjectPool<GameObject> _pool;
        private readonly Dictionary<GameObject, Image> _highlightImages = new();
        private readonly HashSet<GameObject> _activeHighlights = new();
        private readonly Queue<GameObject> _inactiveHighlights = new();

        public HighlightPool(GameObject highlightPrefab)
        {
            _pool = new ObjectPool<GameObject>(() =>
            {
                var obj = Object.Instantiate(highlightPrefab);
                obj.SetActive(false);
                _highlightImages[obj] = obj.GetComponent<Image>();
                return obj;
            }, instance =>
            {
                instance.SetActive(true);
            }, instance =>
            {
                instance.SetActive(false);
            });
        }

        public Image GetHighlightImage(GameObject gameObject)
        {
            return _highlightImages[gameObject];
        }

        public GameObject Get()
        {
            GameObject highlight;
            if (_inactiveHighlights.Count == 0)
            {
                highlight = _pool.Get();
            }
            else
            {
                highlight = _inactiveHighlights.Dequeue();
                highlight.SetActive(true);
            }
            _activeHighlights.Add(highlight);
            return highlight;
        }

        public PooledObject<GameObject> Get(out GameObject v)
        {
            throw new System.NotImplementedException();
        }

        public void Release(GameObject element)
        {
            element.SetActive(false);
            _inactiveHighlights.Enqueue(element);
        }

        public void Clear()
        {
            foreach (var highlight in _activeHighlights)
            {
                Release(highlight);
            }
            _activeHighlights.Clear();
        }

        public int CountInactive => _inactiveHighlights.Count;
    }
}