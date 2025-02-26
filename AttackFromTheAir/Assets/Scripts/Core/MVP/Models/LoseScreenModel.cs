using System;
using System.Collections.Generic;
using System.Linq;
using Core.Buildings;
using Core.GameLogic;
using Core.Level;
using Core.PlayerModule;
using Core.Resourses;
using Core.UI;
using Core.Units;
using UnityEngine;

namespace Core.MVP
{
    public class LoseScreenModel : IModel
    {
        private ILevelProgressionHelper _levelProgressionHelper;
        private IPlayerExpirienceService _playerExperience;
        private SpriteHolder _spriteHolder;
        private ExperienceHolder _experienceHolder;
        private LevelRewardConfig _levelRewardConfig;
        private BuildingSpriteProvider _buildingSpriteProvider;
        private float _baseExperienceValue = 0;
        private float _playerExperienceValue = 0;
        private PlayerExperienceAnimationHelper _playerExperienceAnimationHelper;
        private PlayerExperienceAnimationHelper _baseExperienceAnimationHelper;
        private LoseScreenProxyView _loseScreenProxyView;
        private IPlayerStatisticCollector _playerStatisticCollector;
        public IPropertyReadOnly<int> CurentLevel { get; }
        public List<PanelRewardData> PanelRewardDates = new List<PanelRewardData>();
        public List<PanelRewardData> PanelRewardMyltiplueDates = new List<PanelRewardData>();

        public LoseScreenModel(IPlayerExpirienceService playerExpirienceService, IWallet wallet, LevelSelector levelSelector,
        ILevelProgressionHelper iLevelProgressionHelper, SpriteHolder spriteHolder, ExperienceHolder experienceHolder, LevelRewardConfig levelRewardConfig,
        LoseScreenProxyView loseScreenProxyView, BuildingSpriteProvider buildingSpriteProvider, IPlayerStatisticCollector playerStatisticCollector)
        {
            CurentLevel = levelSelector.CurrentLevel;
            _playerExperience = playerExpirienceService;
            _levelProgressionHelper = iLevelProgressionHelper;
            _spriteHolder = spriteHolder;
            _experienceHolder = experienceHolder;
            _levelRewardConfig = levelRewardConfig;
            _loseScreenProxyView = loseScreenProxyView;

            _playerExperienceAnimationHelper = new PlayerExperienceAnimationHelper(playerExpirienceService, experienceHolder, ExperienceType.PlayerExp);
            _baseExperienceAnimationHelper = new PlayerExperienceAnimationHelper(playerExpirienceService, experienceHolder, ExperienceType.BaseExp);
            _buildingSpriteProvider = buildingSpriteProvider;
            _playerStatisticCollector = playerStatisticCollector;
        }


        public void GenerateLevelCompletionDetails()
        {
            _playerExperienceAnimationHelper.LinkView(_loseScreenProxyView.View.PlayerExperienceAnimationHelper);
            _playerExperienceAnimationHelper.Init();

            _baseExperienceAnimationHelper.LinkView(_loseScreenProxyView.View.BaseExperienceAnimationHelper);
            _baseExperienceAnimationHelper.Init();

            GenerationPlayerLevel(); 
            GenerationBassLevel();
            GenerateLevelReward();  
        }

        public void GenerationPlayerLevel()
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

        public void GenerationBassLevel()
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

        public void GenerateLevelReward()
        {
            PanelRewardDates.Clear();
            AddRewardToPanel(ResourceType.XP, (int)_playerExperienceValue, PanelRewardDates);
            AddRewardToPanel(ResourceType.XPBase, (int)_baseExperienceValue, PanelRewardDates);
            AddRewardToPanel(ResourceType.Hammers, _levelRewardConfig.GenerateGrearsRewards(CurentLevel.Value), PanelRewardDates);
            AddRewardToPanel(ResourceType.Coins, _levelRewardConfig.GenerateCoinsRewards(CurentLevel.Value), PanelRewardDates);
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

        private void AddRewardToPanel(ResourceType resourceType, int rewardCount,List<PanelRewardData> rewardDates)
        {
            PanelRewardData date = new PanelRewardData();
            date.IconReward = _spriteHolder.GetResourceSprite(resourceType);
            date.CountReward = rewardCount;
            rewardDates.Add(date);
        }
    }
}