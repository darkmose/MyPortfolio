using Core.Events;
using Core.UI;
using System;
using UnityEngine;

namespace Core.GameLogic
{
    public class PlayerWeaponContainer : MonoBehaviour, IDisposable
    {
        [SerializeField] private Transform _weaponContainer;

        private CustomProperty<PlayerWeaponType> _currentWeaponType = new CustomProperty<PlayerWeaponType>(default);
        private CustomProperty<PlayerWeaponType> _secondWeaponType = new CustomProperty<PlayerWeaponType>(default);
        private CustomProperty<PlayerExtraWeaponType> _extraWeaponType = new CustomProperty<PlayerExtraWeaponType>(default);
        private FloatProperty _cooldownValue = new FloatProperty();
        private FloatProperty _continuousFireProgress = new FloatProperty();
        private BoolProperty _isWeaponReloading = new BoolProperty();
        private BoolProperty _isContinuousFire = new BoolProperty();
        private BoolProperty _showContinuousFireProgress = new BoolProperty();
        public Transform Container => _weaponContainer;
        public IPropertyReadOnly<float> WeaponCooldownValue => _cooldownValue;
        public IPropertyReadOnly<bool> IsWeaponReloading => _isWeaponReloading;
        public IPropertyReadOnly<PlayerWeaponType> CurrentWeaponType => _currentWeaponType;
        public IPropertyReadOnly<PlayerWeaponType> SecondWeaponType => _secondWeaponType;
        public IPropertyReadOnly<PlayerExtraWeaponType> ExtraWeaponType => _extraWeaponType;
        public IPropertyReadOnly<bool> IsContinuousFire => _isContinuousFire;
        public IPropertyReadOnly<float> ContinuousFireProgress => _continuousFireProgress;
        public IPropertyReadOnly<bool> ShowContinuousFireProgress => _showContinuousFireProgress;

        private PlayerExtraWeapon _extraWeapon;
        private PlayerWeapon _mainWeapon;
        private PlayerWeapon _secondWeapon;
        private bool _usingMainWeapon = true;
        private PlayerWeapon _currentWeapon;

        private void OnEnable()
        {
            EventAggregator.Subscribe<LevelDisposeEvent>(OnLevelDispose);
        }

        private void OnLevelDispose(object sender, LevelDisposeEvent data)
        {
            StopContinousFire();
        }

        private void OnDisable()
        {
            EventAggregator.Unsubscribe<LevelDisposeEvent>(OnLevelDispose);
        }

        public void InitMainWeapon(PlayerWeapon weapon)
        {
            _mainWeapon = weapon;
        }

        public void InitSecondWeapon(PlayerWeapon weapon)
        {
            _secondWeapon = weapon;
        }

        public void InitExtraWeapon(PlayerExtraWeapon weapon)
        {
            _extraWeapon = weapon;
            _extraWeaponType.SetValue(_extraWeapon.PlayerWeaponType, true);
        }

        public void InitContainer()
        {
            _currentWeapon = _mainWeapon;
            _usingMainWeapon = true;

            _currentWeapon.CooldownProgress.RegisterValueChangeListener(OnCooldownValueChanged);
            OnCooldownValueChanged(_currentWeapon.CooldownProgress.Value);
            _currentWeapon.CanShoot.RegisterValueChangeListener(OnCanShootValueChanged);
            OnCanShootValueChanged(_currentWeapon.CanShoot.Value);
            _currentWeapon.ContinuousFireProgress.RegisterValueChangeListener(OnContinuousFireProgressChanged);
            OnContinuousFireProgressChanged(_currentWeapon.ContinuousFireProgress.Value);
            _currentWeapon.ShowContinuousFireProgress.RegisterValueChangeListener(OnContinuousFireProgressVisibilityChanged);
            OnContinuousFireProgressVisibilityChanged(_currentWeapon.ShowContinuousFireProgress.Value);
            _currentWeaponType.SetValue(_currentWeapon.PlayerWeaponType, true);
            _secondWeaponType.SetValue(_secondWeapon.PlayerWeaponType, true);
        }

