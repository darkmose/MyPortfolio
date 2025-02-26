using Core.MVP;
using Zenject;

namespace Core.LootChests
{
    public class LevelRewardX2LootHandler : BaseChestLootHandler
    {
        private const float MULTIPLIER = 2f;
        private WinScreenModel _winScreenModel;
        public override ChestLootType LootType => ChestLootType.LevelRewardX2;

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