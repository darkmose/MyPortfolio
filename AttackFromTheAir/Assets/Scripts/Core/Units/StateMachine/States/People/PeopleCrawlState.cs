using Core.States;
using UnityEngine;

namespace Core.Units
{
    public class PeopleCrawlState : BaseState<UnitStates>
    {
        private IUnit _unit;
        private int _crawlingBoolProperty = Animator.StringToHash("IsCrawling");
        public override UnitStates State => UnitStates.Crawl;

        public PeopleCrawlState(IUnit unit)
        {
            _unit = unit;
        }

        public override void Enter()
        {
            _unit.UnitView.UnitSystems.UnitAnimator.SetBool(_crawlingBoolProperty, true);
        }

        public override void Exit()
        {

        }
    }
}
