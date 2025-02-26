using Core.Buildings;
using System.Collections.Generic;

namespace Core.LobbyBase
{
    public class StorageUpgradableData : BaseObjectUpgradableData
    {
        public int AdditionalRewardValue;

        public StorageUpgradableData(BaseObject baseObject) : base(baseObject)
        {
        }

        public override BuildingType BuildingType => BuildingType.Storage;

        protected override void AddInnerData(Dictionary<BaseObjectDataType, object> data)
        { 
        }

        protected override void AddInnerSpecialBonusData(Dictionary<BaseObjectSpecialBonusType, object> data)
        {
            data.Add(BaseObjectSpecialBonusType.AdditionalMoneyReward, AdditionalRewardValue);
        }

        protected override void CalculateNextLevelDataInner(Dictionary<BaseObjectDataType, object> data)
        {
        }

        protected override void CalculateNextLevelSpecialBonusDataInner(Dictionary<BaseObjectSpecialBonusType, object> data)
        {
            var additionalRewardValue = (int)BaseObjectUpgradableDataConfiguration.CalculateSpecialBonusDataForBuilding(BuildingType, BaseObjectSpecialBonusType.AdditionalMoneyReward, CurrentLevel + 1);
            data.Add(BaseObjectSpecialBonusType.AdditionalMoneyReward, additionalRewardValue);
        }

        protected override void InitDataInner(BaseObjectUpgradableDataConfiguration baseObjectUpgradableDataConfiguration, int currentLevel)
        {
            AdditionalRewardValue = (int)baseObjectUpgradableDataConfiguration.CalculateSpecialBonusDataForBuilding(BuildingType, BaseObjectSpecialBonusType.AdditionalMoneyReward, currentLevel);
        }
    }
}