using Core.GameLogic;
using Core.Tools;
using Core.Utilities;
using DG.Tweening;

namespace Core.Weapon
{
    public abstract class BaseWeapon : IWeapon
    {
        private Tweener _fireTweener;
        private Tweener _cooldownTweener;
        private WeaponConfig _config;
        private IDamagableObject _damagableObject;
        private SimpleEvent<IDamagableObject> _fireEvent = new SimpleEvent<IDamagableObject>();
        private SimpleEvent _fireStopEvent = new SimpleEvent(); 
        private SimpleEvent<float> _aimEvent = new SimpleEvent<float>(); 
        private float _fireDelay;
        private float _cooldownDuration;
        private int _shotsCounter = 0;
        private int _shotsBeforeCooldown = 0;
        private bool _isCooldown;
        public abstract WeaponProjectileType WeaponProjectileType { get; }
        public SimpleEvent<IDamagableObject> FireEvent => _fireEvent;
        public SimpleEvent FireStopEvent => _fireStopEvent;
        public WeaponConfig Config => _config;
        public BaseWeaponView WeaponView { get; set; }
        public SimpleEvent<float> AimEvent => _aimEvent;

        protected BaseWeapon(WeaponConfig weaponConfig)
        {
            _config = weaponConfig;
            if (_config.Firerate == 0f)
            {
                _config.Firerate = 1f;
            }
            _fireDelay = 1f / _config.Firerate;
            _shotsBeforeCooldown = weaponConfig.ShotsBeforeCooldown;
            _cooldownDuration = weaponConfig.CooldownDuration;
        }

        public virtual void StartFire(IDamagableObject damagableObject)
        {
            _damagableObject = damagableObject;
            StartCooldown();
        }

        private void OnFireTimer()
        {
            if (!_isCooldown)
            {
                Fire(_damagableObject);
                _shotsCounter++;
                if (_shotsCounter >= _shotsBeforeCooldown)
                {
                    _shotsCounter = 0;
                    StartCooldown();
                }
                _fireTweener?.Kill();
                _fireTweener = Timer.SetTimer(_fireDelay, OnFireTimer);
            }
        }

        private void StartCooldown()
        {
            _isCooldown = true;
            _aimEvent.Notify(_cooldownDuration);
            _cooldownTweener?.Kill();
            _cooldownTweener = Timer.SetTimer(_cooldownDuration, () =>
            {
                _isCooldown = false;
                OnFireTimer();
            });
        }

        private void Fire(IDamagableObject damagableObject)
        {
            FireEvent.Notify(damagableObject);
        }

        public virtual void StopFire()
        {
            _fireTweener?.Kill();
            _cooldownTweener?.Kill();
            FireStopEvent.Notify();
        }
    }
}