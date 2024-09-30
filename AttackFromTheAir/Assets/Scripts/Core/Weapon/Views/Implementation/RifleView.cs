using Configuration;
using Core.GameLogic;
using Core.Tools;
using DG.Tweening;
using UnityEngine;

namespace Core.Weapon
{
    public class RifleView : BaseBulletWeaponView
    {
        [SerializeField] private ParticleSystem _muzzleFlare;
        [SerializeField] private WeaponProjectileConfiguration _weaponProjectileConfiguration;
        [SerializeField] private ProjectileView _projectilePrefab;
        [SerializeField] private int _projectilePoolCapacity;
        private WeaponProjectilesPool _weaponProjectilesPool;
        private Tweener _bulletTweener;
        public override BulletsWeaponType BulletsWeaponType => BulletsWeaponType.Rifle;

        private void OnEnable()
        {
            _weaponProjectilesPool = new WeaponProjectilesPool(_projectilePrefab, _projectilePoolCapacity);
        }

        public override void OnFire(IDamagableObject damagableObject)
        {
            _muzzleFlare.Play();
            var projectile = _weaponProjectilesPool.GetProjectile();
            projectile.InitFraction(WeaponFraction);
            projectile.InitDamage(Damage);
            var distance = Vector3.Distance(damagableObject.View.transform.position,transform.position);
            var speed = _weaponProjectileConfiguration.SmallBulletSpeed;
            var duration = distance / speed;
            projectile.transform.position = _bulletStartPoint.position;
            projectile.transform.rotation = _bulletStartPoint.rotation;
            var targetPos = damagableObject.View.transform.position + Vector3.up;
            var flightParabolaVertex = VectorTools.GetParabolaVertex(_bulletStartPoint.position, targetPos, 24f);
            var flightPoints = VectorTools.GetParabolaPoints(_bulletStartPoint.position, flightParabolaVertex, targetPos, 5);

            _bulletTweener = projectile.transform.DOPath(flightPoints, duration)
                .SetEase(Ease.Linear)
                .SetLookAt(0f, false)
                .OnComplete(() => OnProjectileReachedDestination(projectile));
        }

        private void OnProjectileReachedDestination(ProjectileView projectile)
        {
            if (!_weaponProjectilesPool.IsInThePool(projectile))
            {
                _weaponProjectilesPool.ReturnProjectile(projectile);
            }
        }

        public override void OnStopFire()
        {
        }

        private void OnDisable()
        {
            _weaponProjectilesPool.Dispose();
        }
    }
}