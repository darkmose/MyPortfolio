using Core.MVP;
using Zenject;

namespace Core.LootChests
{
    public class LevelRewardX3LootHandler : BaseChestLootHandler
    {
        private const float MULTIPLIER = 3f;
        private WinScreenModel _winScreenModel;
        public override ChestLootType LootType => ChestLootType.LevelRewardX3;

        public override void Prepare(DiContainer diContainer)
        {
            base.Prepare(diContainer);
            _winScreenModel = diContainer.Resolve<WinScreenModel>();
        }

        public override void HandleLoot()
        {
            _winScreenModel.MultiplyRewardBy(MULTIPLIER);
        }
    }

    
}