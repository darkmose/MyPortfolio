using UnityEngine;

namespace Core.Units
{
    public class VehicleDestroySystem : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _vehicleExplosionParticles;
        [SerializeField] private ParticleSystem _fireParticles;
        [SerializeField] private Rigidbody _bodyRB;
        [SerializeField] private Rigidbody _turretRB;
        [SerializeField] private GameObject _wheels;
        [SerializeField] private Collider _bodyCollider;
        [SerializeField] private Collider _turretCollider;
        [SerializeField] private float _explosionForce;
        [SerializeField] private float _explosionRadius;
        [SerializeField] private float _explosionUpwardModifier;

        public void Destroy()
        {
            _vehicleExplosionParticles.Play();
            _bodyCollider.isTrigger = false;
            _bodyRB.isKinematic = false;
            _bodyRB.useGravity = true;
            _wheels.SetActive(false);
            _turretRB.isKinematic = false;
            _turretRB.useGravity = true;
            _turretCollider.isTrigger = false;
            _fireParticles.Play();

            AddExplosionForce();
        }

        public void ResetPhysics()
        {
            _bodyCollider.isTrigger = true;
            _bodyRB.isKinematic = true;
            _bodyRB.useGravity = false;
            _turretRB.isKinematic = true;
            _turretRB.useGravity = false;
            _turretCollider.isTrigger = true;
        }

        private void AddExplosionForce()
        {
            var centerPoint = transform.position;
            _bodyRB.AddExplosionForce(_explosionForce, centerPoint, _explosionRadius, _explosionUpwardModifier, ForceMode.Impulse);
        }
    }
}