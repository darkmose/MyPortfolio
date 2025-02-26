using Core.Buildings;
using System.Collections.Generic;

namespace Core.LobbyBase
{
    public class DroneSiteUpgradableData : BaseObjectUpgradableData
    {
        public int AdditionalDroneUsages;
        public override BuildingType BuildingType => BuildingType.DroneSite;

        public DroneSiteUpgradableData(BaseObject baseObject) : base(baseObject)
        {
        }

        protected override void AddInnerData(Dictionary<BaseObjectDataType, object> data)
        {
        }

        protected override void AddInnerSpecialBonusData(Dictionary<BaseObjectSpecialBonusType, object> data)
        {
            data.Add(BaseObjectSpecialBonusType.AdditionalDroneUse, AdditionalDroneUsages);
        }

        protected override void CalculateNextLevelDataInner(Dictionary<BaseObjectDataType, object> data)
        {
        }

        protected override void CalculateNextLevelSpecialBonusDataInner(Dictionary<BaseObjectSpecialBonusType, object> data)
        {
            var additionalDroneUsages = (int)BaseObjectUpgradableDataConfiguration.CalculateSpecialBonusDataForBuilding(BuildingType, BaseObjectSpecialBonusType.AdditionalDroneUse, CurrentLevel + 1);
            data.Add(BaseObjectSpecialBonusType.AdditionalDroneUse, additionalDroneUsages);
        }

        protected override void InitDataInner(BaseObjectUpgradableDataConfiguration baseObjectUpgradableDataConfiguration, int currentLevel)
        {
            AdditionalDroneUsages = (int)baseObjectUpgradableDataConfiguration.CalculateSpecialBonusDataForBuilding(BuildingType, BaseObjectSpecialBonusType.AdditionalDroneUse, currentLevel);
        }
    }
}