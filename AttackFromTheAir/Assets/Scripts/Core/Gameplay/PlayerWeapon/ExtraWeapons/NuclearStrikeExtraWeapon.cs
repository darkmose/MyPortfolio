using Core.Tools;
using Core.Weapon;
using DG.Tweening;
using LunarConsolePlugin;
using UnityEngine;

namespace Core.GameLogic
{
    public class NuclearStrikeExtraWeapon : PlayerExtraWeapon
    {
        [SerializeField] private LayerMask _groundLayer;
        private Camera _camera;

        private void Awake()
        {
            _camera = Camera.main;
        }

        protected override void FireInner()
        {
            var forward = _camera.transform.forward;
            var worldForward = Vector3.forward * Random.Range(-10f, 10f);
            var startPos = transform.position - forward * 5f - worldForward;
            var screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);

            var centerViewport = new Vector2(0.5f, 0.5f);
            var ray = _camera.ViewportPointToRay(centerViewport);

            Vector3 endPos;

            if (Physics.Raycast(ray, out var hit, 1000, _groundLayer))
            {
                endPos = hit.point;
            }
            else
            {
                endPos = transform.position + forward * 100;
            }

            var selectedStartPos = startPos + Random.onUnitSphere;
            var selectedEndPos = endPos + Random.onUnitSphere;
            selectedEndPos.y = endPos.y;

            var projectile = WeaponProjectilesPool.GetProjectile();

            var parabolaVertex = VectorTools.GetParabolaVertex(selectedStartPos, selectedEndPos, 8f);
            var projectilePath = VectorTools.GetParabolaPoints(selectedStartPos, parabolaVertex, selectedEndPos, 5);

            var distance = Vector3.Distance(selectedStartPos, selectedEndPos);
            var duration = distance / PlayerWeaponConfig.Speed * 0.4f;

            projectile.transform.position = selectedStartPos;
            projectile.transform.DOPath(projectilePath, duration).SetLookAt(0.1f, false).SetEase(Ease.InOutSine)
                .OnComplete(() => OnBombReachDestination(projectile));

            var lookVector = Vector3.down + Random.onUnitSphere;
            lookVector.y = Mathf.Clamp(lookVector.y, -1f, 0.1f);
            projectile.transform.rotation = Quaternion.LookRotation(lookVector);
            
        }

        private void OnBombReachDestination(ProjectileView projectileView)
        {
            Timer.SetTimer(1.5f, () =>
            {
                if (!WeaponProjectilesPool.IsInThePool(projectileView))
                {
                    WeaponProjectilesPool.ReturnProjectile(projectileView);
                }
            });
        }
    }
}