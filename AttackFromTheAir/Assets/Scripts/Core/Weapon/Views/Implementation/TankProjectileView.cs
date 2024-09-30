using Core.GameLogic;
using UnityEngine;

namespace Core.Weapon
{
    public class TankProjectileView : ProjectileView
    {
        [SerializeField] private Collider _contactCollider;
        [SerializeField] private Collider _triggerCollider;
        [SerializeField] private ParticleSystem _explosionParticles;
        [SerializeField] private ParticleSystem _mainParticles;
        [SerializeField] private float _explosionRadius;
        [SerializeField] private float _explosionForce;
        [SerializeField] private float _upwardModificator;

        private void OnCollisionEnter(Collision collision)
        {
            Explode();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent<BaseDamagableObjectView>(out var damagableObjectView))
            {
                if (!WeaponFraction.Equals(other.gameObject.tag))
                {
                    damagableObjectView.RaiseGetDamageEvent(Damage);
                    damagableObjectView.HandleExplosion(new ExplosionData
                    {
                        Center = transform.position,
                        Force = _explosionForce,
                        Radius = _explosionRadius,
                        UpwardModificator = _upwardModificator
                    });
                }
            }
        }

        private void OnEnable()
        {
            _contactCollider.enabled = true;
            _triggerCollider.enabled = false;
            _explosionParticles.Clear();
            _mainParticles.Play();
        }

        private void Explode()
        {
            _contactCollider.enabled = false;
            _triggerCollider.enabled = true;
            _explosionParticles.Play();
            _mainParticles.Stop();
            _mainParticles.Clear();
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _explosionRadius);
        }
#endif
    }
}