using Core.GameLogic;
using Core.States;

namespace Core.Units
{
    public class VehiclePlayerAttackState : BaseState<UnitStates>
    {
        private PlayerData _playerData;
        private TargetContainer _targetContainer;
        public override UnitStates State => UnitStates.PlayerAttack;

        public VehiclePlayerAttackState(TargetContainer targetContainer, PlayerData playerData)
        {
            _targetContainer = targetContainer;
            _playerData = playerData;
        }

        public override void Enter()
        {
            var playerTarget = _playerData.PlayerHealth;
            _targetContainer.Target = playerTarget;
            stateMachine.SwitchToState(UnitStates.Attack);
        }

        public override void Exit()
        {
        }
    }
}
