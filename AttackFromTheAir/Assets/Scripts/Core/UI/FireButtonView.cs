using UnityEngine;
using Core.Utilities;
using UnityEngine.EventSystems;
using Core.ControlLogic;
using Configuration;

namespace Core.MVP
{
    public class FireButtonView : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private GameConfiguration _gameConfiguration;
        [SerializeField] private RectTransform _joystickRect;
        private float _controlSensitivity;
        private Vector2 _screenSize;
        private bool _isTouched;
        private Vector2 _startPos;
        private Vector2 _endPos;
        public SimpleEvent ClickEvent { get; } = new SimpleEvent();
        public SimpleEvent PointerDownEvent { get; } = new SimpleEvent();
        public SimpleEvent PointerUpEvent { get; } = new SimpleEvent();
        public SimpleEvent<Vector2> JoystickMoveEvent { get; } = new SimpleEvent<Vector2>();
        public bool IsCalculateVectorMode { get; set; }

        private void Awake()
        {
            _screenSize = _joystickRect.sizeDelta;
            _controlSensitivity = _gameConfiguration.PlayerConfiguration.ControlSensitivity;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            ClickEvent.Notify();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _startPos = eventData.position;
            PointerDownEvent.Notify();
            _isTouched = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _isTouched = false;
            PointerUpEvent.Notify();
        }

        private void Update()
        {
            if (_isTouched && IsCalculateVectorMode)
            {
                _endPos = Input.mousePosition;
                var diff = _endPos - _startPos;
                diff /= _screenSize * _controlSensitivity;
                JoystickMoveEvent.Notify(diff);
            }
        }
    }
}