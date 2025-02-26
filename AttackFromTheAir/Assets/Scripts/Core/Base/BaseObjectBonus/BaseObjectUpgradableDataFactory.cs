using Core.Buildings;
using UnityEngine;
using Zenject;

namespace Core.LobbyBase
{
    public static class BaseObjectUpgradableDataFactory
    {
        public static BaseObjectUpgradableData CreateUpgradableData(BaseObject baseObject)
        {
            switch (baseObject.UpgradableBuilding.BuildingType)
            {
                case BuildingType.Barricade:
                    return new BarricadeUpgradableData(baseObject);
                case BuildingType.Turret:
                    return new TurretUpgradableData(baseObject);
                case BuildingType.InfantryBarracks:
                    return new InfantryBarracksUpgradableData(baseObject);
                case BuildingType.MediumEquipmentSite:
                    return new MediumEquipmentSiteUpgradableData(baseObject);
                case BuildingType.HeavyEquipmentSite:
                    return new HeavyEquipmentSiteUpgradableData(baseObject);
                case BuildingType.Storage:
                    return new StorageUpgradableData(baseObject);
                case BuildingType.DroneSite:
                    return new DroneSiteUpgradableData(baseObject);
                default:
                    throw new System.ArgumentException($"Could not create upgradable data for base object of type {baseObject.UpgradableBuilding.BuildingType}");
            }
        }

    }
}