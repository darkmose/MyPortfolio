using Core.Level;
using Core.Units;
using Core.Utilities;
using UnityEngine;
using Zenject;

namespace Core.GameLogic
{
    public enum UnitSpawnerType
    {
        Infantry,
        MediumEquipment,
        HeavyEquipment
    }

    public interface IUnitSpawner
    {
        IUnitSpawnerView View { get; set; }
        void Prepare(DiContainer diContainer);
        UnitSpawnerType UnitSpawnerType { get; }
        SimpleEvent<IUnit, Transform> UnitSpawnedEvent { get; }
        void Spawn(UnitFraction unitFraction, UnitInitStateDescriptor initState, Transform position);
    }
}