using Core.Level;
using System;
using UnityEngine;

namespace Core.Tools
{
    public class MapLevelViewsGenerator : MonoBehaviour
    {
        private const float PADDING_X = 100f;
        [SerializeField] private string _randomSeed;
        [SerializeField] private MapLevelView _prefab;
        [SerializeField] private RectTransform _root;
        [SerializeField] private float _mapLevelSpacing;
        [SerializeField] private float _minPosY;
        [SerializeField] private float _maxPosY;
        private System.Random _random;
        private int _mapLevelsGenerated;
        private float _startPosX;

        public void Init()
        {
            _random = new System.Random(_randomSeed.GetHashCode());
            CalculateWidthAndHeight();
        }

        private void CalculateWidthAndHeight()
        {
            var rootSize = _root.rect.size;
            _startPosX = (-rootSize.x / 2f) + PADDING_X;
        }

        public MapLevelView GenerateMapLevel()
        {
            var posY = Mathf.Lerp(_minPosY, _maxPosY, (float)_random.NextDouble());
            var instance = Instantiate(_prefab, _root);
            var posX = _startPosX + _mapLevelsGenerated * _mapLevelSpacing;

            instance.transform.localPosition = new Vector3(posX, posY);

            _mapLevelsGenerated++;

            return instance;
        }
    }
}