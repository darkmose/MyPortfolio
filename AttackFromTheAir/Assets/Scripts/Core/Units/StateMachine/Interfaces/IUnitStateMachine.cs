using Core.Tools;

namespace Core.Units
{
    public enum UnitStates
    {
        Idle,
        Patrol,
        Attack,
        Dead,
        Prisoner,
        Exploded,
        TargetChase,
        Crawl,
        Stand,
        MoveToSafePoint
    }

    public interface IUnitStateMachine 
    {
        UnitStates InitialState { get; set; }
        TriggerToUnitStateConfiguration TriggerToUnitStateMatrix { get; set; }        
        void InitStateMachine(IUnit unit);
        void SwitchToState(UnitStates state);
        bool HasState(UnitStates state);
    }


}
