using Core.GameLogic;
using Core.Units;

namespace Core.Buildings
{
    public enum BuildingType
    {
        Barricade,
        AttackBuilding,
        InfantryBarracks,
        MediumEquipmentSite,
        HeavyEquipmentSite,
        Storage,
        Special,
        Simple
    }

    public enum BarricadeType
    {
        SimpleBarricade
    }

    public enum AttackBuildingType
    {
        Turret
    }

    public enum InfantryType
    {
        Soldier
    }

    public enum MediumEquipmentType
    {
        Jeep
    }

    public enum HeavyEquipmentType
    {
        T90,
        Stryker,
        M1Abrams
    }

    public enum StorageBuildingType
    {
        SimpleStorage
    }

    public enum SpecialBuildingType
    {
        TriggerOnDestroy
    }

    public interface IBuilding : IDamagableObject
    {
        BaseBuildingView BuildingView { get; set; }
        UnitFraction UnitFraction { get; set; }
        BuildingType BuildingType { get; }
    }
}