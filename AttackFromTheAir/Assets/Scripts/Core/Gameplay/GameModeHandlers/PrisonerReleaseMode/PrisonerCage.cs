using Core.Tools;
using DG.Tweening;
using UnityEngine;

namespace Core.GameLogic
{
    public class PrisonerCage : MonoBehaviour
    {
        [SerializeField] private Transform _gates;
        [SerializeField] private Transform _gatesOpenPoint;
        [SerializeField] private BuildingGameTriggerView _trigger;

        private void Awake()
        {
            Timer.SetTimer(1f, () =>
            {
                _trigger.Model.ObjectDestroyed.AddListener(OnTriggerDestroyed);
            });            
        }

        private void OnTriggerDestroyed(IDamagableObject @object)
        {
            AnimateCageOpen();
        }

        private void AnimateCageOpen()
        {
            _gates.DOMove(_gatesOpenPoint.position, 1f);
        }

        private void OnDestroy()
        {
            _trigger?.Model.ObjectDestroyed.RemoveListener(OnTriggerDestroyed);
        }
    }
}