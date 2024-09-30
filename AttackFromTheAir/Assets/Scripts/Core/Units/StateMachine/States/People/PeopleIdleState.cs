using Core.States;
using UnityEngine;

namespace Core.Units
{
    public class PeopleIdleState : BaseState<UnitStates>
    {
        public override UnitStates State => UnitStates.Idle;
        private readonly int _velocityParam = Animator.StringToHash("VelForward"); 
        private readonly IUnit _unit;

        public PeopleIdleState(IUnit unit)
        {
            _unit = unit;
        }

        public override void Enter()
        {
            _unit.UnitView.UnitSystems.UnitAnimator.SetFloat(_velocityParam, 0f);
        }

        public override void Exit()
        {
            
        }
    }
}
