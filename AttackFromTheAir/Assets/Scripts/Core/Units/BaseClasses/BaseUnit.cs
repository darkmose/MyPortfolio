using Core.GameLogic;
using Core.Utilities;
using UnityEngine;

namespace Core.Units
{
    public abstract class BaseUnit : BaseDamagableObject, IUnit
    {
        private SimpleEvent<Vector3> _moveToEvent = new SimpleEvent<Vector3>();
        private SimpleEvent _stopMovingEvent = new SimpleEvent();
        public abstract UnitType UnitType { get; }
        public abstract UnitBehaviourType UnitBehaviourType { get; }
        public SimpleEvent<Vector3> MoveToEvent => _moveToEvent;
        public SimpleEvent StopMovingEvent => _stopMovingEvent;
        public BaseUnitView UnitView { get; set; }
        public UnitFraction UnitFraction { get; set; }
        public IUnitStateMachine StateMachine { get; set; }
        public UnitCategory UnitCategory { get; set; }

        public virtual void MoveTo(Vector3 pos)
        {
            _moveToEvent.Notify(pos); 
        }

        public virtual void StopMoving()
        {
            _stopMovingEvent.Notify();
        }
    }
}