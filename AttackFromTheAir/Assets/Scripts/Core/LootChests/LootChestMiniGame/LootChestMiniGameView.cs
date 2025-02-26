using Core.Tools;
using Core.UI;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Core.LootChests
{
    public class LootChestMiniGameView : MonoBehaviour, IDisposable
    {
        [SerializeField] private LootChestView _lootChestViewPrefab;
        [SerializeField] private RectTransform _lootChestsRoot;
        [SerializeField] private GridLayoutGroup _gridLayoutGroup;
        [SerializeField] private int _chestsRows;
        [SerializeField] private int _chestsColumns;
        private LootChestMiniGame _lootChestMiniGameModel;

        public void Dispose()
        {
            _lootChestsRoot.ClearAllChild();
        }

        public void LinkModel(LootChestMiniGame lootChestMiniGame)
        {
            _lootChestMiniGameModel = lootChestMiniGame;

            foreach (var chest in lootChestMiniGame.LootChests)
            {
                var lootChestView = CreateLootChestView();
                chest.LinkView(lootChestView);
            }

            var rootSize = _lootChestsRoot.rect.size;
            var cellSizeX = rootSize.x / _chestsRows;
            var cellSizeY = rootSize.y / _chestsColumns;
            _gridLayoutGroup.cellSize = new Vector2(cellSizeX, cellSizeY);
            _gridLayoutGroup.spacing = new Vector2(-cellSizeX / 8f, -cellSizeY / 8f);
        }

        private LootChestView CreateLootChestView()
        {
            var instance = Instantiate<LootChestView>(_lootChestViewPrefab, _lootChestsRoot);
            if (instance.TryGetComponent(out Image image))
            {
                image.raycastPadding = new Vector4(64f, 64f, 64f, 64f);
            }
            return instance;
        }
    }
}