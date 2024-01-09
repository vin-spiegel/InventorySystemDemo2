using System;
using Inventory;
using UnityEngine;
using UnityEngine.EventSystems;
using VContainer;

namespace Components.Dragging
{
    public class DragItem<T> : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public event Action<PointerEventData> OnEndDragEvent;
        public event Action<PointerEventData> OnDragEvent;
        public event Action<PointerEventData> OnBeginDragEvent;
        
        private Vector2 _startPosition;
        
        void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
        {
            _startPosition = transform.position;
            OnBeginDragEvent?.Invoke(eventData);
        }

        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            transform.position = eventData.position;
            OnDragEvent?.Invoke(eventData);
        }

        void IEndDragHandler.OnEndDrag(PointerEventData eventData)
        {
            OnEndDragEvent?.Invoke(eventData);
        }
    }
}