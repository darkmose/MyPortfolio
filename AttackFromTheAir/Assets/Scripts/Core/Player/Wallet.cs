using System.Collections.Generic;
using Core.Storage;
using Newtonsoft.Json.Linq;
using System;
using Core.UI;
using Configuration;

namespace Core.PlayerModule
{
    public enum MoneyType
    {
        Coins,
        Gems,
        Hammers,
        Cards
    }

    public interface IWallet : IStoragableDictionary
    {
        int GetMoneyCount(MoneyType type);
        IntProperty GetMoneyProperty(MoneyType type);
        void AddMoney(MoneyType type, int amount);
        bool TryToSpend(MoneyType type, int amount);
        bool HasEnoughMoney(MoneyType type, int amount);
    }

    public class Wallet : IWallet
    {
        private Dictionary<MoneyType, IntProperty> _money = new Dictionary<MoneyType, IntProperty>();

        public Wallet(GameConfiguration gameConfiguration)
        {
            _money[MoneyType.Gems] = new IntProperty(0);
            _money[MoneyType.Coins] = new IntProperty(0);
            _money[MoneyType.Hammers] = new IntProperty(0);
            _money[MoneyType.Cards] = new IntProperty(0);

            foreach (var moneyConfig in gameConfiguration.WalletInitConfigs)
            {
                if (_money.ContainsKey(moneyConfig.MoneyType))
                {
                    _money[moneyConfig.MoneyType].SetValue(moneyConfig.InitAmount, true);
                }
            }
        }

        public void AddMoney(MoneyType type, int amount)
        {
            _money[type].SetValue(_money[type].Value + amount, true);
        }

        public int GetMoneyCount(MoneyType type)
        {
            return _money[type].Value;
        }

        public IntProperty GetMoneyProperty(MoneyType type)
        {
            return _money[type];
        }

        public bool HasEnoughMoney(MoneyType type, int amount)
        {
            if (_money.TryGetValue(type, out var money))
            {
                return money.Value >= amount;
            }
            else 
            {
                return false; 
            }
        }

        public void Init()
        {
        }

        public void Load(Dictionary<string, object> data)
        {
            if (data.ContainsKey(nameof(Wallet)))
            {
                var moneyData = (JObject)data[nameof(Wallet)];

                foreach (var money in moneyData)
                {                    
                    var type = (MoneyType)Enum.Parse(typeof(MoneyType),money.Key);
                    var amount = (int)money.Value;
                    _money[type].SetValue(amount, true);
                }
            }
        }

        public void Save(Dictionary<string, object> data)
        {
            var moneyData = new Dictionary<MoneyType, int>();
            foreach (var money in _money)
            {
                moneyData.Add(money.Key, money.Value.Value);
            }
            data[nameof(Wallet)] = moneyData;
        }

        public bool TryToSpend(MoneyType type, int amount)
        {
            if (_money[type].Value >= amount)
            {
                _money[type].SetValue(_money[type].Value - amount, true);
                return true;
            }
            else
            {
                return false;
            }
        }



    }
}