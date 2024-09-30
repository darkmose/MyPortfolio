using Core.GameLogic;
using Core.Utilities;
using UnityEngine;

namespace Core.Units
{
    public enum UnitType
    {
        People,
        Vehicle
    }

    public enum UnitBehaviourType
    {
        Attacking,
        NonAttacking
    }

    public enum UnitFraction
    {
        Ally,
        Enemy,
        Player
    }

    public enum UnitCategory
    {
        Infantry,
        MediumEquipment,
        HeavyEquipment
    }

    public interface IUnit : IDamagableObject
    {
        IUnitStateMachine StateMachine { get; set; }
        BaseUnitView UnitView { get; set; }
        SimpleEvent<Vector3> MoveToEvent { get; }
        SimpleEvent StopMovingEvent { get; }
        UnitType UnitType { get; }
        UnitBehaviourType UnitBehaviourType { get; }
        UnitCategory UnitCategory { get; set; }
        UnitFraction UnitFraction { get; set; }
        void MoveTo(Vector3 pos);
        void StopMoving();
    }
}