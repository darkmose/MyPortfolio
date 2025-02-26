using Core.Utilities;
using System;
using TMPro;

namespace Core.LootChests
{
    public class ChestLoot
    {
        private ChestLootView _view;
        public ChestLootType LootType { get; }
        public SimpleEvent LootTakenEvent { get; } = new SimpleEvent();

        public ChestLoot(ChestLootType lootType)
        {
            LootType = lootType;
        }

        public void LinkView(ChestLootView lootView)
        {
            _view = lootView;
            lootView.LinkModel(this);
        }

        public void Appear(Action onComplete = null)
        {
            _view.Appear(()=> 
            {
                HandleLoot();
                onComplete?.Invoke();
            });
        }

        private void HandleLoot()
        {
            var handler = ChestLootHandlerFactory.Create(LootType);
            handler.HandleLoot();
        }

        public void Hide()
        {
            _view.Hide(() => 
            {
                LootTakenEvent.Notify();
            });
        }
    }
}