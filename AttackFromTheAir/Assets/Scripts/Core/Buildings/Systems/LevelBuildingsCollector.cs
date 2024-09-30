using Core.Events;
using Core.Units;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Buildings
{
    public interface ILevelBuildingsCollector
    {
        List<IBuilding> AllyBuildings { get; }
        List<IBuilding> EnemyBuildings { get; }
        List<IBuilding> GetAllBuildings();
        void DestroyBuildings();
    }

    public class LevelBuildingsCollector : ILevelBuildingsCollector
    {
        private List<IBuilding> _allyBuildings;
        private List<IBuilding> _enemyBuildings;
        private UnitSpawnerCreateSystem _spawnerCreateSystem;
        public List<IBuilding> AllyBuildings => _allyBuildings;
        public List<IBuilding> EnemyBuildings => _enemyBuildings;

        public LevelBuildingsCollector(UnitSpawnerCreateSystem spawnerCreateSystem)
        {
            _spawnerCreateSystem = spawnerCreateSystem;
            _allyBuildings = new List<IBuilding>();
            _enemyBuildings = new List<IBuilding>();
            EventAggregator.Subscribe<BuildingWasSpawnedEvent>(OnBuildingSpawned);
        }

        private void OnBuildingSpawned(object sender, BuildingWasSpawnedEvent data)
        {
            if (data.Building.UnitFraction == UnitFraction.Ally)
            {
                _allyBuildings.Add(data.Building);
            }
            else
            {
                _enemyBuildings.Add(data.Building);
            }
        }

        public void DestroyBuildings()
        {
            foreach (var item in _allyBuildings)
            {
                var model = item;
                var view = item.BuildingView;
                DestroyBuilding(model, view);
            }      
            foreach (var item in _enemyBuildings)
            {
                var model = item;
                var view = item.BuildingView;
                DestroyBuilding(model, view);
            }

            _allyBuildings.Clear();
            _enemyBuildings.Clear();
        }

        private void DestroyBuilding(IBuilding model, IBuildingView view)
        {
            model.ObjectDamaged.RemoveListener(view.OnObjectDamaged);
            model.ObjectDestroyed.RemoveListener(view.OnObjectDestroy);
            model.NormalizedHealth.UnregisterValueChangeListener(view.SetHealthNormalized);
            view.GetDamageEvent.RemoveListener(model.TakeDamage);

            if (model is IUpgradableBuilding upgradableBuilding)
            {
                var upgradableBuildingView = view as BaseUpgradableBuildingView;
                upgradableBuilding.Level.UnregisterValueChangeListener(upgradableBuildingView.OnLevelChange);
            }
            if (model is IAttackingBuilding attackingBuilding)
            {
                var attackingBuildingView = view as BaseAttackingBuildingView;
                attackingBuilding.StartAttackingEvent.RemoveListener(attackingBuildingView.OnStartAttacking);
                attackingBuilding.StopAttacking.RemoveListener(attackingBuildingView.OnStopAttacking);
            }
            if (model is IUnitSpawnBuilding unitSpawnBuilding)
            {
                var unitSpawnBuildingView = view as BaseUnitSpawnBuildingView;
                _spawnerCreateSystem.DestroySpawner(unitSpawnBuilding.Spawner);
            }
            if (view is BaseBuildingView buildingView) 
            {
                GameObject.Destroy(buildingView.gameObject);
            }
        }

        public List<IBuilding> GetAllBuildings()
        {
            var buildings = new List<IBuilding>();
            buildings.AddRange(AllyBuildings);
            buildings.AddRange(EnemyBuildings);
            return buildings;
        }
    }
}