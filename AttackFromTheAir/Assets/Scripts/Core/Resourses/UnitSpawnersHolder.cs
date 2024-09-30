using Core.Buildings;
using Core.GameLogic;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Resourses
{
    [CreateAssetMenu(fileName = "UnitSpawnersHolder", menuName = "ScriptableObjects/UnitSpawnersHolder")]
    public class UnitSpawnersHolder : ScriptableObject
    {
        public List<UnitSpawnerDescriptor> Spawners;
        
        public BaseUnitSpawnerView GetInfantrySpawner(InfantryType infantryType)
        {
            var spawner = Spawners.Find(descr=>descr.Type == UnitSpawnerType.Infantry && descr.Infantry == infantryType);
            if (spawner != null) 
            {
                return spawner.Prefab;
            }
            else
            {
                throw new System.Exception($"Could not find Infantry Spawner of type {infantryType}");
            }
        }

        public BaseUnitSpawnerView GetMediumEquipmentSpawner(MediumEquipmentType mediumEquipmentType)
        {
            var spawner = Spawners.Find(descr=>descr.Type == UnitSpawnerType.MediumEquipment && descr.MediumEquipment == mediumEquipmentType);
            if (spawner != null) 
            {
                return spawner.Prefab;
            }
            else
            {
                throw new System.Exception($"Could not find Medium Equipment Spawner of type {mediumEquipmentType}");
            }
        }
        
        public BaseUnitSpawnerView GetHeavyEquipmentSpawner(HeavyEquipmentType heavyEquipmentType)
        {
            var spawner = Spawners.Find(descr=>descr.Type == UnitSpawnerType.HeavyEquipment && descr.HeavyEquipment == heavyEquipmentType);
            if (spawner != null) 
            {
                return spawner.Prefab;
            }
            else
            {
                throw new System.Exception($"Could not find Heavy Equipment Spawner of type {heavyEquipmentType}");
            }
        }

    }

    [System.Serializable]
    public class UnitSpawnerDescriptor
    {
        public UnitSpawnerType Type;
        public InfantryType Infantry;
        public MediumEquipmentType MediumEquipment;
        public HeavyEquipmentType HeavyEquipment;
        public BaseUnitSpawnerView Prefab;
    }
}