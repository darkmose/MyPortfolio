using Core.MVP;
using Zenject;

namespace Core.LootChests
{
    public class LevelRewardX10LootHandler : BaseChestLootHandler
    {
        private const float MULTIPLIER = 10f;
        private WinScreenModel _winScreenModel;
        public override ChestLootType LootType => ChestLootType.LevelRewardX10;

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