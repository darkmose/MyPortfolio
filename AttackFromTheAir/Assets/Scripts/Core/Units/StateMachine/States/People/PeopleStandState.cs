using Core.States;
using UnityEngine;

namespace Core.Units
{
    public class PeopleStandState : BaseState<UnitStates>
    {
        private IUnit _unit;
        private int _crawlingBoolProperty = Animator.StringToHash("IsCrawling");
        public override UnitStates State => UnitStates.Stand;

        public PeopleStandState(IUnit unit)
        {
            _unit = unit;
        }

        public override void Enter()
        {
            _unit.UnitView.UnitSystems.UnitAnimator.SetBool(_crawlingBoolProperty, false);
        }

        public override void Exit()
        {
        }
    }
}
