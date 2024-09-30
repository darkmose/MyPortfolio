using Core.Resourses;
using Core.UI;
using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

namespace Core.GameLogic
{
    public class DroneContainerView : MonoBehaviour
    {
        [SerializeField] private DroneProvider _droneProvider;
        [SerializeField] private Transform _droneRoot;
        [SerializeField] private Camera _renderTextureCamera;
        [SerializeField] private Transform _cameraRoot;
        private Tweener _cameraRotationTween;
        private DroneView _currentDroneView;

        public void InitDrone(PlayerDroneType playerDroneType)
        {
            ClearDrone();
            var drone = _droneProvider.ProvideByType(playerDroneType);
            var prefab = drone.DroneViewPrefab;
            var instance = Instantiate(prefab, _droneRoot);
            _currentDroneView = instance;
        }

        public void InitDroneWeapon(PlayerWeaponType playerWeaponType)
        {
            _currentDroneView?.InitWeapon(playerWeaponType);
        }

        private void ClearDrone()
        {
            _droneRoot.ClearAllChild();
        }

        public void Enable()
        {
            _renderTextureCamera.enabled = true;
            StartCameraRotation();
        }

        private void StartCameraRotation()
        {
            _cameraRotationTween?.Kill();
            var rotation = Vector3.up * 360;
            _cameraRotationTween = _cameraRoot.DOLocalRotate(rotation, 5f, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);
        }

        public void Disable() 
        {
            ClearDrone();
            _currentDroneView = null;
            _renderTextureCamera.enabled = false;
            _cameraRotationTween?.Kill();
            _cameraRoot.localEulerAngles = Vector3.zero;
        }
    }
}