        private void OnContinuousFireProgressVisibilityChanged(bool isVisible)
        {
            _showContinuousFireProgress.SetValue(isVisible);
        }

        private void OnContinuousFireProgressChanged(float progress)
        {
            _continuousFireProgress.SetValue(progress, false);
        }

        private void OnCanShootValueChanged(bool canShoot)
        {
            _isWeaponReloading.SetValue(!canShoot);
        }

        private void OnCooldownValueChanged(float cooldownValue)
        {
            _cooldownValue.SetValue(1f - cooldownValue, false);
        }

        public void SwitchWeapon()
        {
            _currentWeapon?.CooldownProgress.UnregisterValueChangeListener(OnCooldownValueChanged);
            _currentWeapon?.CanShoot.UnregisterValueChangeListener(OnCanShootValueChanged);
            _currentWeapon?.ContinuousFireProgress.UnregisterValueChangeListener(OnContinuousFireProgressChanged);
            _currentWeapon?.ShowContinuousFireProgress.UnregisterValueChangeListener(OnContinuousFireProgressVisibilityChanged);

            if (_usingMainWeapon) 
            {
                _currentWeapon = _secondWeapon;
                _secondWeaponType.SetValue(_mainWeapon.PlayerWeaponType, false);
            }
            else
            {
                _currentWeapon = _mainWeapon;
                _secondWeaponType.SetValue(_secondWeapon.PlayerWeaponType, false);
            }
            _currentWeaponType.SetValue(_currentWeapon.PlayerWeaponType, false);

            _currentWeapon.CooldownProgress.RegisterValueChangeListener(OnCooldownValueChanged);
            OnCooldownValueChanged(_currentWeapon.CooldownProgress.Value);
            _currentWeapon.CanShoot.RegisterValueChangeListener(OnCanShootValueChanged);
            OnCanShootValueChanged(_currentWeapon.CanShoot.Value);
            _currentWeapon.ContinuousFireProgress.RegisterValueChangeListener(OnContinuousFireProgressChanged);
            OnContinuousFireProgressChanged(_currentWeapon.ContinuousFireProgress.Value);
            _currentWeapon.ShowContinuousFireProgress.RegisterValueChangeListener(OnContinuousFireProgressVisibilityChanged);
            OnContinuousFireProgressVisibilityChanged(_currentWeapon.ShowContinuousFireProgress.Value);

            _usingMainWeapon = !_usingMainWeapon;
        }

        public void Fire()
        {
            _currentWeapon.Fire();
        }

        public void StartContinuousFire()
        {
            _isContinuousFire.SetValue(true);
            _currentWeapon.StartContinuosFire();
        }

        public void StopContinousFire()
        {
            _isContinuousFire.SetValue(false);
            _currentWeapon?.StopContinuosFire();
        }

        public void FireExtraWeapon()
        {
            _extraWeapon.Fire();
        }

        public void Dispose()
        {
            _mainWeapon.CooldownProgress.RemoveAllListeners();
            _secondWeapon.CooldownProgress.RemoveAllListeners();
            _extraWeapon.CooldownProgress.RemoveAllListeners();
            _mainWeapon.CanShoot.RemoveAllListeners();
            _secondWeapon.CanShoot.RemoveAllListeners();
            _extraWeapon.CanShoot.RemoveAllListeners();  
            _mainWeapon.ContinuousFireProgress.RemoveAllListeners();
            _secondWeapon.ContinuousFireProgress.RemoveAllListeners();           
            _mainWeapon.ShowContinuousFireProgress.RemoveAllListeners();
            _secondWeapon.ShowContinuousFireProgress.RemoveAllListeners();

            Destroy(_mainWeapon.gameObject);
            Destroy(_secondWeapon.gameObject);
            Destroy(_extraWeapon.gameObject);

            _mainWeapon = null;
            _secondWeapon = null;
            _extraWeapon = null;
        }
    }
}