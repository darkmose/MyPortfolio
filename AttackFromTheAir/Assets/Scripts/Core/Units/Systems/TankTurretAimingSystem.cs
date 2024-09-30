using Core.GameLogic;
using Core.Tools;
using DG.Tweening;
using UnityEngine;

namespace Core.Units
{
    public class TankTurretAimingSystem : MonoBehaviour
    {
        [SerializeField] private Transform _turretTower;
        [SerializeField] private Transform _turretGun;
        private Sequence _aimSequence;
        private Quaternion _towerRotation;
        private Quaternion _gunRotation;

        private void Awake()
        {
            _towerRotation = _turretTower.localRotation;
            _gunRotation = _turretGun.localRotation;
        }

        public void ResetAim()
        {
            _aimSequence?.Kill();
            var rotateTower = _turretTower.DOLocalRotateQuaternion(_towerRotation, 1f);
            var rotateGun = _turretGun.DOLocalRotateQuaternion(_gunRotation, 1f);

            _aimSequence = DOTween.Sequence().Append(rotateTower).Join(rotateGun);
        }

        public void AimAtTarget(IDamagableObject damagableObjectView, float duration) 
        {
            _aimSequence?.Kill();

            var targetPos = VectorTools.GetParabolaVertex(_turretGun.position, damagableObjectView.View.transform.position, 4f);
            var directionToTarget = (targetPos - _turretTower.position).normalized;
            
            Vector3 turretDirection = new Vector3(directionToTarget.x, 0, directionToTarget.z);
            Quaternion turretTargetRotation = Quaternion.LookRotation(turretDirection);

            var towerRotationTween = _turretTower.DORotateQuaternion(turretTargetRotation, duration);

            Vector3 localTargetPosition = _turretTower.InverseTransformPoint(targetPos);

            Quaternion barrelTargetRotation = Quaternion.LookRotation(directionToTarget);
            var barrelRotationTween = _turretGun.DORotateQuaternion(barrelTargetRotation, duration);

            _aimSequence = DOTween.Sequence().Append(towerRotationTween).Join(barrelRotationTween);
        }

        private void OnDestroy()
        {
            _aimSequence?.Kill();
        }
    }
}