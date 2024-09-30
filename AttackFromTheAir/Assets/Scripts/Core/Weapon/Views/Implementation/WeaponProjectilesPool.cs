using Core.Events;
using Core.Tools;
using DG.Tweening;
using System;

namespace Core.Weapon
{
    public class WeaponProjectilesPool
    {
        private const string POOL_TAG = "Pool";
        private ObjectPooler<ProjectileView> _pool;

        public WeaponProjectilesPool(ProjectileView projectilePrefab, int poolCapacity, Action<ProjectileView> onCreateAction = null)
        {
            _pool = new ObjectPooler<ProjectileView>(TransformsHolderService.ProjectilesContainer, TransformsHolderService.UsedProjectilesContainer);
            _pool.CreatePool(POOL_TAG, projectilePrefab, poolCapacity, onCreateAction);
            EventAggregator.Subscribe<LevelDisposeEvent>(OnLevelDispose);
        }

        private void OnLevelDispose(object sender, LevelDisposeEvent data)
        {
            EventAggregator.Unsubscribe<LevelDisposeEvent>(OnLevelDispose);
            Dispose(BeforeDisposeAction);
        }

        private void BeforeDisposeAction(ProjectileView projectileView)
        {
            projectileView.transform.DOKill();
        }

        public ProjectileView GetProjectile()
        {
            return _pool.GetPooledGameObject(POOL_TAG);
        }

        public bool IsInThePool(ProjectileView projectile)
        {
            return _pool.IsInPool(projectile);
        }

        public void ReturnProjectile(ProjectileView projectileView)
        {
            _pool.ReturnObject(projectileView);
        }

        public void Dispose(Action<ProjectileView> beforeDestroyAction = null)
        {
            _pool.Dispose(beforeDestroyAction);
        }
    }
}