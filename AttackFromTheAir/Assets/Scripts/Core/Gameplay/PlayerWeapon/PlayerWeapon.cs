using Core.Events;
using Core.Tools;
using Core.UI;
using Core.Weapon;
using DG.Tweening;
using System;
using UnityEngine;

namespace Core.GameLogic
{
    public enum PlayerWeaponType
    {
        Bullets,
        Rocket,
        GuidedRocket,
        FirstPersonControlRocket,
        ThrowOffBomb
    }

    public abstract class PlayerWeapon : MonoBehaviour
    {
        private const float COOLDOWN_DURATION_CONSTANT = 25f;
        [SerializeField] protected PlayerWeaponType _playerWeaponType;
        [SerializeField] private ProjectileView _projectileViewPrefab;
        [SerializeField] private int _projectilesPoolCapacity;
        private FloatProperty _cooldownProgress = new FloatProperty();
        private FloatProperty _continuousFireProgress = new FloatProperty();
        private BoolProperty _canShoot = new BoolProperty(true);
        private BoolProperty _showContinuousFireProgress = new BoolProperty(false);
        private Tweener _cooldownTweener;
        private Tweener _continuousFireTweener;
        private float _continuousFireDelay;
        private int _continuousFiresBeforeReload;
        private int _continuousFireCounter;
        private bool _isCooldown;
        private IPlayerWeaponDecorator _weaponConfigDecorator;
        private WeaponProjectilesPool _projectilesPool;
        public PlayerWeaponType PlayerWeaponType => _playerWeaponType;
        public IPropertyReadOnly<float> CooldownProgress => _cooldownProgress;
        public IPropertyReadOnly<bool> CanShoot => _canShoot;
        public IPropertyReadOnly<bool> ShowContinuousFireProgress => _showContinuousFireProgress;
        public IPropertyReadOnly<float> ContinuousFireProgress => _continuousFireProgress;
        protected PlayerWeaponConfig PlayerWeaponConfig => _weaponConfigDecorator.GetWeaponConfig();
        protected WeaponProjectilesPool WeaponProjectilesPool => _projectilesPool;

        public void Fire()
        {
            if (_canShoot.Value && !PlayerWeaponConfig.IsContinuousFire)
            {
                FireInner();
                StartCooldown();
            }
        }

        public void InitWeaponProjectilePool()
        {
            var weaponDamage = PlayerWeaponConfig.Damage;
            _projectilesPool = new WeaponProjectilesPool(_projectileViewPrefab, _projectilesPoolCapacity, 
            (projectile) => 
            {
                projectile.InitDamage(weaponDamage);
                projectile.InitFraction(Units.UnitFraction.Player);
            });
            
            EventAggregator.Subscribe<LevelDisposeEvent>(OnLevelDispose);
        }

        private void OnLevelDispose(object sender, LevelDisposeEvent data)
        {
            EventAggregator.Unsubscribe<LevelDisposeEvent>(OnLevelDispose);
            _projectilesPool.Dispose();
        }

        public void StartContinuosFire()
        {
            if (_canShoot.Value && PlayerWeaponConfig.IsContinuousFire) 
            {
                _showContinuousFireProgress.SetValue(true);

                var firerate = PlayerWeaponConfig.ContinuousFirerate;
                if (firerate == 0)
                {
                    firerate = 1;
                }
                _continuousFiresBeforeReload = PlayerWeaponConfig.ContinuousFiresBeforeReload;
                _continuousFireDelay = 1f / firerate;
                FireInner();
                _continuousFireCounter++;
                MakeContinuousFireProgress();
                if (_continuousFireCounter >= _continuousFiresBeforeReload)
                {
                    _continuousFireCounter = 0;
                    StartCooldown();
                }
                else
                {
                    ContinuousFire();
                }
            }
        }

        public void StopContinuosFire()
        {
            if (PlayerWeaponConfig.IsContinuousFire)
            {
                _continuousFireTweener?.Kill();
            }
        }

        private void ContinuousFire()
        {
            _continuousFireTweener?.Kill();
            _continuousFireTweener = Timer.SetTimer(_continuousFireDelay, () => 
            {
                FireInner();
                _continuousFireCounter++;
                MakeContinuousFireProgress();
                if (_continuousFireCounter >= _continuousFiresBeforeReload)
                {
                    _continuousFireCounter = 0;
                    StopContinuosFire();
                    StartCooldown();
                }
                else
                {
                    ContinuousFire();
                }
            });
        }

        private void MakeContinuousFireProgress()
        {
            var value = (float)_continuousFireCounter / (float)_continuousFiresBeforeReload;
            value = 1f - value;
            _continuousFireProgress.SetValue(value, false);
        }

        public void InitWeaponConfigDecorator(IPlayerWeaponDecorator playerWeaponDecorator)
        {
            _weaponConfigDecorator = playerWeaponDecorator;
        }

        protected abstract void FireInner();

        private void StartCooldown()
        {
            if (_isCooldown) return;
            _showContinuousFireProgress.SetValue(false);

            _isCooldown = true;
            _canShoot.SetValue(false);
            _cooldownTweener?.Kill();
            var cooldownDuration = COOLDOWN_DURATION_CONSTANT / PlayerWeaponConfig.ReloadSpeed;
            _cooldownTweener = Timer.SetTimer(cooldownDuration, OnCooldownOver, OnCooldownUpdate);
        }

        private void OnCooldownUpdate(float cooldownValueNormalized)
        {
            _cooldownProgress.SetValue(cooldownValueNormalized, false);
        }

        private void OnCooldownOver()
        {
            _isCooldown = false;
            _canShoot.SetValue(true);
        }
    }
}