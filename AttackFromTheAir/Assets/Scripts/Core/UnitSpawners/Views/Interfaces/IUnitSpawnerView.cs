using Core.Units;
using UnityEngine;

namespace Core.GameLogic
{
    public interface IUnitSpawnerView
    {
        UnitSpawnerType UnitSpawnerType { get; }
        void OnUnitSpawn(IUnit unit, Transform point);
    }
}