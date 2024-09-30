using Core.GameLogic;
using Core.Level;
using Zenject;

namespace Core.Units
{
    public class NonAttackingPeopleStateMachine : BaseUnitStateMachine
    {
        private ExplosionData _explosionData;

        public NonAttackingPeopleStateMachine(DiContainer diContainer) : base(diContainer)
        {
        }

        protected override void PrepareStateMachine(IUnit unit)
        {
            var levelController = _diContainer.Resolve<ILevelController>();
            var idleState = new PeopleIdleState(unit);
            var patrolState = new PeoplePatrolState(unit, levelController);
            var dieState = new PeopleDeathState(unit);
            var explodedState = new PeopleExplodedState(unit);
            var stand = new PeopleStandState(unit);
            var crawl = new PeopleCrawlState(unit);
            var prisoner = new PeoplePrisonerState(unit, levelController);
            var moveToSafePlace = new PeopleMoveToSafePointState(_unit, levelController);
            InitiateStateMachine(idleState, patrolState, dieState, explodedState, stand, crawl, prisoner, moveToSafePlace);
        }
    }
}
