using Core.Buildings;
using Core.Tools;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.LobbyBase
{
    [CreateAssetMenu(fileName =nameof(BaseObjectUpgradableDataConfiguration), menuName ="ScriptableObjects/"+nameof(BaseObjectUpgradableDataConfiguration))]
    public class BaseObjectUpgradableDataConfiguration : SerializedScriptableObject
    {
        [SerializeField] private List<BaseObjectUpgradableDescriptor> _baseObjectUpgradableDescriptors;
        private Dictionary<BuildingType, Dictionary<BaseObjectDataType, BaseObjectDataDescriptor>> _objectDataDictionary;
        private Dictionary<BuildingType, Dictionary<BaseObjectSpecialBonusType, BaseObjectDataSpecialBonusDescriptor>> _objectSpecialBonusDictionary;

        private void PrepareDataDictionary()
        {
            if (_objectDataDictionary == null)
            {
                _objectDataDictionary = new Dictionary<BuildingType, Dictionary<BaseObjectDataType, BaseObjectDataDescriptor>>();
                foreach (var item in _baseObjectUpgradableDescriptors)
                {
                    _objectDataDictionary.Add(item.BuildingType, new Dictionary<BaseObjectDataType, BaseObjectDataDescriptor>());
                    foreach (var dataDescriptor in item.BaseObjectDataDescriptors)
                    {
                        _objectDataDictionary[item.BuildingType].Add(dataDescriptor.DataType, dataDescriptor);
                    }
                }
            }
        }

        private void PrepareSpecialBonusDictionary()
        {
            if (_objectSpecialBonusDictionary == null)
            {
                _objectSpecialBonusDictionary = new Dictionary<BuildingType, Dictionary<BaseObjectSpecialBonusType, BaseObjectDataSpecialBonusDescriptor>>();
                foreach (var item in _baseObjectUpgradableDescriptors)
                {
                    _objectSpecialBonusDictionary.Add(item.BuildingType, new Dictionary<BaseObjectSpecialBonusType, BaseObjectDataSpecialBonusDescriptor>());
                    if (item.BaseObjectDataSpecialBonusDescriptors != null)
                    {
                        foreach (var specialBonusData in item.BaseObjectDataSpecialBonusDescriptors)
                        {
                            _objectSpecialBonusDictionary[item.BuildingType].Add(specialBonusData.BonusType, specialBonusData);
                        }
                    }
                }
            }
        }

        public float CalculateDataForBuilding(BuildingType buildingType, BaseObjectDataType baseObjectDataType, int level)
        {
            PrepareDataDictionary();
            if (_objectDataDictionary.TryGetValue(buildingType, out var dataDict))
            {
                if (dataDict.TryGetValue(baseObjectDataType, out var objectDataDescriptor))
                {
                    return (float)objectDataDescriptor.Data.HandleUpgradableCalculation(level);
                }
                else
                {
                    throw new Exception($"Could not find object data of type {baseObjectDataType}");
                }
            }
            else
            {
                throw new Exception($"Could not find upgradable progression descriptor of building {buildingType}");
            }
        }

        public float CalculateSpecialBonusDataForBuilding(BuildingType buildingType, BaseObjectSpecialBonusType specialBonusType, int level)
        {
            PrepareSpecialBonusDictionary();
            if (_objectSpecialBonusDictionary.TryGetValue(buildingType, out var dataDict))
            {
                if (dataDict.TryGetValue(specialBonusType, out var objectDataDescriptor))
                {
                    return (float)objectDataDescriptor.Data.HandleUpgradableCalculation(level);
                }
                else
                {
                    throw new Exception($"Could not find object special bonus data of type {specialBonusType}");
                }
            }
            else
            {
                throw new Exception($"Could not find upgradable progression descriptor of building {buildingType}");
            }
        }
    }

    [ShowOdinSerializedPropertiesInInspector]
    public class BaseObjectUpgradableDescriptor
    {
        public BuildingType BuildingType;
        public List<BaseObjectDataDescriptor> BaseObjectDataDescriptors;
        public List<BaseObjectDataSpecialBonusDescriptor> BaseObjectDataSpecialBonusDescriptors;
    }

    [ShowOdinSerializedPropertiesInInspector]
    public class BaseObjectDataDescriptor
    {
        public BaseObjectDataType DataType;
        public BaseUpgradableProgressionDescriptor Data;
    }

    [ShowOdinSerializedPropertiesInInspector]
    public class BaseObjectDataSpecialBonusDescriptor
    {
        public BaseObjectSpecialBonusType BonusType;
        public BaseUpgradableProgressionDescriptor Data;
    }
}