using Configuration;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Core.ControlLogic
{
    public interface IControlPanel
    {
        public event Action OnPointerDownEvent;
        public event Action OnPointerUpEvent;
        public event System.Action<Vector2> OnControlDeltaChangedEvent;
        public Vector2 MoveVector { get; }
        void Enable();
        void Disable();
        void Reset();
    }

    public class ControlPanelView : MonoBehaviour, IControlPanel, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        [SerializeField] private Image _sensorImage;
        [SerializeField] private GameConfiguration _gameConfiguration;

        private Vector2 _startTouch;
        private Vector2 _oldTouchPos = Vector2.zero;
        private Vector2 _currentTouchPos;
        private Vector2 _screenSize;
        private float _controlSensitivity;
        private bool _isTouched;
        public Vector2 MoveVector { get; private set; } = Vector2.zero;

        public event Action<Vector2> OnControlDeltaChangedEvent;
        public event Action OnPointerDownEvent;
        public event Action OnPointerUpEvent;

        private void Awake()
        {
            var resolution = Screen.currentResolution;
            _screenSize = new Vector2(resolution.width, resolution.height);
            _controlSensitivity = _gameConfiguration.PlayerConfiguration.ControlSensitivity;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            OnPointerDownEvent?.Invoke();
            _startTouch = eventData.position;
            _currentTouchPos = eventData.position;
            _oldTouchPos = eventData.position;
            _isTouched = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            OnPointerUpEvent?.Invoke();
            _currentTouchPos = eventData.position;
            _oldTouchPos = eventData.position;
            _isTouched = false;
        }

        private void MakeControlDelta()
        {
            _currentTouchPos = Input.mousePosition;
            var controlDelta = _currentTouchPos - _oldTouchPos;
            _oldTouchPos = _currentTouchPos;
            OnControlDeltaChangedEvent?.Invoke(controlDelta);            
        }

        private void MakeMoveVector()
        {
            MoveVector = _currentTouchPos - _startTouch;
            MoveVector = (MoveVector / _screenSize) * _controlSensitivity;
        }

        public void OnDrag(PointerEventData eventData)
        {
            MakeMoveVector();
        }

        private void FixedUpdate()
        {
            if (_isTouched)
            {
                MakeControlDelta();
            }
        }

        public void Reset()
        {
            MoveVector = Vector2.zero;
            _isTouched = false;
        }

        public void Enable()
        {
            _sensorImage.enabled = true;
        }

        public void Disable()
        {
            _sensorImage.enabled = false;
            Reset();
        }
    }
}