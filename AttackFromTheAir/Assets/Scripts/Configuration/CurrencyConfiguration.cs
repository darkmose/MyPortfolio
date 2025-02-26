using Core.PlayerModule;
using System.Collections.Generic;
using UnityEngine;

namespace Configuration
{
    [CreateAssetMenu(fileName =nameof(CurrencyConfiguration), menuName = "ScriptableObjects/"+nameof(CurrencyConfiguration))]
    public class CurrencyConfiguration : ScriptableObject
    {
        private const string CURRENCY_DEFAULT_FORMAT = "0";
        [SerializeField] private List<CurrencyFormatDescriptor> _textFormats;
        private Dictionary<MoneyType, string> _currencyFormatsDict;

        private void PrepareDictionary()
        {
            if (_currencyFormatsDict == null)
            {
                _currencyFormatsDict = new Dictionary<MoneyType, string>();
                foreach (var format in _textFormats)
                {
                    _currencyFormatsDict.Add(format.MoneyType, format.CurrencyFormat);
                }
            }
        }

        public string ProvideCurrencyFormat(MoneyType moneyType)
        {
            PrepareDictionary();
            if (_currencyFormatsDict.TryGetValue(moneyType, out var format))
            {
                return format;
            }
            else 
            { 
                return CURRENCY_DEFAULT_FORMAT; 
            }
        }
    }

    [System.Serializable]
    public class CurrencyFormatDescriptor
    {
        public MoneyType MoneyType;
        [Tooltip("Format must looks like '0$' where '$' part is format string and '0' is dynamic actual value.")]
        public string CurrencyFormat;
    }
}