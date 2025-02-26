using System;
using System.Collections.Generic;
using Core.Buildings;
using Core.Units;
using UnityEngine;

namespace Core.Resourses
{
    [CreateAssetMenu(fileName = "ExperienceHolder", menuName = "ScriptableObjects/ExperienceHolder", order = 1)]
    public class ExperienceHolder : ScriptableObject
    {
        [SerializeField] public List<UnitExperienceDescriptor> UnitExperienceDescriptors;
        [SerializeField] public List<BuildingExperienceDescriptor> BuildingExperienceDescriptors;
        [SerializeField] public PlayerLevelExperienceData PlayerLevelExperience; 
        [SerializeField] public BaseLevelExperienceData BaseLevelExperienceList;

        private Dictionary<UnitCategory, int> _playerExperienceForUnitsDict;
        private Dictionary<UnitCategory, int> _baseExperienceForUnitsDict;
        private Dictionary<BuildingType, int> _playerExperienceForBuildingsDict;
        private Dictionary<BuildingType, int> _baseExperienceForBuildingsDict;

        private void PrepareUnitExperienceDictionary()
        {
            if (_playerExperienceForUnitsDict == null)
            {
                _playerExperienceForUnitsDict = new Dictionary<UnitCategory, int>();
                foreach (var item in UnitExperienceDescriptors)
                {
                    _playerExperienceForUnitsDict.Add(item.UnitCategory, item.ExperiencePoints);
                }
            }
            if (_baseExperienceForUnitsDict == null)
            {
                _baseExperienceForUnitsDict = new Dictionary<UnitCategory, int>();
                foreach (var item in UnitExperienceDescriptors)
                {
                    _baseExperienceForUnitsDict.Add(item.UnitCategory, item.ExperienceBasePoints);
                }
            }
        }

        private void PrepareBuildingExperienceDictionary()
        {
            if (_playerExperienceForBuildingsDict == null)
            {
                _playerExperienceForBuildingsDict = new Dictionary<BuildingType, int>();
                foreach (var item in BuildingExperienceDescriptors)
                {
                    _playerExperienceForBuildingsDict.Add(item.BuildingType, item.ExperiencePoints);
                }
            }
            if (_baseExperienceForBuildingsDict == null)
            {
                _baseExperienceForBuildingsDict = new Dictionary<BuildingType, int>();
                foreach (var item in BuildingExperienceDescriptors)
                {
                    _baseExperienceForBuildingsDict.Add(item.BuildingType, item.ExperienceBasePoints);
                }
            }
        }

        public int GetUnitExperience(UnitCategory unitCategory, ExperienceType type)
        {
            PrepareUnitExperienceDictionary();
            if (type == ExperienceType.PlayerExp)
            {
                if (_playerExperienceForUnitsDict.TryGetValue(unitCategory, out var value))
                {
                    return value;
                }
                else
                {
                    throw new System.Exception($"Could not find Player EXP value for unit {unitCategory}");
                }
            }
            else
            {
                if (_baseExperienceForUnitsDict.TryGetValue(unitCategory, out var value))
                {
                    return value;
                }
                else
                {                    
                    throw new System.Exception($"Could not find Base EXP value for unit {unitCategory}");
                }
            }
        }

        public int GetBuildingExperience(BuildingType buildingType, ExperienceType type)
        {
            PrepareBuildingExperienceDictionary();
            if (type == ExperienceType.PlayerExp)
            {
                if (_playerExperienceForBuildingsDict.TryGetValue(buildingType, out var value))
                {
                    return value;
                }
                else
                {
                    throw new System.Exception($"Could not find Player EXP value for building {buildingType}");
                }
            }
            else
            {
                if (_baseExperienceForBuildingsDict.TryGetValue(buildingType, out var value))
                {
                    return value;
                }
                else
                {
                    throw new System.Exception($"Could not find Base EXP value for building {buildingType}");
                }
            }
        }

        public int GetRequiredExperienceForPlayerLevel(int level)
        {
            float progressionValue = 800 + Mathf.Pow(level, 2);
            Debug.Log($"Experience for Player level {level} is {progressionValue}");
            return (int)progressionValue;
        }

        public int GetRequiredExperienceForBaseLevel(int level)
        {
            float progressionValue = 1000 + Mathf.Pow(level, 2);
            Debug.Log($"Experience for Base level {level} is {progressionValue}");

            return (int)progressionValue;
        }
    }

    [Serializable]
    public class UnitExperienceDescriptor
    {
        public UnitCategory UnitCategory;
        public int ExperiencePoints;
        public int ExperienceBasePoints;
    }
    
    [Serializable]
    public class BuildingExperienceDescriptor
    {
        public BuildingType BuildingType;
        public int ExperiencePoints;
        public int ExperienceBasePoints;
    }

    [Serializable]
    public class PlayerLevelExperienceData
    {
        public int BaseValue;
        public float Multiplier1;
        public float Multiplier2;
    }

    [Serializable]
    public class BaseLevelExperienceData
    {
        public int BaseValue;
        public float Multiplier1;
        public float Multiplier2;
    }

    public enum ExperienceType
    {
        PlayerExp,
        BaseExp
    }

}


