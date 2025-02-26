using Core.Buildings;
using Core.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Zenject;

namespace Core.LobbyBase
{
    public class Base : IDisposable
    {
        private BaseView _view;
        private Dictionary<BuildingType, List<BaseObject>> _baseObjectsDict;
        public Dictionary<BuildingType, List<BaseObject>> BaseObjectsDict => _baseObjectsDict;
        public bool IsUpgradableInited { get; set; }
        public bool IsLocked { get; set; }
        public string BaseName { get; set; }

        public void SetName(string name)
        {
            BaseName = name;
        }

        public void SetVisibilityMode(bool isActive)
        {
            _view?.SetVisibilityMode(isActive);
        }

        public void InitLoadData(bool isLocked, List<BaseObjectSaveData> loadData)
        {
            IsLocked = isLocked;
            int index = 0;
            SetVisibilityMode(!isLocked);

            foreach (var obj in _baseObjectsDict)
            {
                foreach (var baseObject in obj.Value)
                {
                    if (index < loadData.Count)
                    {
                        var objectLoadData = loadData[index];
                        baseObject.InitLoadData(objectLoadData);
                    }
                    else
                    {
                        baseObject.InitLoadData(new BaseObjectSaveData());
                    }
                    index++;
                }
            }
        }

        public void InitBaseObjectsUpgradableData()
        {
            foreach (var obj in _baseObjectsDict)
            {
                foreach (var baseObject in obj.Value)
                {
                    if (baseObject.IsUnlocked.Value)
                    {
                        baseObject.BaseObjectUpgradableData.InitData(baseObject.UpgradableBuilding.Level.Value);
                    }
                }
            }
            IsUpgradableInited = true;
        }

        public void ShowBase()
        {
            _view.gameObject.SetActive(true);
        }

        public void HideBase()
        {
            _view.gameObject.SetActive(false);
        }

        public void LinkView(BaseView view)
        {
            _view = view;
            _view.LinkModel(this);
            _baseObjectsDict = new Dictionary<BuildingType, List<BaseObject>>();
            var diContainer = DISimple.ServiceLocator.Resolve<DiContainer>();
            var buildingCreateSystem = diContainer.Resolve<BuildingCreateSystem>();

            foreach (var obj in _view.BaseObjectsDict)
            {
                if (!_baseObjectsDict.ContainsKey(obj.Key))
                {
                    _baseObjectsDict.Add(obj.Key, new List<BaseObject>());
                }
                var baseObjects = _baseObjectsDict[obj.Key];
                foreach (var baseObjectView in obj.Value)
                {
                    var baseObject = new BaseObject();
                    baseObject.InitObjectDescriptor(baseObjectView.ObjectDescriptor);
                    baseObject.LinkView(baseObjectView, buildingCreateSystem);
                    baseObject.InitUpgradableData(BaseObjectUpgradableDataFactory.CreateUpgradableData(baseObject));
                    baseObjects.Add(baseObject);
                }
            }
        }

        public void Dispose()
        {
            _baseObjectsDict.Clear();
            _view?.Dispose();
        }
    }
}