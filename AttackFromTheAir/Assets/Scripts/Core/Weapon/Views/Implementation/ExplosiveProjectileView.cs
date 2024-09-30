using Core.GameLogic;
using Core.Tools;
using Core.Utilities;
using UnityEngine;

namespace Core.Weapon
{
    public class ExplosiveProjectileView : ProjectileView
    {
        private const float DESTROY_DELAY = 2f;
        [SerializeField] private Collider _contactCollider;
        [SerializeField] private Collider _triggerCollider;
        [SerializeField] private Renderer _renderer;
        [SerializeField] private ParticleSystem _explosionParticles;
        [SerializeField] private float _explosionRadius;
        [SerializeField] private float _explosionForce;
        [SerializeField] private float _upwardModificator;
        private bool _isExploded;

        private void OnEnable()
        {
            _isExploded = false;
            _contactCollider.enabled = true;
            _triggerCollider.enabled = false;
            _explosionParticles.Stop();
            _explosionParticles.Clear();
            _renderer.enabled = true;
        }

        private void OnCollisionEnter(Collision collision)
        {
            Explode();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!_isExploded) return;

            if (other.gameObject.TryGetComponent<BaseDamagableObjectView>(out var damagableObjectView))
            {
                damagableObjectView.RaiseGetDamageEvent(Damage);
                damagableObjectView.HandleExplosion(new ExplosionData
                {
                    IsPlayerExplosion = true,
                    Center = transform.position,
                    Force = _explosionForce,
                    Radius = _explosionRadius,
                    UpwardModificator = _upwardModificator
                });
            }
        }

        private void Explode()
        {
            _contactCollider.enabled = false;
            _triggerCollider.enabled = true;
            _explosionParticles.Play();
            _renderer.enabled = false;
            _isExploded = true;
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