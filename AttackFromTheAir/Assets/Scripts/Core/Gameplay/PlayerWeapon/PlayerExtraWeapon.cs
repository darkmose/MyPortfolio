using Core.Tools;
using Core.UI;
using Core.Weapon;
using DG.Tweening;
using UnityEngine;

namespace Core.GameLogic
{
    public enum PlayerExtraWeaponType
    {
        AirStrike
    }

    public abstract class PlayerExtraWeapon : MonoBehaviour
    {
        private const float COOLDOWN_DURATION_CONSTANT = 7f;
        [SerializeField] protected PlayerExtraWeaponType _playerWeaponType;
        [SerializeField] private ProjectileView _projectileViewPrefab;
        [SerializeField] private int _projectilesPoolCapacity;
        private WeaponProjectilesPool _projectilesPool;
        private IPlayerWeaponDecorator _weaponConfigDecorator;
        private FloatProperty _cooldownProgress = new FloatProperty();
        private BoolProperty _canShoot = new BoolProperty(false);
        private Tweener _cooldownTweener;
        protected PlayerWeaponConfig PlayerWeaponConfig => _weaponConfigDecorator.GetWeaponConfig();
        protected WeaponProjectilesPool WeaponProjectilesPool => _projectilesPool;
        public PlayerExtraWeaponType PlayerWeaponType => _playerWeaponType;
        public IPropertyReadOnly<float> CooldownProgress => _cooldownProgress;
        public IPropertyReadOnly<bool> CanShoot => _canShoot;

        public void Fire()
        {
            if (_canShoot.Value)
            {
                FireInner();
                StartCooldown();
            }
        }

        public void InitWeaponProjectilePool()
        {
            var initDamage = _projectileViewPrefab.Damage;
            _projectileViewPrefab.InitDamage(PlayerWeaponConfig.Damage);
            _projectilesPool = new WeaponProjectilesPool(_projectileViewPrefab, _projectilesPoolCapacity);
            _projectileViewPrefab.InitDamage(initDamage);
        }

        public void InitWeaponConfigDecorator(IPlayerWeaponDecorator playerWeaponDecorator)
        {
            _weaponConfigDecorator = playerWeaponDecorator;
            _canShoot.SetValue(true);
        }

        protected abstract void FireInner();

        private void StartCooldown()
        {
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
            _canShoot.SetValue(true);
        }
    }
}