using Core.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Core.UI
{
    public class ZoomScalePanelView : MonoBehaviour, IPointerDownHandler, IDragHandler
    {
        [SerializeField] private RectTransform _handler;
        [SerializeField] private RectTransform _upPoint;
        [SerializeField] private RectTransform _downPoint;
        [SerializeField] private TextMeshProUGUI _value;
        private SimpleEvent<float> _valueChangedEvent = new SimpleEvent<float>();
        private SimpleEvent _pointerDownEvent = new SimpleEvent();
        public SimpleEvent<float> ValueChangedEvent => _valueChangedEvent;
        public SimpleEvent PointerDownEvent => _pointerDownEvent;

        public void OnDrag(PointerEventData eventData)
        {
            MoveHandlerTo(eventData);
        }

        private void MoveHandlerTo(PointerEventData eventData)
        {
            var y = eventData.position.y;
            var upY = _upPoint.position.y;
            var downY = _downPoint.position.y;
            y = Mathf.Clamp(y, downY, upY);
            var position = _handler.position;
            position.y = y;
            _handler.position = position;

            var value = Mathf.InverseLerp(downY, upY, y);
            _valueChangedEvent.Notify(value);
        }

        public void SetHandlePosNormalized(float value)
        {
            var pos = _handler.position;
            var upY = _upPoint.position.y;
            var downY = _downPoint.position.y;
            var posY = Mathf.Lerp(downY, upY, value);
            pos.y = posY;
            _handler.position = pos;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            MoveHandlerTo(eventData);
            _pointerDownEvent.Notify();
        }

        public void SetValue(float value)
        {
            _value.text = value.ToString("x#.#");
        }
    }
}