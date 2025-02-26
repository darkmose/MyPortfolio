using Core.Buildings;
using System.Collections.Generic;

namespace Core.LobbyBase
{
    public class InfantryBarracksUpgradableData : BaseObjectUpgradableData
    {
        public int InfantryAmount;
        public int InfantryLife;
        public int InfantryRespawnDelay;
        public int InfantryType;

        public InfantryBarracksUpgradableData(BaseObject baseObject) : base(baseObject)
        {
        }

        public override BuildingType BuildingType => BuildingType.InfantryBarracks;

        protected override void AddInnerData(Dictionary<BaseObjectDataType, object> data)
        {
            data.Add(BaseObjectDataType.Amount, InfantryAmount);
            data.Add(BaseObjectDataType.InfantryLife, InfantryLife);
            data.Add(BaseObjectDataType.InfantryRespawnDelay, InfantryRespawnDelay);
            data.Add(BaseObjectDataType.InfantyType, InfantryType);
        }

        protected override void AddInnerSpecialBonusData(Dictionary<BaseObjectSpecialBonusType, object> data)
        {
        }

        protected override void CalculateNextLevelDataInner(Dictionary<BaseObjectDataType, object> data)
        {
            var infantryAmount = (int)BaseObjectUpgradableDataConfiguration.CalculateDataForBuilding(BuildingType, BaseObjectDataType.Amount, CurrentLevel + 1);
            var infantryLife = (int)BaseObjectUpgradableDataConfiguration.CalculateDataForBuilding(BuildingType, BaseObjectDataType.InfantryLife, CurrentLevel + 1);
            var infantryRespawnDelay = (int)BaseObjectUpgradableDataConfiguration.CalculateDataForBuilding(BuildingType, BaseObjectDataType.InfantryRespawnDelay, CurrentLevel + 1);
            var infantryType = (int)BaseObjectUpgradableDataConfiguration.CalculateDataForBuilding(BuildingType, BaseObjectDataType.InfantyType, CurrentLevel + 1);

            data.Add(BaseObjectDataType.Amount, infantryAmount);
            data.Add(BaseObjectDataType.InfantryLife, infantryLife);
            data.Add(BaseObjectDataType.InfantryRespawnDelay, infantryRespawnDelay);
            data.Add(BaseObjectDataType.InfantyType, infantryType);
        }

        protected override void CalculateNextLevelSpecialBonusDataInner(Dictionary<BaseObjectSpecialBonusType, object> data)
        {
        }

        protected override void InitDataInner(BaseObjectUpgradableDataConfiguration baseObjectUpgradableDataConfiguration, int currentLevel)
        {
            InfantryAmount = (int)baseObjectUpgradableDataConfiguration.CalculateDataForBuilding(BuildingType, BaseObjectDataType.Amount, currentLevel);
            InfantryLife = (int)baseObjectUpgradableDataConfiguration.CalculateDataForBuilding(BuildingType, BaseObjectDataType.InfantryLife, currentLevel);
            InfantryRespawnDelay = (int)baseObjectUpgradableDataConfiguration.CalculateDataForBuilding(BuildingType, BaseObjectDataType.InfantryRespawnDelay, currentLevel);
            InfantryType = (int)baseObjectUpgradableDataConfiguration.CalculateDataForBuilding(BuildingType, BaseObjectDataType.InfantyType, currentLevel);
        }
    }
}