using Core.Utilities;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Core.UI
{
    public class TwoStateButton : Selectable, IPointerDownHandler, IPointerUpHandler
    {
        public SimpleEvent PointerDownEvent { get; } = new SimpleEvent();
        public SimpleEvent PointerUpEvent { get; } = new SimpleEvent();

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            PointerDownEvent.Notify();
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            PointerUpEvent.Notify();
        }
    }
}