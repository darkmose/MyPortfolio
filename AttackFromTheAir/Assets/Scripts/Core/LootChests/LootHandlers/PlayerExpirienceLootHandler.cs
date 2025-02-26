using Configuration;
using Core.MVP;
using Core.PlayerModule;
using UnityEngine;
using Zenject;

namespace Core.LootChests
{
    public class PlayerExpirienceLootHandler : BaseChestLootHandler
    {
        private IWallet _wallet;
        private LootChestsConfiguration _lootChestsConfiguration;
        private WinScreenModel _winScreenModel;
        public override ChestLootType LootType => ChestLootType.PlayerExpirience;

        public override void Prepare(DiContainer diContainer)
        {
            base.Prepare(diContainer);
            _lootChestsConfiguration = Resources.Load<LootChestsConfiguration>("ScriptableObjects/" + nameof(LootChestsConfiguration));
            _winScreenModel = diContainer.Resolve<WinScreenModel>();
        }

        public override void HandleLoot()
        {
            var amount = _lootChestsConfiguration.GetLootAmount(LootType);
            _winScreenModel.PlayerExperienceAnimationHelper.AddExperience(amount);
        }
    }
}