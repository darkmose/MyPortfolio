using Configuration;
using Core.Events;
using Core.Tools;
using System;

namespace Core.GameLogic
{
    public class PlayerWeaponCameraShakeHelper
    {
        private GameConfiguration _gameConfiguration;
        private CameraFXController _cameraFXController;

        public PlayerWeaponCameraShakeHelper(GameConfiguration gameConfiguration, CameraFXController cameraFXController)
        {
            _gameConfiguration = gameConfiguration;
            _cameraFXController = cameraFXController;
            EventAggregator.Subscribe<PlayerWeaponFireEvent>(OnPlayerWeaponFire);
            EventAggregator.Subscribe<PlayerExtraWeaponFireEvent>(OnPlayerExtraWeaponFire);
        }

        private void OnPlayerExtraWeaponFire(object sender, PlayerExtraWeaponFireEvent data)
        {
            var shakeMode = _gameConfiguration.PlayerConfiguration.GetCameraShakeMode(data.PlayerWeapon.PlayerWeaponType);
            _cameraFXController.ShakeCamera(shakeMode);
        }

        private void OnPlayerWeaponFire(object sender, PlayerWeaponFireEvent data)
        {
            var shakeMode = _gameConfiguration.PlayerConfiguration.GetCameraShakeMode(data.PlayerWeapon.PlayerWeaponType);
            _cameraFXController.ShakeCamera(shakeMode);
        }
    }
}