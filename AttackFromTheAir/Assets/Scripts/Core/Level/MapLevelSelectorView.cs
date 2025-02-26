using Core.Tools;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Level
{
    public class MapLevelSelectorView : MonoBehaviour
    {
        [SerializeField] private int _bakedMarkersAmount = 50;
        [SerializeField] private RectTransform _scrollViewContent;
        [SerializeField] private MapLevelViewsGenerator _mapLevelViewsGenerator;
        private List<MapLevelView> _levels;
        private MapLevelSelector _model;

        public void LinkModel(MapLevelSelector model)
        {
            _model = model;
            _levels = new List<MapLevelView>();
            _mapLevelViewsGenerator.Init();
            var modelMapLevels = model.MapLevels;
            for (int i = 0; i < _bakedMarkersAmount; i++)
            {
                var mapLevelView = _mapLevelViewsGenerator.GenerateMapLevel();

                if (i < modelMapLevels.Count)
                {
                    var mapLevel = modelMapLevels[i];

                    mapLevel.IsAvailable.RegisterValueChangeListener(mapLevelView.SetAvailable);
                    mapLevel.IsFinished.RegisterValueChangeListener(mapLevelView.SetFinished);
                    mapLevel.SelectEvent.AddListener(mapLevelView.SetSelected);

                    mapLevelView.SetAvailable(mapLevel.IsAvailable.Value);
                    mapLevelView.SetFinished(mapLevel.IsFinished.Value);
                    mapLevelView.SetLevelNumber(i + 1);
                    mapLevelView.ClickEvent.AddListener(mapLevel.OnMapLevelClick);
                }
                else
                {
                    mapLevelView.SetLevelNumber(i + 1);
                    mapLevelView.SetLockedStatus();
                }
                _levels.Add(mapLevelView);
            }
        }

        public void FocusOnLevel(int level)
        {
            var mapLevel = _levels[level - 1];
            var rootPos = _scrollViewContent.position;
            var centerX = Screen.width / 2f;
            var posXDelta = mapLevel.transform.position.x - centerX;
            rootPos.x -= posXDelta;
            _scrollViewContent.position = rootPos;
        }
    }
}