using Configuration;
using Core.PlayerModule;
using UnityEngine;
using Zenject;

namespace Core.LootChests
{
    public class WeaponUpgradeCardLootHandler : BaseChestLootHandler
    {
        private IWallet _wallet;
        private LootChestsConfiguration _lootChestsConfiguration;
        public override ChestLootType LootType => ChestLootType.WeaponUpgradeCard;

        public override void Prepare(DiContainer diContainer)
        {
            base.Prepare(diContainer);
            _wallet = diContainer.Resolve<IWallet>();
            _lootChestsConfiguration = Resources.Load<LootChestsConfiguration>("ScriptableObjects/" + nameof(LootChestsConfiguration));
        }

        public override void HandleLoot()
        {
            var amount = _lootChestsConfiguration.GetLootAmount(LootType);
            _wallet.AddMoney(MoneyType.Cards ,amount);       
        }
    }
}