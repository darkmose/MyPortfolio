using UnityEngine;
using UnityEngine.AI;

namespace Core.Units
{
    public class PeopleUnitMoveSystem : UnitMoveSystem
    {
        [SerializeField] private Animator _unitAnimator;
        private readonly int _velocityParam = Animator.StringToHash("VelForward");

        public override void StopMove()
        {
            base.StopMove();
            _unitAnimator.SetFloat(_velocityParam, 0f);
        }

        protected override void AfterCheckDestination()
        {
            base.AfterCheckDestination();
            var vel = NavMeshAgent.velocity.magnitude / NavMeshAgent.speed;
            _unitAnimator.SetFloat(_velocityParam, vel);
        }
    }
}