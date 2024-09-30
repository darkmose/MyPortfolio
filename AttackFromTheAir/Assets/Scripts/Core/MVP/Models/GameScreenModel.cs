using Configuration;
using Core.GameLogic;
using Core.Resourses;
using Core.UI;
using UnityEngine;

namespace Core.MVP
{
    public class GameScreenModel : IModel
    {
        private GameConfiguration _gameConfiguration;
        private PlayerWeaponSpriteProvider _weaponSpriteProvider;
        private FloatProperty _zoomScaleValue = new FloatProperty();
        private CustomProperty<Sprite> _currentWeaponIcon = new CustomProperty<Sprite>(default);
        private CustomProperty<Sprite> _secondWeaponIcon = new CustomProperty<Sprite>(default);
        private CustomProperty<Sprite> _extraWeaponIcon = new CustomProperty<Sprite>(default);
        public IPropertyReadOnly<bool> IsWeaponReloading { get; }
        public IPropertyReadOnly<bool> ShowContinuousFireProgress { get; }
        public IPropertyReadOnly<float> WeaponReloadValue { get; }
        public IPropertyReadOnly<float> ContinuousFireProgress { get; }
        public IPropertyReadOnly<float> ZoomScaleValue => _zoomScaleValue;
        public IPropertyReadOnly<Sprite> CurrentWeaponIcon => _currentWeaponIcon;
        public IPropertyReadOnly<Sprite> SecondWeaponIcon => _secondWeaponIcon;
        public IPropertyReadOnly<Sprite> ExtraWeaponIcon => _extraWeaponIcon;

        public GameScreenModel(GameConfiguration gameConfiguration, PlayerWeaponSpriteProvider playerWeaponSpriteProvider, PlayerData player)
        {
            _gameConfiguration = gameConfiguration;
            _weaponSpriteProvider = playerWeaponSpriteProvider;
            player.WeaponContainer.CurrentWeaponType.RegisterValueChangeListener(OnCurrentWeaponChanged);
            player.WeaponContainer.SecondWeaponType.RegisterValueChangeListener(OnSecondWeaponChanged);
            player.WeaponContainer.ExtraWeaponType.RegisterValueChangeListener(OnExtraWeaponChanged);
            WeaponReloadValue = player.WeaponContainer.WeaponCooldownValue;
            IsWeaponReloading = player.WeaponContainer.IsWeaponReloading;
            ContinuousFireProgress = player.WeaponContainer.ContinuousFireProgress;
            ShowContinuousFireProgress = player.WeaponContainer.ShowContinuousFireProgress;
        }

        private void OnExtraWeaponChanged(PlayerExtraWeaponType type)
        {
            var weaponIcon = _weaponSpriteProvider.ProvideByType(type);
            _extraWeaponIcon.SetValue(weaponIcon, true);
        }

        private void OnSecondWeaponChanged(PlayerWeaponType type)
        {
            var weaponIcon = _weaponSpriteProvider.ProvideByType(type);
            _secondWeaponIcon.SetValue(weaponIcon, true);
        }

        private void OnCurrentWeaponChanged(PlayerWeaponType type)
        {
            var weaponIcon = _weaponSpriteProvider.ProvideByType(type);
            _currentWeaponIcon.SetValue(weaponIcon, true);
        }

        public void SetNormalizedZoom(float zoom)
        {
            var min = _gameConfiguration.PlayerConfiguration.CameraZoomScaleMin;
            var max = _gameConfiguration.PlayerConfiguration.CameraZoomScaleMax;

            var scaleValue = Mathf.Lerp(min, max, zoom);
            _zoomScaleValue.SetValue(scaleValue, true);
        }

        public float SetDefaultZoom()
        {
            var defaultValue = _gameConfiguration.PlayerConfiguration.DefaultNormalizedZoomScale;
            SetNormalizedZoom(defaultValue);
            return defaultValue;
        }
    }
}