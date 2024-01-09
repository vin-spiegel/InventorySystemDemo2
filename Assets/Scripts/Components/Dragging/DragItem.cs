using Inventory;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Components.Dragging
{
    public class DragItem<T> : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        private Canvas _canvas;

        private void Awake()
        {
            Debug.Log("DragItem Awake");
            // _canvas = GetComponentInParent<Canvas>();
        }
        
        void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
        {
            Debug.Log("OnBeginDrag");
        }

        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            Debug.Log("OnDrag");
        }

        void IEndDragHandler.OnEndDrag(PointerEventData eventData)
        {
            Debug.Log("OnEndDrag");
        }
    }
}