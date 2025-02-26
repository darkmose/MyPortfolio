using Core.GameLogic;
using Core.States;

namespace Core.Units
{
    public class PeoplePlayerAttackState : BaseState<UnitStates>
    {
        private PlayerData _playerData;
        private TargetContainer _targetContainer;
        public override UnitStates State => UnitStates.PlayerAttack;

        public PeoplePlayerAttackState(TargetContainer targetContainer, PlayerData playerData)
        {
            _targetContainer = targetContainer;
            _playerData = playerData;
        }

        public override void Enter()
        {
            var attackTarget = _playerData.PlayerHealth;
            _targetContainer.Target = attackTarget;
            stateMachine.SwitchToState(UnitStates.Attack);
        }

        public override void Exit()
        {
        }
    }
}
