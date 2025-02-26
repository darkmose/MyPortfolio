using Core.Buildings;
using Core.Utilities;
using System;
using UnityEngine;

namespace Core.LobbyBase
{
    public class BaseObjectObserver : IDisposable
    {
        private BaseObject _baseObject;
        public BuildingType BuildingType { get; }
        public SimpleEvent<int> BuildingLevelChangedEvent { get; } = new SimpleEvent<int>();

        public BaseObjectObserver(BaseObject baseObject)
        {
            _baseObject = baseObject;
            BuildingType = baseObject.UpgradableBuilding.BuildingType;
            baseObject.UpgradableBuilding.Level.RegisterValueChangeListener(OnBuildingLevelChanged);
        }

        private void OnBuildingLevelChanged(int level)
        {
            BuildingLevelChangedEvent.Notify(level);
        }

        public void Dispose()
        {
            _baseObject.UpgradableBuilding.Level.UnregisterValueChangeListener(OnBuildingLevelChanged);
        }
    }



}