using Core.PlayerModule;
using Zenject;

namespace Core.LootChests
{
    public class ExtraWeaponUpgradeCardX2LootHandler : BaseChestLootHandler
    {
        private IWallet _wallet;
        public override ChestLootType LootType => ChestLootType.WeaponUpgradeCardX2;

        public override void Prepare(DiContainer diContainer)
        {
            base.Prepare(diContainer);
            _wallet = diContainer.Resolve<IWallet>();   
        }

        public override void HandleLoot()
        {
            var currentCards = _wallet.GetMoneyCount(MoneyType.ExtraCards);
            _wallet.AddMoney(MoneyType.ExtraCards, currentCards);
        }
    }
}