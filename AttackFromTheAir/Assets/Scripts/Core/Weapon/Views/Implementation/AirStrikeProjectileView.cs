using Core.Events;
using Core.GameLogic;
using Core.Tools;
using Core.Utilities;
using DestroyIt;
using UnityEngine;

namespace Core.Weapon
{
    public class AirStrikeProjectileView : ProjectileView
    {
        [SerializeField] private Collider _contactCollider;
        [SerializeField] private Collider _triggerCollider;
        [SerializeField] private Renderer _renderer;
        [SerializeField] private ParticleSystem _explosionParticles;
        [SerializeField] private Explode _destroyItExplode;
        [SerializeField] private float _explosionRadius;
        [SerializeField] private float _explosionForce;
        [SerializeField] private float _upwardModificator;
        [SerializeField] private ParticleSystem _engineOnParticles;
        [SerializeField] private Rigidbody _rigidbody;
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

        public void EngineOff()
        {
            _engineOnParticles.Stop();
            _engineOnParticles.Clear();
        }

        public void EngineOn()
        {
            _engineOnParticles.Play();
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
            EventAggregator.Post(this, new ExplosionEvent { Position = transform.position, playerWeaponType = PlayerWeaponType.ThrowOffBomb });
            _contactCollider.enabled = false;
            _triggerCollider.enabled = true;
            _explosionParticles.Play();
            _renderer.enabled = false;
            _isExploded = true;
            _destroyItExplode.Explosion();
            _rigidbody.Sleep();
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