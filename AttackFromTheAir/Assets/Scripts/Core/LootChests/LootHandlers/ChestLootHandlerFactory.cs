using System.Collections.Generic;
using Zenject;

namespace Core.LootChests
{
    public class ChestLootHandlerFactory
    {
        private static DiContainer _diContainer;
        private static ChestLootHandlerFactory _instance;
        private static Dictionary<ChestLootType, BaseChestLootHandler> _handlersDictionary;

        public ChestLootHandlerFactory(DiContainer diContainer)
        {
            _instance = this;
            _diContainer = diContainer;
            _handlersDictionary = new Dictionary<ChestLootType, BaseChestLootHandler>();
        }

        public static BaseChestLootHandler Create(ChestLootType lootType)
        {
            if (_handlersDictionary.TryGetValue(lootType, out var cachedHandler))
            {
                return cachedHandler;
            }
            else
            {
                BaseChestLootHandler handler = null;

                switch (lootType)
                {
                    case ChestLootType.SoftCurrency:
                        handler = new SoftCurrencyLootHandler();
                        break;
                    case ChestLootType.PlayerExpirience:
                        handler = new PlayerExpirienceLootHandler();
                        break;
                    case ChestLootType.WeaponUpgradeCard:
                        handler = new WeaponUpgradeCardLootHandler();
                        break;
                    case ChestLootType.ExtraWeaponUpgradeCard:
                        handler = new ExtraWeaponUpgradeCardLootHandler();
                        break;
                    case ChestLootType.BaseUpgradeCurrency:
                        handler = new BaseUpgradeCurrencyLootHandler();
                        break;
                    case ChestLootType.SoftCurrencyX2:
                        handler = new SoftCurrencyX2LootHandler();
                        break;
                    case ChestLootType.PlayerExpirienceX2:
                        handler = new PlayerExpirienceX2LootHandler();
                        break;
                    case ChestLootType.WeaponUpgradeCardX2:
                        handler = new WeaponUpgradeCardX2LootHandler();
                        break;
                    case ChestLootType.SoftCurrencyX3:
                        handler = new SoftCurrencyX3LootHandler();
                        break;
                    case ChestLootType.PlayerExpirienceX3:
                        handler = new PlayerExpirienceX3LootHandler();
                        break;
                    case ChestLootType.ExtraWeaponUpgradeCardX2:
                        handler = new ExtraWeaponUpgradeCardX2LootHandler();
                        break;
                    case ChestLootType.HardCurrency:
                        handler = new HardCurrencyLootHandler();
                        break;
                    case ChestLootType.BaseUpgradeCurrencyX2:
                        handler = new BaseUpgradeCurrencyX2LootHandler();
                        break;
                    case ChestLootType.Bomb:
                        handler = new BombLootHandler();
                        break;
                    case ChestLootType.BombDefuser:
                        handler = new BombDeffuserLootHandler();
                        break;
                    case ChestLootType.LevelRewardX2:
                        handler = new LevelRewardX2LootHandler();
                        break;
                    case ChestLootType.LevelRewardX3:
                        handler = new LevelRewardX3LootHandler();
                        break;
                    case ChestLootType.LevelRewardX4:
                        handler = new LevelRewardX4LootHandler();
                        break;
                    case ChestLootType.LevelRewardX5:
                        handler = new LevelRewardX5LootHandler();
                        break;
                    case ChestLootType.LevelRewardX10:
                        handler = new LevelRewardX10LootHandler();
                        break;
                    default:
                        handler = new BombLootHandler();
                        break;
                }
                handler.Prepare(_diContainer);
                _handlersDictionary.Add(lootType, handler);
                return handler;
            }

        }
    }

    
}