using Core.Level;
using Core.States;
using UnityEngine;

namespace Core.Units
{
    public class PeoplePrisonerState : BaseState<UnitStates>
    {
        public override UnitStates State => UnitStates.Prisoner;
        private readonly IUnit _unit;
        private readonly ILevelController _levelController;
        private readonly int _velocityParam = Animator.StringToHash("VelForward");
        private readonly int _prisonerParam = Animator.StringToHash("IsPrisoner");

        public PeoplePrisonerState(IUnit unit, ILevelController levelController)
        {
            _unit = unit;
            _levelController = levelController;
        }

        public override void Enter()
        {
            _unit.UnitView.gameObject.tag = "Prisoner";
            _unit.UnitView.UnitSystems.UnitAnimator.SetFloat(_velocityParam, 0f);
            _unit.UnitView.UnitSystems.UnitAnimator.SetBool(_prisonerParam, true);
        }

        public override void Exit()
        {
            _unit.UnitView.UnitSystems.UnitAnimator.SetBool(_prisonerParam, false);

        }
    }
}
