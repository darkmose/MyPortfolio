using Core.Buildings;
using System.Collections.Generic;

namespace Core.LobbyBase
{
    public class MediumEquipmentSiteUpgradableData : BaseObjectUpgradableData
    {
        public int MediumEquipmentAmount;
        public int MediumEquipmentLife;
        public int MediumEquipmentRespawnDelay;
        public int MediumEquipmentType;

        public MediumEquipmentSiteUpgradableData(BaseObject baseObject) : base(baseObject)
        {
        }

        public override BuildingType BuildingType => BuildingType.MediumEquipmentSite;

        protected override void AddInnerData(Dictionary<BaseObjectDataType, object> data)
        {
            data.Add(BaseObjectDataType.Amount, MediumEquipmentAmount);
            data.Add(BaseObjectDataType.MediumEquipmentLife, MediumEquipmentLife);
            data.Add(BaseObjectDataType.MediumEquipmentRespawnDelay, MediumEquipmentRespawnDelay);
            data.Add(BaseObjectDataType.MediumEquipmentType, MediumEquipmentType);
        }

        protected override void AddInnerSpecialBonusData(Dictionary<BaseObjectSpecialBonusType, object> data)
        {
        }

        protected override void CalculateNextLevelDataInner(Dictionary<BaseObjectDataType, object> data)
        {
            var mediumEquipmentAmount = (int)BaseObjectUpgradableDataConfiguration.CalculateDataForBuilding(BuildingType, BaseObjectDataType.Amount, CurrentLevel + 1);
            var mediumEquipmentLife = (int)BaseObjectUpgradableDataConfiguration.CalculateDataForBuilding(BuildingType, BaseObjectDataType.MediumEquipmentLife, CurrentLevel + 1);
            var mediumEquipmentRespawnDelay = (int)BaseObjectUpgradableDataConfiguration.CalculateDataForBuilding(BuildingType, BaseObjectDataType.MediumEquipmentRespawnDelay, CurrentLevel + 1);
            var mediumEquipmentType = (int)BaseObjectUpgradableDataConfiguration.CalculateDataForBuilding(BuildingType, BaseObjectDataType.MediumEquipmentType, CurrentLevel + 1);

            data.Add(BaseObjectDataType.Amount, mediumEquipmentAmount);
            data.Add(BaseObjectDataType.MediumEquipmentLife, mediumEquipmentLife);
            data.Add(BaseObjectDataType.MediumEquipmentRespawnDelay, mediumEquipmentRespawnDelay);
            data.Add(BaseObjectDataType.MediumEquipmentType, mediumEquipmentType);
        }

        protected override void CalculateNextLevelSpecialBonusDataInner(Dictionary<BaseObjectSpecialBonusType, object> data)
        {
        }

        protected override void InitDataInner(BaseObjectUpgradableDataConfiguration baseObjectUpgradableDataConfiguration, int currentLevel)
        {
            MediumEquipmentAmount = (int)baseObjectUpgradableDataConfiguration.CalculateDataForBuilding(BuildingType, BaseObjectDataType.Amount, currentLevel);
            MediumEquipmentLife = (int)baseObjectUpgradableDataConfiguration.CalculateDataForBuilding(BuildingType, BaseObjectDataType.MediumEquipmentLife, currentLevel);
            MediumEquipmentRespawnDelay = (int)baseObjectUpgradableDataConfiguration.CalculateDataForBuilding(BuildingType, BaseObjectDataType.MediumEquipmentRespawnDelay, currentLevel);
            MediumEquipmentType = (int)baseObjectUpgradableDataConfiguration.CalculateDataForBuilding(BuildingType, BaseObjectDataType.MediumEquipmentType, currentLevel);
        }
    }
}