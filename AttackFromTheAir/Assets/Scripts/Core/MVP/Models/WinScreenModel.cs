using System.Collections.Generic;
using Core.Buildings;
using Core.GameLogic;
using Core.Level;
using Core.LobbyBase;
using Core.PlayerModule;
using Core.Resourses;
using Core.UI;
using UnityEngine;

namespace Core.MVP
{
    public class WinScreenModel : IModel
    {
        private const int MULTIPLY_BUTTON_MULTIPLIER = 2;

        private readonly FloatProperty _levelCompletionProgress = new FloatProperty();
        private readonly ListProperty<PanelRewardData> _rewardPanelList = new ListProperty<PanelRewardData>();
        private readonly ListProperty<PanelRewardData> _multipliedRewardPanelList = new ListProperty<PanelRewardData>();
        private readonly ILevelProgressionHelper _levelProgressionHelper;
        private readonly IPlayerExpirienceService _playerExperience;
        private readonly IWallet _wallet;
        private readonly SpriteHolder _spriteHolder;
        private readonly BuildingSpriteProvider _buildingSpriteProvider;
        private readonly ExperienceHolder _experienceHolder;
        private readonly LevelRewardConfig _levelRewardConfig;
        private readonly PlayerExperienceAnimationHelper _playerExperienceAnimationHelper;
        private readonly PlayerExperienceAnimationHelper _baseExperienceAnimationHelper;
        private readonly WinScreenProxyView _winScreenProxyView;
        private readonly List<PanelRewardData> _panelRewardDates = new List<PanelRewardData>();
        private readonly List<PanelRewardData> _panelRewardMyltiplueDates = new List<PanelRewardData>();
        private readonly IPlayerStatisticCollector _playerStatisticCollector;
        private float _baseExperienceValue = 0;
        private float _playerExperienceValue = 0;
        private int _xpPlayer = 0;
        private int _xpBas = 0;
        private int _hammer = 0;
        private int _coins = 0;
        private float _rewardMultiplier = 1f;
        private BaseObjectsSpecialBonusHandler _baseObjectsSpecialBonusHandler;
        public IPropertyReadOnly<int> CurentLevel { get; }
        public IPropertyReadOnly<List<PanelRewardData>> RewardPanelList => _rewardPanelList;
        public IPropertyReadOnly<List<PanelRewardData>> MultipliedRewardPanelList => _multipliedRewardPanelList;
        public IPropertyReadOnly<float> LevelCompletionProgress => _levelCompletionProgress;
        public PlayerExperienceAnimationHelper PlayerExperienceAnimationHelper => _playerExperienceAnimationHelper;
        public PlayerExperienceAnimationHelper BaseExperienceAnimationHelper => _baseExperienceAnimationHelper;

        public WinScreenModel(IPlayerExpirienceService playerExpirienceService, IWallet wallet, LevelSelector levelSelector,
        ILevelProgressionHelper iLevelProgressionHelper, SpriteHolder spriteHolder, ExperienceHolder experienceHolder, LevelRewardConfig levelRewardConfig, 
        WinScreenProxyView winScreenProxyView, BaseObjectsSpecialBonusHandler baseObjectsSpecialBonusHandler, BuildingSpriteProvider buildingSpriteProvider,
        IPlayerStatisticCollector playerStatisticCollector)
        {
            CurentLevel = levelSelector.CurrentLevel;
            _playerExperience = playerExpirienceService;
            _levelProgressionHelper = iLevelProgressionHelper;
            _spriteHolder = spriteHolder;
            _experienceHolder = experienceHolder;
            _levelRewardConfig = levelRewardConfig;
            _wallet = wallet;
            _winScreenProxyView = winScreenProxyView;
            _baseObjectsSpecialBonusHandler = baseObjectsSpecialBonusHandler;

            _playerExperienceAnimationHelper = new PlayerExperienceAnimationHelper(playerExpirienceService, experienceHolder, ExperienceType.PlayerExp);
            _baseExperienceAnimationHelper = new PlayerExperienceAnimationHelper(playerExpirienceService, experienceHolder, ExperienceType.BaseExp);
            _buildingSpriteProvider = buildingSpriteProvider;
            _playerStatisticCollector = playerStatisticCollector;
        }

