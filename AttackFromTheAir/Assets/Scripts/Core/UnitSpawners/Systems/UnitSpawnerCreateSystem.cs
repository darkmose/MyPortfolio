using Core.Buildings;
using Core.GameLogic;
using Core.Resourses;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Units
{
    public class UnitSpawnerCreateSystem : IDisposable
    {
        private UnitSpawnersHolder _unitSpawnersHolder;
        private Dictionary<IUnitSpawner, BaseUnitSpawnerView> _unitViewsDict;
        private Dictionary<InfantryType, IInfantrySpawner> _infantryCachedSpawners;
        private Dictionary<MediumEquipmentType, IMediumEquipmentSpawner> _mediumEquipmentCachedSpawners;
        private Dictionary<HeavyEquipmentType, IHeavyEquipmentSpawner> _heavyEquipmentCachedSpawners;

        public UnitSpawnerCreateSystem()
        {
            _unitSpawnersHolder = Resources.Load<UnitSpawnersHolder>("ScriptableObjects/UnitSpawnersHolder");
            _unitViewsDict = new Dictionary<IUnitSpawner, BaseUnitSpawnerView>();
            _infantryCachedSpawners = new Dictionary<InfantryType, IInfantrySpawner>();
            _mediumEquipmentCachedSpawners = new Dictionary<MediumEquipmentType, IMediumEquipmentSpawner>();
            _heavyEquipmentCachedSpawners = new Dictionary<HeavyEquipmentType, IHeavyEquipmentSpawner>();
        }

        public void DestroySpawner(IUnitSpawner unitSpawner)
        {
            if (unitSpawner is IInfantrySpawner infantrySpawner)
            {
                if (_infantryCachedSpawners.ContainsKey(infantrySpawner.InfantryType))
                {
                    _infantryCachedSpawners.Remove(infantrySpawner.InfantryType);
                }
            }

            if (unitSpawner is IMediumEquipmentSpawner mediumEquipmentSpawner)
            {
                if (_mediumEquipmentCachedSpawners.ContainsKey(mediumEquipmentSpawner.MediumEquipmentType))
                {
                    _mediumEquipmentCachedSpawners.Remove(mediumEquipmentSpawner.MediumEquipmentType);
                }
            }

            if (unitSpawner is IHeavyEquipmentSpawner heavyEquipmentSpawner)
            {
                if (_heavyEquipmentCachedSpawners.ContainsKey(heavyEquipmentSpawner.HeavyEquipmentType))
                {
                    _heavyEquipmentCachedSpawners.Remove(heavyEquipmentSpawner.HeavyEquipmentType);
                }
            }

            if (_unitViewsDict.TryGetValue(unitSpawner, out var view))
            {
                unitSpawner.UnitSpawnedEvent.RemoveListener(view.OnUnitSpawn);
                if (view != null)
                {
                    GameObject.Destroy(view.gameObject);
                }
            }
        }

        public IMediumEquipmentSpawner CreateMediumEquipmentSpawner(MediumEquipmentType mediumEquipmentType)
        {
            if (_mediumEquipmentCachedSpawners.TryGetValue(mediumEquipmentType, out var unitSpawner))
            {
                return unitSpawner;
            }

            switch (mediumEquipmentType)
            {
                case MediumEquipmentType.Jeep:
                    var model = new JeepMediumEquipmentSpawner();
                    LinkMediumEquipmentModelView(model);
                    return model;
                default:
                    throw new System.ArgumentException();
            }
        }

        private void LinkMediumEquipmentModelView(IMediumEquipmentSpawner model)
        {
            var viewPrefab = _unitSpawnersHolder.GetMediumEquipmentSpawner(model.MediumEquipmentType);
            var view = GameObject.Instantiate(viewPrefab);
            model.UnitSpawnedEvent.AddListener(view.OnUnitSpawn);
            model.View = view;
            _unitViewsDict.Add(model, view);
            _mediumEquipmentCachedSpawners.Add(model.MediumEquipmentType, model);
        }

        public IHeavyEquipmentSpawner CreateHeavyEquipmentSpawner(HeavyEquipmentType heavyEquipmentType) 
        {
            if (_heavyEquipmentCachedSpawners.TryGetValue(heavyEquipmentType, out var unitSpawner))
            {
                return unitSpawner;
            }

            switch (heavyEquipmentType)
            {
                case HeavyEquipmentType.T90:
                    var t90model = new T90HeavyEquipmentSpawner();
                    LinkHeavyEquipmentModelView(t90model);
                    return t90model;
                case HeavyEquipmentType.Stryker:
                    var modelStryker = new StrykerHeavyEquipmentSpawner();
                    LinkHeavyEquipmentModelView(modelStryker);
                    return modelStryker;
                case HeavyEquipmentType.M1Abrams:
                    var modelM1Abrams = new M1AbramsHeavyEquipmentSpawner();
                    LinkHeavyEquipmentModelView(modelM1Abrams);
                    return modelM1Abrams;
                default:
                    throw new System.ArgumentException();
            }
        }

        private void LinkHeavyEquipmentModelView(IHeavyEquipmentSpawner model)
        {
            var viewPrefab = _unitSpawnersHolder.GetHeavyEquipmentSpawner(model.HeavyEquipmentType);
            var view = GameObject.Instantiate(viewPrefab);
            model.UnitSpawnedEvent.AddListener(view.OnUnitSpawn);
            model.View = view;
            _unitViewsDict.Add(model, view);
            _heavyEquipmentCachedSpawners.Add(model.HeavyEquipmentType, model);
        }


        public IInfantrySpawner CreateInfantrySpawner(InfantryType infantryType)
        {
            if (_infantryCachedSpawners.TryGetValue(infantryType, out var unitSpawner))
            {
                return unitSpawner;
            }

            switch (infantryType)
            {
                case InfantryType.Soldier:
                    var model = new SoldierInfantrySpawner();
                    LinkInfantryModelView(model);
                    return model;
                default:
                    throw new System.ArgumentException();
            }
        }

        private void LinkInfantryModelView(IInfantrySpawner model)
        {
            var viewPrefab = _unitSpawnersHolder.GetInfantrySpawner(model.InfantryType);
            var view = GameObject.Instantiate(viewPrefab);
            model.UnitSpawnedEvent.AddListener(view.OnUnitSpawn);
            model.View = view;
            _unitViewsDict.Add(model, view);
            _infantryCachedSpawners.Add(model.InfantryType, model);
        }

        public void Dispose()
        {
            var spawners = new List<IUnitSpawner>();

            foreach (var spawner in _infantryCachedSpawners.Values)
            {
                spawners.Add(spawner);
            }
            foreach (var spawner in _mediumEquipmentCachedSpawners.Values)
            {
                spawners.Add(spawner);
            }
            foreach (var spawner in _heavyEquipmentCachedSpawners.Values)
            {
                spawners.Add(spawner);
            }

            foreach (var spawner in spawners)
            {
                DestroySpawner(spawner);
            }
        }
    }
}