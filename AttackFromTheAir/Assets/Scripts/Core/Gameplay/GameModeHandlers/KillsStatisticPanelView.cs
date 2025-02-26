using Core.Resourses;
using Core.UI;
using System;
using UnityEngine;
using Zenject;

namespace Core.GameLogic
{
    public class KillsStatisticPanelView : MonoBehaviour
    {
        [SerializeField] private StatisticPanelView _statisticPanelPrefab;
        [SerializeField] private BuildingSpriteProvider _buildingSpriteProvider;
        [SerializeField] private SpriteHolder _spriteHolder;
        [SerializeField] private Transform _root;
        private IPlayerStatisticCollector _statisticCollector;
        private IPlayerDroneSelector _droneSelector;

        private void Awake()
        {
            var di = DISimple.ServiceLocator.Resolve<DiContainer>();
            _statisticCollector = di.Resolve<IPlayerStatisticCollector>();
            _droneSelector = di.Resolve<IPlayerDroneSelector>();
        }

        private void OnEnable()
        {
            SetStatistic();
            _droneSelector.SelectedDrone.RegisterValueChangeListener(OnDroneChanged);
        }

        private void OnDroneChanged(PlayerDroneType type)
        {
            SetStatistic();
        }

        private void OnDisable()
        {
            Clear();
            _droneSelector.SelectedDrone.UnregisterValueChangeListener(OnDroneChanged);
        }

        private void SetStatistic()
        {
            Clear();
            var destroyedUnits = _statisticCollector.GetDestroyedUnitsStatistics();
            var destroyedBuildings = _statisticCollector.GetDestroyedBuildingStatistics();

            foreach (var item in destroyedUnits)
            {
                if (item.Value == 0)
                {
                    continue;
                }
                var unitSprite = _spriteHolder.GetUnitSprite(item.Key);
                var statisticPanelInstance = Instantiate(_statisticPanelPrefab, _root);
                statisticPanelInstance.SetIcon(unitSprite);
                statisticPanelInstance.SetValue(item.Value);
            }

            foreach (var item in destroyedBuildings)
            {
                if (item.Value == 0)
                {
                    continue;
                }
                var buildingSprite = _buildingSpriteProvider.ProvideByType(item.Key);
                var statisticPanelInstance = Instantiate(_statisticPanelPrefab, _root);
                statisticPanelInstance.SetIcon(buildingSprite);
                statisticPanelInstance.SetValue(item.Value);
            }
        }

        private void Clear()
        {
            _root.ClearAllChild();
        }
    }
}