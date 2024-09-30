using Core.GameLogic;
using UnityEngine;

namespace Core.Units
{
    public abstract class BaseUnitView : BaseDamagableObjectView, IUnitView
    {
        public abstract BaseUnitSystems UnitSystems { get; }
        public abstract UnitType UnitType { get; }
        public abstract UnitBehaviourType UnitBehaviourType { get; }
        public abstract void OnMoveTo(Vector3 pos);
        public abstract void OnStopMoving();
    }
}