        public void MultiplyRewardBy(float multiplier)
        {
            _rewardMultiplier *= multiplier;

            _playerExperienceAnimationHelper.MultiplyExperience(multiplier);
            _playerExperience.SetPlayerLevel(_playerExperienceAnimationHelper.CurrentLevel);
            _playerExperience.SetPlayerExperience(_playerExperienceAnimationHelper.CurrentExperienceValue);
            _playerExperience.SetBaseExperiencePercentage(_playerExperienceAnimationHelper.CurrentExperiencePercentageValue);

            _baseExperienceAnimationHelper.MultiplyExperience(multiplier);
            _playerExperience.SetBaseLevel(_baseExperienceAnimationHelper.CurrentLevel);
            _playerExperience.SetBaseExperience(_baseExperienceAnimationHelper.CurrentExperienceValue);
            _playerExperience.SetBaseExperiencePercentage(_baseExperienceAnimationHelper.CurrentExperiencePercentageValue);

            GenerateReward();
        }

        public void GenerateLevelCompletionDetails()
        {
            _rewardMultiplier = 1f;
            _playerExperienceAnimationHelper.LinkView(_winScreenProxyView.View.PlayerExperienceAnimationHelper);
            _playerExperienceAnimationHelper.Init();
            _baseExperienceAnimationHelper.LinkView(_winScreenProxyView.View.BaseExperienceAnimationHelper);
            _baseExperienceAnimationHelper.Init();

            GenerationLevelDestoy();
            GenerationPlayerLevel(); 
            GenerationBassLevel();
            GenerateReward();
        }

        public List<PanelDestroyData> GetPanelDestroyDataList()
        {
            var unitDestroy = _levelProgressionHelper.UnitDestroyDictionary;
            var buildingDestroy = _levelProgressionHelper.BuildingDestroyDictionary;

            List<PanelDestroyData> panelDestroyData = new List<PanelDestroyData>();
            PanelDestroyData panelDestroy;

            var destroyedUnitsStatistic = _playerStatisticCollector.GetDestroyedUnitsStatistics();
            var destroyedBuildingsStatistic = _playerStatisticCollector.GetDestroyedBuildingStatistics();

            foreach (var unitItem in unitDestroy)
            {
                if (unitItem.Value == 0)
                {
                    continue;
                }
                panelDestroy = new PanelDestroyData();
                panelDestroy.IconUnitDestroy = _spriteHolder.GetUnitSprite(unitItem.Key);
                panelDestroyData.Add(panelDestroy);

                if (destroyedUnitsStatistic[unitItem.Key] == unitItem.Value)
                {
                    panelDestroy.CountUnitDestroyText = unitItem.Value.ToString();
                }
                else
                {
                    var format = $"<color=white>{destroyedUnitsStatistic[unitItem.Key]}</color><color=green> +{unitItem.Value}</color>";
                    panelDestroy.CountUnitDestroyText = format;
                }

            }
            foreach (var buildingItem in buildingDestroy)
            {
                if (buildingItem.Value == 0)
                {
                    continue;
                }
                panelDestroy = new PanelDestroyData();
                panelDestroy.IconUnitDestroy = _buildingSpriteProvider.ProvideByType(buildingItem.Key);
                panelDestroyData.Add(panelDestroy);

                if (destroyedBuildingsStatistic[buildingItem.Key] == buildingItem.Value)
                {
                    panelDestroy.CountUnitDestroyText = buildingItem.Value.ToString();
                }
                else
                {
                    var format = $"<color=white>{destroyedBuildingsStatistic[buildingItem.Key]}</color><color=green> +{buildingItem.Value}</color>";
                    panelDestroy.CountUnitDestroyText = format;
                }
            }
            

            return panelDestroyData;
        }

