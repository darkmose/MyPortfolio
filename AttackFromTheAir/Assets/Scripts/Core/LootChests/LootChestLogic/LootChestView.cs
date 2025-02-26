using Core.Resourses;
using Core.Utilities;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Core.LootChests
{
    public interface ILootChestView
    {
        ILootChest Model { get; }
        void Open();
    }

    public interface ILootChestViewInitor
    {
        void LinkModel(ILootChest model);
    }

    public class LootChestView : MonoBehaviour, ILootChestView, ILootChestViewInitor, IPointerClickHandler
    {
        [SerializeField] private LootChestSpriteProvider _lootChestSpriteProvider;
        [SerializeField] private ChestLootSpriteProvider _lootSpriteProvider;
        [SerializeField] private ChestLootView _chestLoot;
        [SerializeField] private GameObject _lootPanel;
        [SerializeField] private Image _lootImage;
        [SerializeField] private Image _chestImage;
        private SimpleEvent _chestViewClickRequestEvent = new SimpleEvent();
        private Sprite _openedChestSprite;
        public ILootChest Model { get; set; }
        public SimpleEvent ChestViewClickRequestEvent => _chestViewClickRequestEvent;

        public void LinkModel(ILootChest model)
        {
            Model = model;
            var lootChestSpriteDescriptor = _lootChestSpriteProvider.ProvideByRarity(model.Rarity);
            _chestImage.sprite = lootChestSpriteDescriptor.ClosedSprite;
            _openedChestSprite = lootChestSpriteDescriptor.OpenedSprite;

            var lootSprite = _lootSpriteProvider.ProvideByLootType(model.Loot.LootType);
            _lootImage.sprite = lootSprite;

            model.Loot.LinkView(_chestLoot);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _chestViewClickRequestEvent.Notify();
        }

        public void Open()
        {
            _chestImage.sprite = _openedChestSprite;
            _lootPanel.SetActive(true);
            //TODO particles play
        }
    }
}