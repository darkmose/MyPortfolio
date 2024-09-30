using Core.Tools;
using Core.Weapon;
using DG.Tweening;
using UnityEngine;

namespace Core.GameLogic
{
    public class ThrowOffBombWeapon : PlayerWeapon
    {
        [SerializeField] private LayerMask _groundLayer;
        [SerializeField] private float _startPointRadius;
        [SerializeField] private float _endPointRadius;
        [SerializeField] private float _waveFrequency;
        [SerializeField] private float _waveAmplitude;
        private Camera _camera;

        private void Awake()
        {
            _camera = Camera.main;
        }

        protected override void FireInner()
        {
            Debug.Log("[ThrowOffBombWeapon] FireInner");
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

            var selectedStartPos = startPos + Random.onUnitSphere * _startPointRadius;
            var selectedEndPos = endPos + Random.onUnitSphere * _endPointRadius;
            selectedEndPos.y = endPos.y;

            var projectile = WeaponProjectilesPool.GetProjectile();
            projectile.transform.position = selectedStartPos;

            var x = 0f;
            var distance = Vector3.Distance(selectedStartPos, selectedEndPos);
            var direction = forward;
            var duration = distance / PlayerWeaponConfig.Speed;

            DOTween.To(() => x, value => x = value, 1f, duration)
            .OnUpdate(() =>
            {
                    float sineValue = Mathf.Sin(x * distance * _waveFrequency) * _waveAmplitude;
                    Vector3 sineOffset = Vector3.Cross(direction, Vector3.up) * sineValue;
                    projectile.transform.position = Vector3.Lerp(selectedStartPos, selectedEndPos, x) + sineOffset; 
            })
            .SetEase(Ease.Linear);

            projectile.transform.eulerAngles = Vector3.forward * 180f;

            var randX = Random.Range(30f, 80f);
            var randZ = Random.Range(30f, 80f);

            var punch = new Vector3(randX, 0f, randZ);
            projectile.transform.DOPunchRotation(punch, duration, 1, 1).SetEase(Ease.Linear).OnComplete(()=>OnBombReachDestination(projectile));
        }

        private void OnBombReachDestination(ProjectileView projectileView)
        {
            Timer.SetTimer(2f, () =>
            {
                if (!WeaponProjectilesPool.IsInThePool(projectileView))
                {
                    WeaponProjectilesPool.ReturnProjectile(projectileView);
                }
            });
        }
    }
}