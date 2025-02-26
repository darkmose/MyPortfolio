using Zenject;

namespace Core.LootChests
{
    public class PlayerExpirienceX2LootHandler : BaseChestLootHandler
    {
        public override ChestLootType LootType => ChestLootType.PlayerExpirienceX2;

        public override void Prepare(DiContainer diContainer)
        {
            base.Prepare(diContainer);
        }

        public override void HandleLoot()
        {

        }
    }
}