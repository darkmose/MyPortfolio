using Core.MVP;
using Core.Tools;
using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Core.GameLogic
{
    public class PlayerDistanceChecker
    {
        private const float PRE_NOISE_EFFECT_DISTANCE_PERCENTAGE = 0.8f;
        private const float NOISE_EFFECT_DISTANCE = 30f;
        private const float ERROR_EFFECT_DISTANCE = 20f;
        private IGameScreenPresenter _gameScreenPresenter;
        private PlayerData _playerData;
        private MonoUpdater _updater;
        private CameraFXController _cameraFXController;
        private Vector3 _leftBottomPoint;
        private Vector3 _rightTopPoint;
        private bool _errorEnabled;
        private bool _noiseEnabled;

        public PlayerDistanceChecker(IGameScreenPresenter gameScreenPresenter, PlayerData playerData, MonoUpdater updater, CameraFXController cameraFXController)
        {
            _gameScreenPresenter = gameScreenPresenter;
            _playerData = playerData;
            _updater = updater;
            _cameraFXController = cameraFXController;
        }

        public void InitLevelBounds(Vector3 leftBottomPoint, Vector3 rightTopPoint)
        {
            _leftBottomPoint = leftBottomPoint;
            _rightTopPoint = rightTopPoint;
        }

        public void StartCheck()
        {
            _updater.AddMonoUpdateListener(OnMonoUpdate);
        }

        private void OnMonoUpdate()
        {
            Vector3 playerPosition = _playerData.transform.position;

            float distanceToLeft = playerPosition.x - _leftBottomPoint.x;
            float distanceToRight = _rightTopPoint.x - playerPosition.x;
            float distanceToBottom = playerPosition.z - _leftBottomPoint.z;
            float distanceToTop = _rightTopPoint.z - playerPosition.z;

            var minDistance = Mathf.Min(distanceToLeft, distanceToRight, distanceToBottom, distanceToTop);

            var isNoiseEnabled = minDistance < NOISE_EFFECT_DISTANCE;
            var isErrorEnabled = minDistance < ERROR_EFFECT_DISTANCE;
            EnableNoise(isNoiseEnabled, minDistance);
            EnableError(isErrorEnabled);
            if (isErrorEnabled)
            {
                ClampPlayerPos();
            }
        }

        private void EnableNoise(bool isEnabled, float playerDistance)
        {
            if (isEnabled)
            {
                var power = 1f - (playerDistance / NOISE_EFFECT_DISTANCE);
                _cameraFXController.SetOutMapInterference(power);
            }

            if (_noiseEnabled == isEnabled)
            {
                return;
            }
            _noiseEnabled = isEnabled;

            _cameraFXController.SetActiveOutMapInterferences(isEnabled);
        }

        private void EnableError(bool isEnabled)
        {
            if (isEnabled == _errorEnabled)
            {
                return;
            }
            _errorEnabled = isEnabled;

            _gameScreenPresenter.SetActiveConnectionErrorPanel(isEnabled);
        }

        public void StopCheck()
        {
            _updater.RemoveUpdateListener(OnMonoUpdate);
        }

        void ClampPlayerPos()
        {
            Vector3 playerPosition = _playerData.transform.position;

            playerPosition.x = Mathf.Clamp(playerPosition.x, _leftBottomPoint.x, _rightTopPoint.x);
            playerPosition.z = Mathf.Clamp(playerPosition.z, _leftBottomPoint.z, _rightTopPoint.z);

            _playerData.transform.position = playerPosition;
        }
    }
}