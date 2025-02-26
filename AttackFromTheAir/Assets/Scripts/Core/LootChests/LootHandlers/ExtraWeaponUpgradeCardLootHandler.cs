using Configuration;
using Core.PlayerModule;
using UnityEngine;
using Zenject;

namespace Core.LootChests
{
    public class ExtraWeaponUpgradeCardLootHandler : BaseChestLootHandler
    {
        private IWallet _wallet;
        private LootChestsConfiguration _lootChestsConfiguration;
        public override ChestLootType LootType => ChestLootType.ExtraWeaponUpgradeCard;

        public override void Prepare(DiContainer diContainer)
        {
            base.Prepare(diContainer);
            _wallet = diContainer.Resolve<IWallet>();
            _lootChestsConfiguration = Resources.Load<LootChestsConfiguration>("ScriptableObjects/" + nameof(LootChestsConfiguration));
        }

        public override void HandleLoot()
        {
            var amount = _lootChestsConfiguration.GetLootAmount(LootType);
            _wallet.AddMoney(MoneyType.ExtraCards ,amount);       
        }
    }
}