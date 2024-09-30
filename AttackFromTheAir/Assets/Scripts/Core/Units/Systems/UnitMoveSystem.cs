using Core.Utilities;
using UnityEngine;
using UnityEngine.AI;

namespace Core.Units
{
    public interface IUnitMoveSystem
    {
        SimpleEvent UnitReachedDestinationEvent { get; }
        bool IsMoving { get; }
        void MoveTo(Vector3 pos);
        void StopMove();
    }

    public abstract class UnitMoveSystem : MonoBehaviour, IUnitMoveSystem
    {
        [SerializeField] private NavMeshAgent _navMeshAgent;
        private bool _isMoving;
        private SimpleEvent _unitReachedDestinationEvent = new SimpleEvent();
        protected NavMeshAgent NavMeshAgent => _navMeshAgent;
        public SimpleEvent UnitReachedDestinationEvent => _unitReachedDestinationEvent;
        public bool IsMoving => _isMoving;

        public void WarpAgentTo(Vector3 pos)
        {
            if (_navMeshAgent.enabled)
            {
               _navMeshAgent.Warp(pos);
            }
        }

        public void SetActiveNavMeshAgent(bool isActive)
        {
            _navMeshAgent.enabled = isActive;
        }

        public virtual void MoveTo(Vector3 pos)
        {
            _isMoving = true;
            if (_navMeshAgent.enabled)
            {
                _navMeshAgent.SetDestination(pos);
            }
        }

        public virtual void StopMove()
        {
            _isMoving = false;
            if (_navMeshAgent.enabled)
            {
                _navMeshAgent.isStopped = true;
            }
        }

        private void CheckReachedDestination()
        {
            if (_isMoving && _navMeshAgent.enabled)
            {
                if (!_navMeshAgent.pathPending)
                {
                    if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
                    {
                        if (!_navMeshAgent.hasPath || _navMeshAgent.velocity.sqrMagnitude == 0f)
                        {
                            _isMoving = false;
                            _unitReachedDestinationEvent.Notify();
                        }
                    }
                }

                AfterCheckDestination();
            }
        }

        protected virtual void AfterCheckDestination()
        {
        }

        private void FixedUpdate()
        {
            CheckReachedDestination();
        }
    }
}