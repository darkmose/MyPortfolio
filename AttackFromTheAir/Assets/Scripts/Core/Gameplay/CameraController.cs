using Cinemachine;
using Configuration;
using Core.Level;
using Core.MVP;
using Core.Resourses;
using DG.Tweening;
using UnityEngine;

namespace Core.GameLogic
{
    public enum CameraState
    {
        Movement,
        Targeting
    }

    public class CameraController : MonoBehaviour
    {
        private const int CAMERA_ROTATE_ANGLE = 30;
        private const float MOVE_VECTOR_ACCURACY = 0.05f;
        [SerializeField] private GameConfiguration _gameConfiguration;
        [SerializeField] private Transform _cameraRoot;
        [SerializeField] private Transform _innerCamera;
        private CinemachineVirtualCamera _cam;
        private Tweener _backRotationTweener;
        private Quaternion _tangageQuaternion = Quaternion.identity;
        private Quaternion _yawQuaternion = Quaternion.identity;
        private Quaternion _startRotation = Quaternion.identity;
        private Vector3 _upVector = Vector3.up;
        private Vector2 _currentMoveVector = Vector2.zero;
        private float _tangageAllowedAngle;
        private float _yawAllowedAngle;
        private float _rotateSpeed;
        private float _yawTangageSpeed;
        private bool _yawTangageEnabled;
        private bool _rootRotationEnabled;
        private bool _isTargeting;

        private void Awake()
        {
            TryGetComponent(out _cam);    
            _rotateSpeed = _gameConfiguration.PlayerConfiguration.CameraRotationSpeed;
            _yawTangageSpeed = _gameConfiguration.PlayerConfiguration.YawTangageSpeed;
        }

        public void InitDroneConfiguration(DroneCameraConfiguration droneCameraConfiguration)
        {
            _cam.transform.localEulerAngles = droneCameraConfiguration.CameraInitLocalRotation;
            _tangageAllowedAngle = droneCameraConfiguration.TangageAllowedAngle;
            _yawAllowedAngle = droneCameraConfiguration.YawAllowedAngle;
            _startRotation = _cam.transform.localRotation;
        }

        public void EnableRootRotation()
        {
            _rootRotationEnabled = true;
        }

        public void DisableRootRotation()
        {
            _rootRotationEnabled = false;
        }

        public void PointerDownHandler()
        {
            _backRotationTweener?.Kill();
            _yawTangageEnabled = true;
            DisableRootRotation();
        }

        public void PointerUpHandler()
        {
            _yawTangageEnabled = false;
            _backRotationTweener?.Kill();
            _tangageQuaternion = Quaternion.identity;
            _yawQuaternion = Quaternion.identity;
            _innerCamera.DOLocalRotateQuaternion(_startRotation, 2f);
            EnableRootRotation();
        }

        public void SetMoveVector(Vector2 moveVector)
        {
            Debug.Log($"[CameraController][SetMoveVector] {moveVector}");
            _currentMoveVector = moveVector;

            var tangage = -_currentMoveVector.y * _tangageAllowedAngle;
            float tangageOffset = Mathf.Clamp(tangage, -_tangageAllowedAngle, _tangageAllowedAngle);
            var yaw = -_currentMoveVector.x * _yawAllowedAngle;
            float yawOffset = Mathf.Clamp(yaw, -_yawAllowedAngle, _yawAllowedAngle);
            _tangageQuaternion = Quaternion.Euler(tangageOffset, 0, 0);
            _yawQuaternion = Quaternion.Euler(0, 0, yawOffset);
        }

        private void RotateCameraRoot()
        {
            var rootRotation = _cameraRoot.localRotation * Quaternion.AngleAxis(-CAMERA_ROTATE_ANGLE, _upVector);
            _cameraRoot.localRotation = Quaternion.RotateTowards(_cameraRoot.localRotation, rootRotation, Time.deltaTime * _rotateSpeed);
        }

        private void ApplyYawTangage()
        {
            var cameraRotation = _startRotation * _tangageQuaternion * _yawQuaternion;
            _innerCamera.localRotation = Quaternion.RotateTowards(_innerCamera.localRotation, cameraRotation, Time.deltaTime * _yawTangageSpeed);
        }

        private void Update()
        {
            if (_rootRotationEnabled)
            {
                RotateCameraRoot();
            }
            if (_yawTangageEnabled)
            {
                ApplyYawTangage();
            }
        }
    }
}