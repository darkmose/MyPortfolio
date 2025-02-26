namespace Core.LootChests
{
    public interface ILootChestViewLink
    {
        LootChestView Link { get; }
        void LinkView(LootChestView link);
    }
}