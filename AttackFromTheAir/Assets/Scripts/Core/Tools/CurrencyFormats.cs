using Configuration;
using Core.PlayerModule;
using UnityEngine;

namespace Core.Utilities
{
    public class CurrencyFormats
    {
        private CurrencyConfiguration _currencyConfiguration;
        private static CurrencyFormats _innerInstance;

        public CurrencyFormats()
        {
            _innerInstance = this;
            _currencyConfiguration = Resources.Load<CurrencyConfiguration>("ScriptableObjects/"+nameof(CurrencyConfiguration));
        }

        public static string ProvideCurrencyFormat(MoneyType moneyType)
        {
            return _innerInstance._currencyConfiguration.ProvideCurrencyFormat(moneyType);    
        }
    }
}