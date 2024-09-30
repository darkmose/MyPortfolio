using Core.GameLogic;
using UnityEngine;

namespace Core.Units
{
    public interface IUnitView : IDamagableObjectView
    {
        UnitType UnitType { get; }
        UnitBehaviourType UnitBehaviourType { get; } 
        void OnMoveTo(Vector3 pos);
        void OnStopMoving();
    }
}