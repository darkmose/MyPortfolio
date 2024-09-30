using Configuration;
using Core.GameLogic;
using Core.Tools;
using DG.Tweening;
using UnityEngine;

namespace Core.Weapon
{
    public class TankTurretView : BaseExploProjectileWeaponView
    {
        [SerializeField] private Transform _projectileStartPoint;
        [SerializeField] private ParticleSystem _muzzleFlare;
        [SerializeField] private ProjectileView _projectilePrefab;
        [SerializeField] private int _projectilePoolCapacity;
        [SerializeField] private WeaponProjectileConfiguration _weaponProjectileConfiguration;
        [SerializeField] private AnimationCurve _projectileFlightCurve;
        private WeaponProjectilesPool _weaponProjectilesPool;
        public override ExploProjectileWeaponType ExploProjectileWeaponType => ExploProjectileWeaponType.TankTurret;

        private void OnEnable()
        {
            _weaponProjectilesPool = new WeaponProjectilesPool(_projectilePrefab, _projectilePoolCapacity);
        }

        public override void OnFire(IDamagableObject damagableObject)
        {
            _muzzleFlare.Play();
            var projectile = _weaponProjectilesPool.GetProjectile();
            projectile.InitDamage(Damage);
            projectile.InitFraction(WeaponFraction);
            var distance = Vector3.Distance(damagableObject.View.transform.position, transform.position);
            var speed = _weaponProjectileConfiguration.HeavyProjectileSpeed;
            var duration = distance / speed;

            var parabolaVertex = VectorTools.GetParabolaVertex(_projectileStartPoint.position, damagableObject.View.transform.position, 4f);
            var parabolaPoints = VectorTools.GetParabolaPoints(_projectileStartPoint.position, parabolaVertex, damagableObject.View.transform.position, 11);

            projectile.transform.position = _projectileStartPoint.position;

            projectile.transform.DOPath(parabolaPoints, duration)
                .SetEase(_projectileFlightCurve)
                .SetLookAt(0f, false)
                .OnComplete(() => OnProjectileReachedDestination(projectile));
        }

        private void OnProjectileReachedDestination(ProjectileView projectile)
        {
            Timer.SetTimer(1.5f, () =>
            {
                if (!_weaponProjectilesPool.IsInThePool(projectile))
                {
                    _weaponProjectilesPool.ReturnProjectile(projectile);
                }
            });
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