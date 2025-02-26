using Zenject;

namespace Core.LootChests
{
    public abstract class BaseChestLootHandler
    {
        public abstract ChestLootType LootType { get; }
        public virtual void Prepare(DiContainer diContainer)
        {
        }
        public abstract void HandleLoot();
    }
}