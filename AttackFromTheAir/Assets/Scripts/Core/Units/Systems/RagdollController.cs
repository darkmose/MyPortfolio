using Core.GameLogic;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Units
{
    public class RagdollController : MonoBehaviour
    {
        [SerializeField] private List<Rigidbody> _rigidbodies;

        private void Start()
        {
            DisableRagdoll();
        }

        public void EnableRagdoll()
        {
            foreach (var rb in _rigidbodies)
            {
                rb.isKinematic = false;
                rb.useGravity = true;
            }
        }

        public void DisableRagdoll() 
        {
            foreach (var rb in _rigidbodies)
            {
                rb.isKinematic = true;
                rb.useGravity = false;
            }
        }

        public void AddExplosion(ExplosionData explosionData)
        {
            foreach (var rb in _rigidbodies)
            {
                rb.AddExplosionForce(explosionData.Force, explosionData.Center, explosionData.Radius, explosionData.UpwardModificator);
            }
        }
    }
}