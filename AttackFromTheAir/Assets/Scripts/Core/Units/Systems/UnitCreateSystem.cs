using Core.Events;
using Core.GameLogic;
using Core.Level;
using System;
using System.Collections.Generic;
using Zenject;

namespace Core.Units
{
    public class UnitCreateSystem : IDisposable
    {
        private UnitSpawnerCreateSystem _unitSpawnerCreateSystem;
        private DiContainer _diContainer;

        public UnitCreateSystem(UnitSpawnerCreateSystem unitSpawnerCreateSystem, DiContainer diContainer)
        {
            _unitSpawnerCreateSystem = unitSpawnerCreateSystem;
            EventAggregator.Subscribe<LevelDisposeEvent>(OnLevelDispose);
            _diContainer = diContainer;
        }

        private void OnLevelDispose(object sender, LevelDisposeEvent data)
        {
            Dispose();
        }

        public void CreateUnits(List<UnitSpawnPointsDescriptor> unitSpawnPointsDescriptors)
        {
            IUnitSpawner unitSpawner = null;

            foreach (var descriptor in unitSpawnPointsDescriptors)
            {
                switch (descriptor.UnitSpawnerType)
                {
                    case GameLogic.UnitSpawnerType.Infantry:
                        unitSpawner = _unitSpawnerCreateSystem.CreateInfantrySpawner(descriptor.InfantryType);
                        break;
                    case GameLogic.UnitSpawnerType.MediumEquipment:
                        unitSpawner = _unitSpawnerCreateSystem.CreateMediumEquipmentSpawner(descriptor.MediumEquipmentType);
                        break;
                    case GameLogic.UnitSpawnerType.HeavyEquipment:
                        unitSpawner = _unitSpawnerCreateSystem.CreateHeavyEquipmentSpawner(descriptor.HeavyEquipmentType);
                        break;
                }

                unitSpawner.Prepare(_diContainer);

                foreach (var point in descriptor.SpawnPoints)
                {
                    unitSpawner.Spawn(descriptor.UnitFraction, descriptor.InitStateDescriptor, point);
                }
            }
        }

        public void Dispose()
        {
            _unitSpawnerCreateSystem.Dispose();
        }
    }
}