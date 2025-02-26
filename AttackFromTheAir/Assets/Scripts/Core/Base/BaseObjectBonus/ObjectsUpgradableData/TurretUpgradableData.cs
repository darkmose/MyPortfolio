using Core.Buildings;
using System.Collections.Generic;

namespace Core.LobbyBase
{
    public class TurretUpgradableData : BaseObjectUpgradableData
    {
        public int Damage;
        public int VisualType;

        public TurretUpgradableData(BaseObject baseObject) : base(baseObject)
        {
        }

        public override BuildingType BuildingType => BuildingType.Turret;

        protected override void AddInnerData(Dictionary<BaseObjectDataType, object> data)
        {
            data.Add(BaseObjectDataType.Damage, Damage);
            data.Add(BaseObjectDataType.VisualType, VisualType);
        }

        protected override void AddInnerSpecialBonusData(Dictionary<BaseObjectSpecialBonusType, object> data)
        {
        }

        protected override void CalculateNextLevelDataInner(Dictionary<BaseObjectDataType, object> data)
        {
            var damage = (int)BaseObjectUpgradableDataConfiguration.CalculateDataForBuilding(BuildingType, BaseObjectDataType.Damage, CurrentLevel + 1);
            var visualType = (int)BaseObjectUpgradableDataConfiguration.CalculateDataForBuilding(BuildingType, BaseObjectDataType.VisualType, CurrentLevel + 1);
            data.Add(BaseObjectDataType.Damage, damage);
            data.Add(BaseObjectDataType.VisualType, visualType);
        }

        protected override void CalculateNextLevelSpecialBonusDataInner(Dictionary<BaseObjectSpecialBonusType, object> data)
        {
        }

        protected override void InitDataInner(BaseObjectUpgradableDataConfiguration baseObjectUpgradableDataConfiguration, int currentLevel)
        {
            Damage = (int)baseObjectUpgradableDataConfiguration.CalculateDataForBuilding(BuildingType, BaseObjectDataType.Damage, currentLevel);
            VisualType = (int)baseObjectUpgradableDataConfiguration.CalculateDataForBuilding(BuildingType, BaseObjectDataType.VisualType, currentLevel);
        }
    }
}