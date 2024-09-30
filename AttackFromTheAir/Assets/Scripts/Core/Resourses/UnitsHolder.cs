using Core.Buildings;
using Core.GameLogic;
using Core.Units;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Resourses
{
    [CreateAssetMenu(fileName ="UnitsHolder", menuName ="ScriptableObjects/UnitsHolder")]
    public class UnitsHolder : ScriptableObject
    {
        public List<UnitDescriptor> Units;

        public BaseUnitView GetInfantryUnit(InfantryType infantryType)
        {
            var unit = Units.Find(descr=>descr.Type == UnitSpawnerType.Infantry && descr.Infantry == infantryType);
            if (unit != null)
            {
                return unit.Prefab;
            }
            else 
            {
                throw new ArgumentException($"Could not find Infantry Unit of type {infantryType}");
            }
        }  
        
        public BaseUnitView GetMediumEquipmentUnit(MediumEquipmentType mediumEquipmentType)
        {
            var unit = Units.Find(descr=>descr.Type == UnitSpawnerType.MediumEquipment && descr.MediumEquipment == mediumEquipmentType);
            if (unit != null)
            {
                return unit.Prefab;
            }
            else 
            {
                throw new ArgumentException($"Could not find Medium Equipment Unit of type {mediumEquipmentType}");
            }
        } 
        
        public BaseUnitView GetHeavyEquipmentUnit(HeavyEquipmentType heavyEquipmentType)
        {
            var unit = Units.Find(descr=>descr.Type == UnitSpawnerType.HeavyEquipment && descr.HeavyEquipment == heavyEquipmentType);
            if (unit != null)
            {
                return unit.Prefab;
            }
            else 
            {
                throw new ArgumentException($"Could not find Heavy Equipment Unit of type {heavyEquipmentType}");
            }
        }        
    }

    [Serializable]
    public class UnitDescriptor
    {
        public UnitSpawnerType Type;
        public InfantryType Infantry;
        public MediumEquipmentType MediumEquipment;
        public HeavyEquipmentType HeavyEquipment;
        public BaseUnitView Prefab;
    }
}