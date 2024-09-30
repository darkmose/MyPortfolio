using Core.Buildings;
using Core.Events;
using Core.GameLogic;
using UnityEngine;

namespace Core.Tools
{
    public class BuildingGameTriggerView : BaseSpecialBuildingView, IGameTrigger
    {
        [SerializeField] private int _triggerID;
        [SerializeField] private GameTriggerType _triggerType;
        public int ID => _triggerID;
        public GameTriggerType TriggerType => _triggerType;
        public override SpecialBuildingType SpecialBuildingType => SpecialBuildingType.TriggerOnDestroy;
        public override BuildingType BuildingType => BuildingType.Special;

        public override void OnObjectDamaged(IDamagableObject damagableObject)
        {

        }

        public override void OnObjectDestroy(IDamagableObject damagableObject)
        {
            EventAggregator.Post(this, new GameTriggerEvent { GameTrigger = this });
        }

        public override void SetHealthNormalized(float health)
        {

        }
    }
}