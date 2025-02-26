using Core.Buildings;
using System.Collections.Generic;

namespace Core.LobbyBase
{
    public class BarricadeUpgradableData : BaseObjectUpgradableData
    {
        public int BarricadeAmount;

        public BarricadeUpgradableData(BaseObject baseObject) : base(baseObject)
        {
        }

        public override BuildingType BuildingType => BuildingType.Barricade;

        protected override void AddInnerData(Dictionary<BaseObjectDataType, object> data)
        {
            data.Add(BaseObjectDataType.Amount, BarricadeAmount);
        }

        protected override void AddInnerSpecialBonusData(Dictionary<BaseObjectSpecialBonusType, object> data)
        {
        }

        protected override void CalculateNextLevelDataInner(Dictionary<BaseObjectDataType, object> data)
        {
            var barricadeAmount = (int)BaseObjectUpgradableDataConfiguration.CalculateDataForBuilding(BuildingType, BaseObjectDataType.Amount, CurrentLevel + 1);
            data.Add(BaseObjectDataType.Amount, barricadeAmount);
        }

        protected override void CalculateNextLevelSpecialBonusDataInner(Dictionary<BaseObjectSpecialBonusType, object> data)
        {
        }

        protected override void InitDataInner(BaseObjectUpgradableDataConfiguration baseObjectUpgradableDataConfiguration, int currentLevel)
        {
            BarricadeAmount = (int)baseObjectUpgradableDataConfiguration.CalculateDataForBuilding(BuildingType, BaseObjectDataType.Amount, currentLevel);
        }


    }
}