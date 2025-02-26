using Core.MVP;
using Core.Utilities;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Core.LobbyBase
{
    public class PlayerBaseControlPanel : MonoBehaviour, IPointerClickHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        private bool _enabled = true;
        public SimpleEvent<Vector2> OnDragEvent { get; } = new SimpleEvent<Vector2>();
        public SimpleEvent<Vector2> OnPointerClickEvent { get; } = new SimpleEvent<Vector2>();
        public SimpleEvent<Vector2> OnDragBeginEvent { get; } = new SimpleEvent<Vector2>();
        public SimpleEvent<Vector2> OnDragEndEvent { get; } = new SimpleEvent<Vector2>();

        public void SetEnabled(bool enabled)
        {
            _enabled = enabled;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (_enabled)
                OnDragBeginEvent.Notify(eventData.position);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (_enabled)
                OnDragEvent.Notify(-eventData.delta);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (_enabled)
                OnDragEndEvent.Notify(eventData.position);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_enabled)
                OnPointerClickEvent.Notify(eventData.position);
        }
    }

}