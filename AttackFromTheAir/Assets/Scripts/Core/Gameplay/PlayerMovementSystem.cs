using Configuration;
using Core.ControlLogic;
using Core.DISimple;
using Core.MVP;
using Core.Tools;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace Core.GameLogic
{
    public class PlayerMovementSystem : MonoBehaviour
    {
        private const float INERTIA_DURATION = .1f;
        [SerializeField] private Transform _playerRoot;
        [SerializeField] private GameConfiguration _gameConfiguration;
        [SerializeField] private CameraController _cameraController;
        private Vector3 _cameraLookVector = Vector3.zero;
        private IControlPanel _controlPanel;
        private Vector2 _moveVector;
        private float _moveSpeedMin;
        private float _moveSpeedMax;
        private float _moveSpeed;
        private float _lastZoom;
        private bool _isMoving;
        private bool _isRotating;
        private Camera _camera;
        private Tweener _moveInertia;
        private IGameScreenPresenter _gameScreenPresenter;

        private void Awake()
        {
            _camera = Camera.main;
            _moveSpeedMin = _gameConfiguration.PlayerConfiguration.CameraMoveSpeedMin;
            _moveSpeedMax = _gameConfiguration.PlayerConfiguration.CameraMoveSpeedMax;
            var defaultZoom = _gameConfiguration.PlayerConfiguration.DefaultNormalizedZoomScale;

            UpdateMoveSpeedByZoom(defaultZoom);

            var di = ServiceLocator.Resolve<DiContainer>();
            _gameScreenPresenter = di.Resolve<IGameScreenPresenter>();
            _gameScreenPresenter.ZoomNormalizedValueChangedEvent.AddListener(OnZoomChanged);
        }

        private void OnZoomChanged(float zoomValue)
        {
            UpdateMoveSpeedByZoom(zoomValue);
        }

        private void UpdateMoveSpeedByZoom(float zoom)
        {
            _moveSpeed = Mathf.Lerp(_moveSpeedMin, _moveSpeedMax, zoom);
            _lastZoom = zoom;
        }

        public void InitControlPanel(IControlPanel controlPanel)
        {
            _controlPanel = controlPanel;
            _controlPanel.OnControlDeltaChangedEvent += ControlDeltaChangedHandler;
            _controlPanel.OnPointerDownEvent += PointerDownHandler;
            _controlPanel.OnPointerUpEvent += PointerUpHandler;
        }

        public void PointerUpHandler()
        {
            _moveInertia = DOTween.To(() => _moveSpeed, value => _moveSpeed = value, 0f, INERTIA_DURATION).OnComplete(() => 
            {
                _moveVector = Vector3.zero;
                _controlPanel.Reset();
                _isMoving = false;
                UpdateMoveSpeedByZoom(_lastZoom);
            }).SetEase(Ease.InOutCirc);
            _cameraController.PointerUpHandler();
        }

        public void PointerDownHandler()
        {
            _moveInertia?.Complete();
            _moveInertia?.Kill();
            _cameraLookVector = _camera.transform.localEulerAngles;
            _isMoving = true;
            _cameraController.PointerDownHandler();
        }

        public void ControlDeltaChangedHandler(Vector2 delta)
        {
            _moveVector = _controlPanel.MoveVector;
            _cameraController.SetMoveVector(_moveVector);
        }

        public void SetMoveVector(Vector2 moveVector)
        {
            _moveVector = moveVector;
            _cameraController.SetMoveVector(moveVector);
        }

        private void Move()
        {
            var position = _playerRoot.position;
            var convertedMoveVector = CameraVectorConversionService.ConvertVector(_moveVector, _cameraLookVector);
            position.x += convertedMoveVector.x;
            position.z += convertedMoveVector.y;
            _playerRoot.position = Vector3.MoveTowards(_playerRoot.position, position, Time.deltaTime * _moveSpeedMin); 
        }

        private void Update()
        {
            if (_isMoving) 
            {
                Move();
            }
        }

        private void OnDestroy()
        {
            _gameScreenPresenter.ZoomNormalizedValueChangedEvent.RemoveListener(OnZoomChanged);
        }
    }
}