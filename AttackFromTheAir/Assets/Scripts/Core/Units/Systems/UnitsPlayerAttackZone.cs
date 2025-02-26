using Core.Events;
using Core.GameLogic;
using Core.Tools;
using UnityEngine;

namespace Core.Units
{
    public class UnitsPlayerAttackZone : MonoBehaviour, IGameTrigger
    {
        [SerializeField] private Collider _collider;
        private GameTriggerEvent _triggerEvent;
        public int ID => 0;
        public GameTriggerType TriggerType => GameTriggerType.PlayerInAttackZone;

        private void Awake()
        {
            _triggerEvent = new GameTriggerEvent();
            _triggerEvent.TriggerID = ID;
            _triggerEvent.TriggerObject = gameObject;
            SetActive(false);
            Timer.SetTimer(7f, () => SetActive(true)); //Unit attacking player on loading screen
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("UnitsPlayerAttackZone " + other.gameObject.name + " Enter Zone");
            if (other.TryGetComponent(out PlayerData playerData))
            {
                _triggerEvent.TriggerType = GameTriggerType.PlayerInAttackZone;
                EventAggregator.Post(this, _triggerEvent);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            Debug.Log("UnitsPlayerAttackZone " + other.gameObject.name + " Exit Zone");
            if (other.TryGetComponent(out PlayerData playerData))
            {
                _triggerEvent.TriggerType = GameTriggerType.PlayerOutAttackZone;
                EventAggregator.Post(this, _triggerEvent);
            }
        }

        public void SetActive(bool active)
        {
            _collider.enabled = active;
        }
    }
}