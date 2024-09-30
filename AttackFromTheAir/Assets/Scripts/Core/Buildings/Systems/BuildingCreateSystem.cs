using Core.Events;
using Core.GameLogic;
using Core.Level;
using Core.Resourses;
using Core.Units;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Core.Buildings
{
    public class BuildingCreateSystem
    {
        private BuildingsHolder _buildingsHolder;
        private UnitSpawnerCreateSystem _spawnerCreateSystem;
        private BuildingWasSpawnedEvent _buildingWasSpawnedEvent;
        private DiContainer _diContainer;

        public BuildingCreateSystem(UnitSpawnerCreateSystem spawnerCreateSystem, DiContainer diContainer)
        {
            _buildingsHolder = Resources.Load<BuildingsHolder>("ScriptableObjects/BuildingsHolder");
            _spawnerCreateSystem = spawnerCreateSystem;
            _buildingWasSpawnedEvent = new BuildingWasSpawnedEvent();
            _diContainer = diContainer;
        }

        public IBarricadeBuilding GetBarricadeBuilding(BarricadeType barricadeType)
        {
            switch (barricadeType)
            {
                case BarricadeType.SimpleBarricade:
                    var model = new BarricadeBuilding();
                    var viewPrefab = _buildingsHolder.GetBarricadeBuilding(barricadeType);
                    var view = Object.Instantiate<BaseBuildingView>(viewPrefab);

                    LinkModelWithView(model, view);

                    return model;
                default:
                    throw default;
            }
        }

        private void LinkModelWithView(IBuilding model, IBuildingView view)
        {
            model.ObjectDamaged.AddListener(view.OnObjectDamaged);
            model.ObjectDestroyed.AddListener(view.OnObjectDestroy);
            model.NormalizedHealth.RegisterValueChangeListener(view.SetHealthNormalized);
            view.GetDamageEvent.AddListener(model.TakeDamage);
            view.Model = model;
            if (model is IUpgradableBuilding upgradableBuilding)
            {
                var upgradableBuildingView = view as BaseUpgradableBuildingView;
                upgradableBuilding.Level.RegisterValueChangeListener(upgradableBuildingView.OnLevelChange);
            }
            if (model is IAttackingBuilding attackingBuilding)
            {
                var attackingBuildingView = view as BaseAttackingBuildingView;
                attackingBuilding.StartAttackingEvent.AddListener(attackingBuildingView.OnStartAttacking);
                attackingBuilding.StopAttacking.AddListener(attackingBuildingView.OnStopAttacking);
            }
            if (model is IUnitSpawnBuilding unitSpawnBuilding)
            {
                var unitSpawnBuildingView = view as BaseUnitSpawnBuildingView;
                IUnitSpawner unitSpawner = null;
                IUnitSpawnerView unitSpawnerView = null;

                switch (unitSpawnBuilding.BuildingType)
                {
                    case BuildingType.InfantryBarracks:
                        unitSpawner = _spawnerCreateSystem.CreateInfantrySpawner(InfantryType.Soldier);
                        unitSpawnerView = unitSpawner.View;
                        break;
                    case BuildingType.MediumEquipmentSite:
                        unitSpawner = _spawnerCreateSystem.CreateMediumEquipmentSpawner(MediumEquipmentType.Jeep);
                        unitSpawnerView = unitSpawner.View;
                        break;
                    case BuildingType.HeavyEquipmentSite:
                        unitSpawner = _spawnerCreateSystem.CreateHeavyEquipmentSpawner(HeavyEquipmentType.T90);
                        unitSpawnerView = unitSpawner.View;
                        break;
                }
                unitSpawnBuilding.InitSpawner(unitSpawner);
                unitSpawnBuildingView.InitSpawner(unitSpawnerView);
            }
            CreateStateMachine(model);
            _buildingWasSpawnedEvent.Building = model;
            EventAggregator.Post(this, _buildingWasSpawnedEvent);
        }

        public IAttackingBuilding GetAttackingBuilding(AttackBuildingType attackBuildingType)
        {
            switch (attackBuildingType)
            {
                case AttackBuildingType.Turret:
                    var model = new TurretBuilding();
                    var viewPrefab = _buildingsHolder.GetAttackingBuilding(attackBuildingType);
                    var view = Object.Instantiate<BaseBuildingView>(viewPrefab);
                    LinkModelWithView(model, view);
                    return model;
                default:
                    throw default;
            }
        }

        public IBuilding GetModelForView(IBuildingView view)
        {
            IBuilding model = null;

            switch (view.BuildingType)
            {
                case BuildingType.Barricade:
                    model = new BarricadeBuilding();
                    break;
                case BuildingType.AttackBuilding:
                    if (view is BaseAttackingBuildingView attackingBuildingView)
                    {
                        switch (attackingBuildingView.AttackBuildingType)
                        {
                            case AttackBuildingType.Turret:
                                model = new TurretBuilding();
                                break;
                        }
                    }
                    break;
                case BuildingType.InfantryBarracks:
                    model = new InfantryBarracksBuilding();
                    break;
                case BuildingType.MediumEquipmentSite:
                    model = new MediumEquipmentSiteBuilding();
                    break;
                case BuildingType.HeavyEquipmentSite:
                    model = new HeavyEquipmentSiteBuilding();
                    break;
                case BuildingType.Storage:
                    if (view is BaseStorageBuildingView storageView)
                    {
                        switch (storageView.StorageBuildingType)
                        {
                            case StorageBuildingType.SimpleStorage:
                                model = new StorageBuilding();
                                break;
                        };
                    }
                    break;
                case BuildingType.Special:
                    model = new SimpleBuilding();
                    break;
                case BuildingType.Simple:
                    model = new SimpleBuilding();
                    break;
            }
            if (model == null || view == null)
            {
                throw default;
            }

            if (view is BaseBuildingView buildingView)
            {
                model.BuildingView = buildingView;
            }
            LinkModelWithView(model, view);
            return model;
        }

        private void CreateStateMachine(IBuilding building)
        {
            if (building is IAttackingBuilding attackingBuilding)
            {
                var stateMachine = new AttackBuildingStateMachine(_diContainer);
                stateMachine.InitStateMachine(building);
                stateMachine.SwitchToState(BuildingStates.Idle);
            }
            else
            {
                var stateMachine = new NonAttackBuildingStateMachine(_diContainer);
                stateMachine.InitStateMachine(building);
                stateMachine.SwitchToState(BuildingStates.Idle);
            }
        }

        public void GetModelsForViews(List<BuildingViewsDescriptor> buildingViewsDescriptors)
        {
            if (buildingViewsDescriptors == null || buildingViewsDescriptors.Count == 0) return;

            foreach (var descriptor in buildingViewsDescriptors)
            {
                if(descriptor.Buildings == null || descriptor.Buildings.Count == 0) continue;

                foreach (var buildingView in descriptor.Buildings)
                {
                    var model = GetModelForView(buildingView);
                    model.UnitFraction = descriptor.UnitFraction;
                }
            }
        }
    }
}