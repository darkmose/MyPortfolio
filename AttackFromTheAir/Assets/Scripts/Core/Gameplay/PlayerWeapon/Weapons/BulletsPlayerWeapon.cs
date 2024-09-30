using Core.Weapon;
using DG.Tweening;
using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Core.GameLogic
{
    public class BulletsPlayerWeapon : PlayerWeapon
    {
        [SerializeField] private LayerMask _groundLayer;
        private Camera _camera;

        private void Awake()
        {
            _camera = Camera.main;
        }

        protected override void FireInner()
        {
            Debug.Log("[BulletsPlayerWeapon] FireInner");
            var projectile = WeaponProjectilesPool.GetProjectile();
            var forward = _camera.transform.forward;
            var startPos = transform.position + forward;
            var screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
            var ray = _camera.ScreenPointToRay(screenCenter);
            Vector3 endPos;

            if (Physics.Raycast(ray, out var hit, 1000, _groundLayer))
            {
                endPos = hit.point;
            }
            else
            {
                endPos = transform.position + forward * 100;
            }

            var selectedEndPos = endPos + UnityEngine.Random.onUnitSphere * 1f;
            selectedEndPos.y = endPos.y;

            projectile.transform.position = startPos;
            var distance = Vector3.Distance(startPos, selectedEndPos);
            var duration = distance / PlayerWeaponConfig.Speed;
            var direction = (selectedEndPos - startPos).normalized;
            projectile.transform.rotation = Quaternion.LookRotation(direction);

            projectile.transform.DOMove(selectedEndPos, duration).SetEase(Ease.Linear).OnComplete(()=>OnProjectileReachDestination(projectile));
        }


        private void OnProjectileReachDestination(ProjectileView projectileView)
        {
            Core.Tools.Timer.SetTimer(2f, () =>
            {
                if (!WeaponProjectilesPool.IsInThePool(projectileView))
                {
                    WeaponProjectilesPool.ReturnProjectile(projectileView);
                }
            });
        }
    }
}