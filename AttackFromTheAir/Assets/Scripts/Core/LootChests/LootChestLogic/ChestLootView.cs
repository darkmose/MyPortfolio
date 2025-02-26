using Core.Tools;
using System;
using UnityEngine;

namespace Core.LootChests
{
    public class ChestLootView : MonoBehaviour
    {
        private ChestLoot _model;

        public void LinkModel(ChestLoot loot)
        {
            _model = loot;
        }

        public void Appear(Action onComplete)
        {
            Timer.SetTimer(1f, ()=>onComplete?.Invoke());
        }

        public void Hide(Action onComplete)
        {
            Timer.SetTimer(1f, ()=>onComplete?.Invoke());
        }
    }
}