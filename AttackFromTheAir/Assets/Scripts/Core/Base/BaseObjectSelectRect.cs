using Core.UI;
using System;
using System.Globalization;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.LobbyBase
{
    public class BaseObjectSelectRect : MonoBehaviour, IDisposable
    {
        [SerializeField] private Image _rect;
        [SerializeField] private Sprite _activeSprite;
        [SerializeField] private Sprite _inactiveSprite;

        [Header("Object Level")]
        [SerializeField] private TextMeshProUGUI _objectLevel;
        [SerializeField] private GameObject _objectLevelPanel;

        [Header("Object UnlockProgress")]
        [SerializeField] private GameObject _progressPanel;
        [SerializeField] private TextMeshProUGUI _progress;
        [SerializeField] private Image _progressBar;

        [Header("Info")]
        [SerializeField] private Image _infoIcon;
        [SerializeField] private Button _infoPanelButton;
        [SerializeField] private GameObject _infoPanel;
        [SerializeField] private BaseObjectDataPanelView _objectDataPanelPrefab;
        [SerializeField] private BaseObjectDataPanelView _objectSpecialBonusDataPanelPrefab;
        [SerializeField] private Transform _objectDataRoot;
        [SerializeField] private Button _closeButton;
        [SerializeField] private TextMeshProUGUI _objectName;
        [SerializeField] private BaseObjectBonusVocabulary _baseObjectBonusVocabulary;

        private BaseObject _link;

        public void SetActiveSprite()
        {
            _rect.sprite = _activeSprite;
        }

        public void SetInactiveSprite()
        {
            _rect.sprite = _inactiveSprite;
        }
        
        public void LinkBaseObject(BaseObject obj)
        {
            _link = obj;
            if (_link.IsUnlocked.Value)
            {
                _progressPanel.SetActive(false);
                _infoIcon.gameObject.SetActive(true);
                _infoPanelButton.interactable = true;
            }
            else
            {
                _infoIcon.gameObject.SetActive(false);
                _infoPanelButton.interactable = false;
                _link.UnlockProgress.RegisterValueChangeListener(SetProgress);
                SetProgress(_link.UnlockProgress.Value);
            }
            _link.BaseObjectUpgradableData.InitVocabulary(_baseObjectBonusVocabulary);
            _link.UpgradableBuilding.Level.RegisterValueChangeListener(SetBuildingLevel);
            SetBuildingLevel(_link.UpgradableBuilding.Level.Value);
            _objectName.text = obj.UpgradableBuilding.BuildingType.ToString();
        }

        private void OnCloseButtonClick()
        {
            _infoPanel.gameObject.SetActive(false);
        }

        private void OnInfoButtonClick()
        {
            _infoPanel.gameObject.SetActive(true);
        }

        public void AddInfo(string info, bool isSpecialBonusData)
        {
            BaseObjectDataPanelView dataPanel = null;
            if (isSpecialBonusData)
            {
                dataPanel = Instantiate<BaseObjectDataPanelView>(_objectSpecialBonusDataPanelPrefab, _objectDataRoot);
            }
            else
            {
                dataPanel = Instantiate<BaseObjectDataPanelView>(_objectDataPanelPrefab, _objectDataRoot);
            }
            dataPanel.SetInfo(info);
        }

        public void ClearObjectData()
        {
            _objectDataRoot.ClearAllChild();
        }

        private void OnDisable()
        {
            _infoPanelButton.onClick.RemoveListener(OnInfoButtonClick);
            _closeButton.onClick.RemoveListener(OnCloseButtonClick);
        }

        private void OnEnable()
        {
            _infoPanelButton.onClick.AddListener(OnInfoButtonClick);
            _closeButton.onClick.AddListener(OnCloseButtonClick);
        }

        private void SetBuildingLevel(int level)
        {
            _objectLevel.text = level.ToString("LVL 0");
            ClearObjectData();
            CollectUpgradableData();
            CollectUpgradableSpecialBonusData();
        }

        private void CollectUpgradableData()
        {
            var objectDatas = _link.BaseObjectUpgradableData.GetData();
            var nextLevelObjectDatas = _link.BaseObjectUpgradableData.CalculateNextLevelData();

            string[] combinedDatas = new string[objectDatas.Count];
            int index = 0;
            foreach (var objData in objectDatas)
            {
                var combinedData = combinedDatas[index];

                var dataType = _baseObjectBonusVocabulary.GetObjectDataDescription(objData.Key);
                var dataFormat = _baseObjectBonusVocabulary.ProvideDataFormat(objData.Key);
                var dataValue = objData.Value.ToString() + dataFormat;
                combinedData = $"{dataType}: {dataValue}";
                combinedDatas[index] = combinedData;
                index++;
            }
            index = 0;
            foreach (var objData in nextLevelObjectDatas)
            {
                var combinedData = combinedDatas[index];
                var dataValue = objData.Value.ToString();
                var dataFormat = _baseObjectBonusVocabulary.ProvideDataFormat(objData.Key);
                combinedData += $"  {_baseObjectBonusVocabulary.NextLevelValueDivider}  {dataValue}" + dataFormat;
                combinedDatas[index] = combinedData;
                index++;
            }

            var info = new StringBuilder();
            info.AppendJoin("\n", combinedDatas);
            AddInfo(info.ToString(), false);
        }

        private void CollectUpgradableSpecialBonusData()
        {
            var objectDatas = _link.BaseObjectUpgradableData.GetSpecialBonusData();

            if (objectDatas.Count == 0)
            {
                AddInfo(_baseObjectBonusVocabulary.NoSpecialBonusText, false);
            }
            else
            {
                var nextLevelObjectDatas = _link.BaseObjectUpgradableData.CalculateNextLevelSpecialBonusData();
                string[] combinedDatas = new string[objectDatas.Count];
                int index = 0;
                foreach (var objData in objectDatas)
                {
                    var combinedData = combinedDatas[index];

                    var dataType = _baseObjectBonusVocabulary.GetSpecialBonusDataDescription(objData.Key);
                    var dataFormat = _baseObjectBonusVocabulary.ProvideSpecialBonusDataFormat(objData.Key);
                    var dataValue = objData.Value.ToString()+ dataFormat;
                    combinedData = $"{dataType}: {dataValue}";
                    combinedDatas[index] = combinedData;
                    index++;
                }
                index = 0;  
                foreach (var objData in nextLevelObjectDatas)
                {
                    var combinedData = combinedDatas[index];
                    var dataFormat = _baseObjectBonusVocabulary.ProvideSpecialBonusDataFormat(objData.Key);
                    var dataValue = objData.Value.ToString();
                    combinedData += $"  {_baseObjectBonusVocabulary.NextLevelValueDivider}  {dataValue}" + dataFormat;
                    combinedDatas[index] = combinedData;
                    index++;
                }

                var info = new StringBuilder();
                info.Append("<color=yellow>");
                info.AppendJoin("\n", combinedDatas);
                info.Append("</color>");
                AddInfo(info.ToString(), true);

            }
        }

        private void SetProgress(float progress)
        {
            if (progress == 1f)
            {
                _infoIcon.gameObject.SetActive(true);
                _infoPanelButton.interactable = true;
                _progressPanel.SetActive(false);
                return;
            }
            _progressBar.fillAmount = progress;
            _progress.text = progress.ToString("0.0%");
        }

        public void SetActiveObjectLevel(bool isActive)
        {
            _objectLevelPanel.SetActive(isActive);
        }

        public void Dispose()
        {
            _link.UnlockProgress.UnregisterValueChangeListener(SetProgress);
            _link.UpgradableBuilding.Level.UnregisterValueChangeListener(SetBuildingLevel);
        }
    }
}