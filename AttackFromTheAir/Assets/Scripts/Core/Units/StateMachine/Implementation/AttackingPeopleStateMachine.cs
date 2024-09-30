using Core.Level;
using Zenject;

namespace Core.Units
{
    public partial class AttackingPeopleStateMachine : BaseUnitStateMachine
    {
        private TargetContainer _targetContainer;

        public AttackingPeopleStateMachine(DiContainer diContainer) : base(diContainer)
        {
            _targetContainer = new TargetContainer();
        }

        protected override void PrepareStateMachine(IUnit unit)
        {
            var levelController = _diContainer.Resolve<ILevelController>();
            var idleState = new PeopleIdleState(unit);
            var patrolState = new PeoplePatrolState(unit, levelController);
            var dieState = new PeopleDeathState(unit);
            var explodedState = new PeopleExplodedState(unit);
            var targetChaseState = new UnitTargetChaseState(unit, levelController, _targetContainer);
            var targetAttackState = new PeopleAttackState(unit, levelController, _targetContainer);
            var stand = new PeopleStandState(unit);
            var crawl = new PeopleCrawlState(unit);
            var prisoner = new PeoplePrisonerState(unit, levelController);
            var moveToSafePlace = new PeopleMoveToSafePointState(_unit, levelController);
            InitiateStateMachine(idleState, patrolState, dieState, explodedState, targetChaseState, targetAttackState, stand, crawl, prisoner, moveToSafePlace);
        }
    }
}
