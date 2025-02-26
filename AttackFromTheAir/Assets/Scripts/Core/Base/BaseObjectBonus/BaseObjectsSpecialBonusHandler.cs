using Core.Utilities;
using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Core.LobbyBase
{
    public class BaseObjectsSpecialBonusHandler
    {
        private PlayerBasesManager _playerBasesManager;
        private Dictionary<BaseObjectSpecialBonusType, List<BaseObject>> _specialBonusBaseObjects;
        private Dictionary<BaseObjectSpecialBonusType, object> _specialBonusesDict;
        public SimpleEvent<BaseObjectSpecialBonusType, object> SpecialBonusChangedEvent { get; } = new SimpleEvent<BaseObjectSpecialBonusType, object>();

        public BaseObjectsSpecialBonusHandler(PlayerBasesManager playerBasesManager)
        {
            _playerBasesManager = playerBasesManager;
            _specialBonusBaseObjects = new Dictionary<BaseObjectSpecialBonusType, List<BaseObject>>();
            _specialBonusesDict = new Dictionary<BaseObjectSpecialBonusType, object>();
        }

        public void InitHandling()
        {
            var bases = _playerBasesManager.Bases;
            _playerBasesManager.NewBaseAddedEvent.AddListener(OnNewBaseAdded);
            foreach (var @base in bases)
            {
                InitBaseHandling(@base);
            }
            RefreshSpecialBonusData();
        }

        private void InitBaseHandling(Base @base)
        {
            var objs = @base.BaseObjectsDict;
            foreach (var item in objs)
            {
                foreach (var obj in item.Value)
                {
                    var specialBonusData = obj.BaseObjectUpgradableData.GetSpecialBonusData();
                    if (specialBonusData.Count > 0)
                    {
                        foreach (var data in specialBonusData)
                        {
                            if (_specialBonusBaseObjects.ContainsKey(data.Key))
                            {
                                _specialBonusBaseObjects[data.Key].Add(obj);
                            }
                            else
                            {
                                _specialBonusBaseObjects.Add(data.Key, new List<BaseObject>());
                                _specialBonusBaseObjects[data.Key].Add(obj);
                            }
                        }
                    }
                    obj.UpgradableBuilding.Level.RegisterValueChangeListener(OnBuildingLevelUp);
                }
            }
        }

        private void OnNewBaseAdded()
        {
            var newBase = _playerBasesManager.Bases.Last();
            InitBaseHandling(newBase);
            RefreshSpecialBonusData();
        }

        private void OnBuildingLevelUp(int level)
        {
            RefreshSpecialBonusData();
        }

        private void RefreshSpecialBonusData()
        {
            foreach (var item in _specialBonusBaseObjects)
            {
                if (_specialBonusesDict.ContainsKey(item.Key))
                {
                    _specialBonusesDict[item.Key] = 0;
                }
                else
                {
                    _specialBonusesDict.Add(item.Key, 0);
                }
                foreach (var obj in item.Value)
                {
                    if (obj.IsUnlocked.Value)
                    {
                        CalculateSpecialBonusData(item.Key, obj.BaseObjectUpgradableData);
                    }
                }
            }
        }

        private void CalculateSpecialBonusData(BaseObjectSpecialBonusType specialBonusType, BaseObjectUpgradableData upgradableData)
        {
            var data = upgradableData.GetSpecialBonusData();
            var current = _specialBonusesDict[specialBonusType];
            object result = (int)current + (int)data[specialBonusType];

            if (result != null)
            {
                if (!current.Equals(result))
                {
                    _specialBonusesDict[specialBonusType] = result;
                    SpecialBonusChangedEvent.Notify(specialBonusType, result);
                }
            }
        }

        public bool TryGetSpecialBonusData(BaseObjectSpecialBonusType specialBonusType, out object specialBonusData)
        {
            if (_specialBonusesDict.TryGetValue(specialBonusType, out var data))
            {
                specialBonusData = data;
                return true;
            }
            else
            {
                specialBonusData = null;
                return false;
            }
        }
    }
}