        private void GenerationPlayerLevel()
        {
            var unitDestroy = _levelProgressionHelper.UnitDestroyDictionary;
            var buildingDestroy = _levelProgressionHelper.BuildingDestroyDictionary;
            _playerExperienceValue = 0;

            foreach (var unit in unitDestroy)
            {
                if (unit.Value == 0)
                {
                    continue;
                }
                int prewResult = _experienceHolder.GetUnitExperience(unit.Key, ExperienceType.PlayerExp);

                int finishResult = prewResult * unit.Value;
                _playerExperienceValue += finishResult;
            }
            foreach (var building in buildingDestroy)
            {
                if (building.Value == 0)
                {
                    continue;
                }
                int prewResult = _experienceHolder.GetBuildingExperience(building.Key, ExperienceType.PlayerExp);

                int finishResult = prewResult * building.Value;
                _playerExperienceValue += finishResult;
            }

            _playerExperienceAnimationHelper.AddExperience((int)_playerExperienceValue);
            _playerExperience.SetPlayerLevel(_playerExperienceAnimationHelper.CurrentLevel);
            _playerExperience.SetPlayerExperience(_playerExperienceAnimationHelper.CurrentExperienceValue);
            _playerExperience.SetPlayerExperiencePercentage(_playerExperienceAnimationHelper.CurrentExperiencePercentageValue);

        }

        private void GenerationBassLevel()
        {
            var unitDestroy = _levelProgressionHelper.UnitDestroyDictionary;
            var buildingDestroy = _levelProgressionHelper.BuildingDestroyDictionary;
            _baseExperienceValue = 0;

            foreach (var unit in unitDestroy)
            {
                if (unit.Value == 0)
                {
                    continue;
                }
                int prewResult = _experienceHolder.GetUnitExperience(unit.Key, ExperienceType.BaseExp);
                int finishResult = prewResult * unit.Value;
                _baseExperienceValue += finishResult;
            }
            foreach (var building in buildingDestroy)
            {
                if (building.Value == 0)
                {
                    continue;
                }
                int prewResult = _experienceHolder.GetBuildingExperience(building.Key, ExperienceType.BaseExp);
                int finishResult = prewResult * building.Value;
                _baseExperienceValue += finishResult;
            }

            _baseExperienceAnimationHelper.AddExperience((int)_baseExperienceValue);
            _playerExperience.SetBaseLevel(_baseExperienceAnimationHelper.CurrentLevel);
            _playerExperience.SetBaseExperience(_baseExperienceAnimationHelper.CurrentExperienceValue);
            _playerExperience.SetBaseExperiencePercentage(_baseExperienceAnimationHelper.CurrentExperiencePercentageValue);
        }

        private void GenerationLevelDestoy()
        {
            _levelCompletionProgress.SetValue(_levelProgressionHelper.MissionCompletionProgress, false);
        }

        private void GenerateReward()
        {
            _xpPlayer = _playerExperienceAnimationHelper.AdditionalExpirienceValue;
            _xpBas = _baseExperienceAnimationHelper.AdditionalExpirienceValue;
            _hammer = _levelRewardConfig.GenerateGrearsRewards(CurentLevel.Value);
            _coins = _levelRewardConfig.GenerateCoinsRewards(CurentLevel.Value);
            if (_baseObjectsSpecialBonusHandler.TryGetSpecialBonusData(BaseObjectSpecialBonusType.AdditionalMoneyReward, out var specialBonusData))
            {
                _coins += (int)specialBonusData;
            }

            GenerateLevelReward(_xpPlayer, _xpBas, _hammer, _coins);
            GenerateMyltiplReward(_xpPlayer, _xpBas, _hammer, _coins);
        }

