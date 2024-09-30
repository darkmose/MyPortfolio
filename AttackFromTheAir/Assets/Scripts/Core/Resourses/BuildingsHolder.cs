using Core.Buildings;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Resourses
{
    [CreateAssetMenu(fileName = "BuildingsHolder", menuName = "ScriptableObjects/BuildingsHolder")]
    public class BuildingsHolder : ScriptableObject
    {
        [SerializeField] private List<BuildingDescriptor> _buildingDescriptors;

        public BaseBuildingView GetBarricadeBuilding(BarricadeType barricadeType)
        {
            var building = _buildingDescriptors.Find(pred=>pred.BuildingType == BuildingType.Barricade && pred.BarricadeType == barricadeType);
            if (building != null)
            {
                return building.Prefab;
            }
            else 
            {
                throw new System.ArgumentException($"Could not find Barricade building of type {barricadeType}");
            }
        }

        public BaseBuildingView GetAttackingBuilding(AttackBuildingType attackBuildingType)
        {
            var building = _buildingDescriptors.Find(pred=>pred.BuildingType == BuildingType.AttackBuilding && pred.AttackBuildingType == attackBuildingType);
            if (building != null)
            {
                return building.Prefab;
            }
            else 
            {
                throw new System.ArgumentException($"Could not find Attacking building of type {attackBuildingType}");
            }
        }

        public BaseBuildingView GetInfantryBuilding(InfantryType infantryType)
        {
            var building = _buildingDescriptors.Find(pred=>pred.BuildingType == BuildingType.InfantryBarracks && pred.SpawnedInfantryType == infantryType);
            if (building != null)
            {
                return building.Prefab;
            }
            else 
            {
                throw new System.ArgumentException($"Could not find Infantry Barracks building of type {infantryType}");
            }
        }

        public BaseBuildingView GetMediumEquipmentBuilding(MediumEquipmentType mediumEquipmentType)
        {
            var building = _buildingDescriptors.Find(pred=>pred.BuildingType == BuildingType.MediumEquipmentSite && pred.SpawnedMediumEquipmentType == mediumEquipmentType);
            if (building != null)
            {
                return building.Prefab;
            }
            else 
            {
                throw new System.ArgumentException($"Could not find Medium Equipment Site building of type {mediumEquipmentType}");
            }
        }

        public BaseBuildingView GetHeavyEquipmentBuilding(HeavyEquipmentType heavyEquipmentType)
        {
            var building = _buildingDescriptors.Find(pred=>pred.BuildingType == BuildingType.HeavyEquipmentSite && pred.SpawnedHeavyEquipmentType == heavyEquipmentType);
            if (building != null)
            {
                return building.Prefab;
            }
            else 
            {
                throw new System.ArgumentException($"Could not find Heavy Equipment Site building of type {heavyEquipmentType}");
            }
        }

        public BaseBuildingView GetStorageBuilding(StorageBuildingType storageBuildingType)
        {
            var building = _buildingDescriptors.Find(pred=>pred.BuildingType == BuildingType.Storage && pred.StorageBuildingType == storageBuildingType);
            if (building != null)
            {
                return building.Prefab;
            }
            else 
            {
                throw new System.ArgumentException($"Could not find Storage building of type {storageBuildingType}");
            }
        }

    }

    [System.Serializable]
    public class BuildingDescriptor
    {
        public BuildingType BuildingType;
        public BarricadeType BarricadeType;
        public AttackBuildingType AttackBuildingType;
        public InfantryType SpawnedInfantryType;
        public MediumEquipmentType SpawnedMediumEquipmentType;
        public HeavyEquipmentType SpawnedHeavyEquipmentType;
        public StorageBuildingType StorageBuildingType;
        public BaseBuildingView Prefab;
    }
}