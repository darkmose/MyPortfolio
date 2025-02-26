using Core.MVP;
using Zenject;

namespace Core.LootChests
{
    public class LevelRewardX4LootHandler : BaseChestLootHandler
    {
        private const float MULTIPLIER = 4f;
        private WinScreenModel _winScreenModel;
        public override ChestLootType LootType => ChestLootType.LevelRewardX4;

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