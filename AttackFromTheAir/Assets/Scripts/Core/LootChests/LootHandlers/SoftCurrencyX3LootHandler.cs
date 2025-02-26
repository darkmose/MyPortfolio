using Core.PlayerModule;
using Zenject;

namespace Core.LootChests
{
    public class SoftCurrencyX3LootHandler : BaseChestLootHandler
    {
        private IWallet _wallet;
        public override ChestLootType LootType => ChestLootType.SoftCurrencyX3;

        public override void Prepare(DiContainer diContainer)
        {
            base.Prepare(diContainer);
            _wallet = diContainer.Resolve<IWallet>();   
        }

        public override void HandleLoot()
        {
            var currentMoney = _wallet.GetMoneyCount(MoneyType.Coins);
            _wallet.AddMoney(MoneyType.Coins, currentMoney * 2);
        }
    }
}