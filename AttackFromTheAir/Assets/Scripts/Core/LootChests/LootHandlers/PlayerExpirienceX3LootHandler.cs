using Zenject;

namespace Core.LootChests
{
    public class PlayerExpirienceX3LootHandler : BaseChestLootHandler
    {
        public override ChestLootType LootType => ChestLootType.PlayerExpirienceX3;

        public override void Prepare(DiContainer diContainer)
        {
            base.Prepare(diContainer);
        }

        public override void HandleLoot()
        {

        }
    }
}