        private void GenerateLevelReward(int xpPlayer, int xpBas, int hammer, int coins)
        {
            Debug.Log("[GenerateLevelReward] Reward Multiplier = "+_rewardMultiplier.ToString());
            _panelRewardDates.Clear();

            AddRewardToPanel(ResourceType.XP, xpPlayer, _panelRewardDates);
            AddRewardToPanel(ResourceType.XPBase, xpBas, _panelRewardDates);
            AddRewardToPanel(ResourceType.Hammers, hammer * (int)_rewardMultiplier, _panelRewardDates);
            AddRewardToPanel(ResourceType.Coins, coins * (int)_rewardMultiplier, _panelRewardDates);

            _rewardPanelList.Notify(_panelRewardDates);
        }
        private void GenerateMyltiplReward(int xpPlayer, int xpBas, int hammer, int coins)
        {
            _panelRewardMyltiplueDates.Clear();

            int xpX2 = (xpPlayer + xpBas) * MULTIPLY_BUTTON_MULTIPLIER * (int)_rewardMultiplier;
            int hammerX2 = hammer * MULTIPLY_BUTTON_MULTIPLIER * (int)_rewardMultiplier;
            int CoinsX2 = coins * MULTIPLY_BUTTON_MULTIPLIER * (int)_rewardMultiplier;

            AddRewardToPanel(ResourceType.XP, xpX2, _panelRewardMyltiplueDates);
            AddRewardToPanel(ResourceType.Hammers, hammerX2, _panelRewardMyltiplueDates);
            AddRewardToPanel(ResourceType.Coins, CoinsX2, _panelRewardMyltiplueDates);

            _multipliedRewardPanelList.Notify(_panelRewardMyltiplueDates);
        }

        private void AddRewardToPanel(ResourceType resourceType, int rewardCount, List<PanelRewardData> rewardDates)
        {
            PanelRewardData date = new PanelRewardData();
            date.IconReward = _spriteHolder.GetResourceSprite(resourceType);
            date.CountReward = rewardCount;
            rewardDates.Add(date);
        }

        public void AdvertisementMultipliedReward() 
        {
            _wallet.AddMoney(MoneyType.Coins, _coins * MULTIPLY_BUTTON_MULTIPLIER * (int)_rewardMultiplier);
            _wallet.AddMoney(MoneyType.Hammers, _hammer * MULTIPLY_BUTTON_MULTIPLIER * (int)_rewardMultiplier);

            _playerExperienceAnimationHelper.MultiplyExperience(MULTIPLY_BUTTON_MULTIPLIER);
            _playerExperience.SetPlayerLevel(_playerExperienceAnimationHelper.CurrentLevel);
            _playerExperience.SetPlayerExperience(_playerExperienceAnimationHelper.CurrentExperienceValue);
            _playerExperience.SetPlayerExperiencePercentage(_playerExperienceAnimationHelper.CurrentExperiencePercentageValue);

            _baseExperienceAnimationHelper.MultiplyExperience(MULTIPLY_BUTTON_MULTIPLIER);
            _playerExperience.SetBaseLevel(_baseExperienceAnimationHelper.CurrentLevel);
            _playerExperience.SetBaseExperience(_baseExperienceAnimationHelper.CurrentExperienceValue);
            _playerExperience.SetBaseExperiencePercentage(_baseExperienceAnimationHelper.CurrentExperiencePercentageValue);

            _rewardMultiplier *= 2f;
            GenerateLevelReward(_xpPlayer, _xpBas, _hammer, _coins);
            Debug.Log(_playerExperience.ToString());
            Debug.Log(_playerExperienceAnimationHelper.ToString());
            Debug.Log(_baseExperienceAnimationHelper.ToString());
        }
    }

    public class PanelDestroyData
    {
        public Sprite IconUnitDestroy;
        public string CountUnitDestroyText;
    }

    public class PanelRewardData
    {
        public ResourceType Type;
        public Sprite IconReward;
        public int CountReward;
    }
}