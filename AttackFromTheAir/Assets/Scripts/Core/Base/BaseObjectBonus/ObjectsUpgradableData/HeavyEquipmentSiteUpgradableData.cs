using Core.Buildings;
using System.Collections.Generic;

namespace Core.LobbyBase
{
    public class HeavyEquipmentSiteUpgradableData : BaseObjectUpgradableData
    {
        public int HeavyEquipmentAmount;
        public int HeavyEquipmentLife;
        public int HeavyEquipmentRespawnDelay;
        public int HeavyEquipmentType;

        public HeavyEquipmentSiteUpgradableData(BaseObject baseObject) : base(baseObject)
        {
        }

        public override BuildingType BuildingType => BuildingType.HeavyEquipmentSite;

        protected override void AddInnerData(Dictionary<BaseObjectDataType, object> data)
        {
            data.Add(BaseObjectDataType.Amount, HeavyEquipmentAmount);
            data.Add(BaseObjectDataType.HeavyEquipmentLife, HeavyEquipmentLife);
            data.Add(BaseObjectDataType.HeavyEquipmentRespawnDelay, HeavyEquipmentRespawnDelay);
            data.Add(BaseObjectDataType.HeavyEquipmentType, HeavyEquipmentType);
        }

        protected override void AddInnerSpecialBonusData(Dictionary<BaseObjectSpecialBonusType, object> data)
        {
        }

        protected override void CalculateNextLevelDataInner(Dictionary<BaseObjectDataType, object> data)
        {
            var heavyEquipmentAmount = (int)BaseObjectUpgradableDataConfiguration.CalculateDataForBuilding(BuildingType, BaseObjectDataType.Amount, CurrentLevel + 1);
            var heavyEquipmentLife = (int)BaseObjectUpgradableDataConfiguration.CalculateDataForBuilding(BuildingType, BaseObjectDataType.HeavyEquipmentLife, CurrentLevel + 1);
            var heavyEquipmentRespawnDelay = (int)BaseObjectUpgradableDataConfiguration.CalculateDataForBuilding(BuildingType, BaseObjectDataType.HeavyEquipmentRespawnDelay, CurrentLevel + 1);
            var heavyEquipmentType = (int)BaseObjectUpgradableDataConfiguration.CalculateDataForBuilding(BuildingType, BaseObjectDataType.HeavyEquipmentType, CurrentLevel + 1);

            data.Add(BaseObjectDataType.Amount, heavyEquipmentAmount);
            data.Add(BaseObjectDataType.HeavyEquipmentLife, heavyEquipmentLife);
            data.Add(BaseObjectDataType.HeavyEquipmentRespawnDelay, heavyEquipmentRespawnDelay);
            data.Add(BaseObjectDataType.HeavyEquipmentType, heavyEquipmentType);
        }

        protected override void CalculateNextLevelSpecialBonusDataInner(Dictionary<BaseObjectSpecialBonusType, object> data)
        {

        }

        protected override void InitDataInner(BaseObjectUpgradableDataConfiguration baseObjectUpgradableDataConfiguration, int currentLevel)
        {
            HeavyEquipmentAmount = (int)baseObjectUpgradableDataConfiguration.CalculateDataForBuilding(BuildingType, BaseObjectDataType.Amount, currentLevel);
            HeavyEquipmentLife = (int)baseObjectUpgradableDataConfiguration.CalculateDataForBuilding(BuildingType, BaseObjectDataType.HeavyEquipmentLife, currentLevel);
            HeavyEquipmentRespawnDelay = (int)baseObjectUpgradableDataConfiguration.CalculateDataForBuilding(BuildingType, BaseObjectDataType.HeavyEquipmentRespawnDelay, currentLevel);
            HeavyEquipmentType = (int)baseObjectUpgradableDataConfiguration.CalculateDataForBuilding(BuildingType, BaseObjectDataType.HeavyEquipmentType, currentLevel);
        }
    }
}