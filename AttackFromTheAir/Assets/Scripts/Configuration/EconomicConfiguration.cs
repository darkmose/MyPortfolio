using Core.Buildings;
using Core.PlayerModule;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Configuration
{
    [CreateAssetMenu(fileName = nameof(EconomicConfiguration),menuName = "ScriptableObjects/"+nameof(EconomicConfiguration))]
    public class EconomicConfiguration : ScriptableObject
    {
        public BaseObjectsUpgradeConfiguration BaseObjectsUpgradeConfiguration;
    }

    [System.Serializable]
    public class BaseObjectsUpgradeConfiguration
    {
        [SerializeField] private List<BaseObjectUpgradeDescriptor> _baseObjectUpgradeDescriptors;
        private Dictionary<BuildingType, BaseObjectUpgradeDescriptor> _upgradeCostsDictionary;

        private void Prepare()
        {
            if (_upgradeCostsDictionary is null)
            {
                _upgradeCostsDictionary = new Dictionary<BuildingType, BaseObjectUpgradeDescriptor>();
                foreach (var descr in _baseObjectUpgradeDescriptors)
                {
                    _upgradeCostsDictionary.Add(descr.BuildingType, descr);
                }
            }
        }

        public int ProvideStartHammerCurrencyCost(BuildingType buildingType)
        {
            Prepare();
            if (_upgradeCostsDictionary.TryGetValue(buildingType, out var baseObjectUpgradeDescriptor))
            {
                return baseObjectUpgradeDescriptor.StartHammersLevelCost;
            }
            else
            {
                throw new System.Exception($"Could not find upgrade descriptor for Building of type {buildingType}");
            }
        }

        public CostDescriptor ProvideSoftCurrencyCostFor(BuildingType buildingType, int buildingLevel)
        {
            Prepare();
            if (_upgradeCostsDictionary.TryGetValue(buildingType, out var baseObjectUpgradeDescriptor))
            {
                return baseObjectUpgradeDescriptor.ProvideSoftCurrencyCostForLevel(buildingLevel);
            }
            else
            {
                throw new System.Exception($"Could not find upgrade descriptor for Building of type {buildingType}");
            }
        }

        public CostDescriptor ProvideHardCurrencyCostFor(BuildingType buildingType, int buildingLevel)
        {
            Prepare();
            if (_upgradeCostsDictionary.TryGetValue(buildingType, out var baseObjectUpgradeDescriptor))
            {
                return baseObjectUpgradeDescriptor.ProvideHardCurrencyCostForLevel(buildingLevel);
            }
            else
            {
                throw new System.Exception($"Could not find upgrade descriptor for Building of type {buildingType}");
            }
        }

        public int ProvideHammersCostFor(BuildingType buildingType, int buildingLevel)
        {
            Prepare();
            if (_upgradeCostsDictionary.TryGetValue(buildingType, out var baseObjectUpgradeDescriptor))
            {
                return baseObjectUpgradeDescriptor.ProvideHammersCostForLevel(buildingLevel);
            }
            else
            {
                throw new System.Exception($"Could not find upgrade descriptor for Building of type {buildingType}");
            }
        }
    }

    [System.Serializable]
    public class BaseObjectUpgradeDescriptor
    {
        private const int LEVELS_OFFSET = 2;
        public BuildingType BuildingType;

        [Header("Procedural Cost Calculation")]
        [Space(10)]
        [Header("Soft Currency")]
        public CostDescriptor SoftCurrencyLevelCost;
        public int StartSoftCurrencyCost;
        [Header("Hard Currency")]
        public CostDescriptor HardCurrencyLevelCost;
        public int StartHardCurrencyCost;

        [Header("Hammers Currency")]
        public int HammersLevelCost;
        public int StartHammersLevelCost;

        public CostDescriptor ProvideSoftCurrencyCostForLevel(int level)
        {
            var descriptor = new CostDescriptor(SoftCurrencyLevelCost);
            var costMultiplier = level - LEVELS_OFFSET;
            descriptor.Amount *= costMultiplier;
            descriptor.Amount += StartSoftCurrencyCost;
            return descriptor;
        }

        public CostDescriptor ProvideHardCurrencyCostForLevel(int level)
        {
            var descriptor = new CostDescriptor(HardCurrencyLevelCost);
            var costMultiplier = level - LEVELS_OFFSET;
            descriptor.Amount *= costMultiplier;
            descriptor.Amount += StartHardCurrencyCost;
            return descriptor;
        }

        public int ProvideHammersCostForLevel(int level)
        {
            var levelCost = HammersLevelCost;
            var costMultiplier = level - LEVELS_OFFSET;
            levelCost *= costMultiplier;
            levelCost += StartHammersLevelCost;
            return levelCost;
        }
    }

    [System.Serializable]
    public class CostDescriptor
    {
        public MoneyType MoneyType;
        public int Amount;

        public CostDescriptor()
        {
        }

        public CostDescriptor(CostDescriptor copy)
        {
            MoneyType = copy.MoneyType;
            Amount = copy.Amount;
        }
    }
}