using Core.GameLogic;

namespace Core.Buildings
{
    public class SimpleStorageBuildingView : BaseStorageBuildingView
    {
        public override StorageBuildingType StorageBuildingType => StorageBuildingType.SimpleStorage;

        public override void OnLevelChange(int level)
        {
            throw new System.NotImplementedException();
        }

        public override void OnObjectDamaged(IDamagableObject damagableObject)
        {
            throw new System.NotImplementedException();
        }

        public override void OnObjectDestroy(IDamagableObject damagableObject)
        {
            throw new System.NotImplementedException();
        }

        public override void SetHealthNormalized(float health)
        {
            throw new System.NotImplementedException();
        }
    }
}