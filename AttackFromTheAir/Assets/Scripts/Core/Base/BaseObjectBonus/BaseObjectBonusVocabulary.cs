using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Core.LobbyBase
{
    [CreateAssetMenu(fileName =nameof(BaseObjectBonusVocabulary), menuName ="ScriptableObjects/"+nameof(BaseObjectBonusVocabulary))]
    public class BaseObjectBonusVocabulary : ScriptableObject
    {
        [SerializeField] private List<BaseObjectDataDescription> _baseObjectDataDescriptions;
        [SerializeField] private List<BaseObjectSpecialBonusDataDescription> _baseObjectSpecialBonusDataDescriptions;
        [SerializeField] private string _noSpecialBonusText;
        [SerializeField] private string _nextLevelValueDivider;
        [SerializeField] private List<BaseObjectDataFormat> _baseObjectDataFormats;
        [SerializeField] private List<BaseObjectSpecialBonusDataFormat> _baseObjectSpecialBonusDataFormats;
        public string NoSpecialBonusText => _noSpecialBonusText;
        public string NextLevelValueDivider => _nextLevelValueDivider;
        private Dictionary<BaseObjectDataType, string> _dataDescriptionDictionary;
        private Dictionary<BaseObjectSpecialBonusType, string> _specialBonusDescriptionDictionary;

        private Dictionary<BaseObjectDataType, string> _dataFormatsDict;
        private Dictionary<BaseObjectSpecialBonusType, string> _specialBonusDataFormatsDict;

        private void PrepareObjectDataDict()
        {
            if (_dataDescriptionDictionary == null)
            {
                _dataDescriptionDictionary = new Dictionary<BaseObjectDataType, string>();
                foreach (var description in _baseObjectDataDescriptions)
                {
                    _dataDescriptionDictionary.Add(description.DataType, description.Description);  
                }
            }
            if (_dataFormatsDict == null)
            {
                _dataFormatsDict = new Dictionary<BaseObjectDataType, string>();
                foreach (var dataFormat in _baseObjectDataFormats)
                {
                    _dataFormatsDict.Add(dataFormat.BaseObjectDataType, dataFormat.ValueFormat);
                }
            }
        }

        private void PrepareSpecialBonusDataDict()
        {
            if (_specialBonusDescriptionDictionary == null)
            {
                _specialBonusDescriptionDictionary = new Dictionary<BaseObjectSpecialBonusType, string>();
                foreach (var description in _baseObjectSpecialBonusDataDescriptions)
                {
                    _specialBonusDescriptionDictionary.Add(description.SpecialBonusType, description.Description);
                }
            }
            if (_specialBonusDataFormatsDict == null)
            {
                _specialBonusDataFormatsDict = new Dictionary<BaseObjectSpecialBonusType, string>();
                foreach (var dataFormat in _baseObjectSpecialBonusDataFormats)
                {
                    _specialBonusDataFormatsDict.Add(dataFormat.SpecialBonusType, dataFormat.ValueFormat);
                }
            }
        }

        public string GetObjectDataDescription(BaseObjectDataType dataType)
        {
            PrepareObjectDataDict();
            if (_dataDescriptionDictionary.TryGetValue(dataType, out var description))
            {
                return description;
            }
            else
            {
                throw new System.Exception($"Could not find description for data type {dataType}");
            }
        }

        public string GetSpecialBonusDataDescription(BaseObjectSpecialBonusType specialBonusType)
        {
            PrepareSpecialBonusDataDict();
            if (_specialBonusDescriptionDictionary.TryGetValue(specialBonusType, out var description))
            {
                return description;
            }
            else
            {
                throw new System.Exception($"Could not find description for special bonus data type {specialBonusType}");
            }
        }

        public string ProvideDataFormat(BaseObjectDataType dataType)
        {
            PrepareObjectDataDict();
            if (_dataFormatsDict.TryGetValue(dataType, out var format))
            {
                return format;
            }
            else
            {
                return "";
            }
        }

        public string ProvideSpecialBonusDataFormat(BaseObjectSpecialBonusType specialBonusType)
        {
            PrepareSpecialBonusDataDict();
            if (_specialBonusDataFormatsDict.TryGetValue(specialBonusType, out var format))
            {
                return format;
            }
            else
            {
                return "";
            }
        }
    }

    [System.Serializable]
    public class BaseObjectDataDescription
    {
        public BaseObjectDataType DataType;
        public string Description;
    }

    [System.Serializable]
    public class BaseObjectSpecialBonusDataDescription
    {
        public BaseObjectSpecialBonusType SpecialBonusType;
        public string Description;
    }

    [System.Serializable]
    public class BaseObjectDataFormat
    {
        public BaseObjectDataType BaseObjectDataType;
        public string ValueFormat;
    }

    [System.Serializable]
    public class BaseObjectSpecialBonusDataFormat
    {
        public BaseObjectSpecialBonusType SpecialBonusType;
        public string ValueFormat;
    }
}