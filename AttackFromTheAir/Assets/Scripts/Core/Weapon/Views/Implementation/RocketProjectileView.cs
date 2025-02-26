using Cinemachine;
using Core.Events;
using Core.GameLogic;
using Core.Utilities;
using DestroyIt;
using UnityEngine;

namespace Core.Weapon
{
    public class RocketProjectileView : ProjectileView
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
        private float _projectileSpeed;
        private bool _isEngineOn;
        private bool _isExploded;
        private Vector3 _moveTarget = Vector3.zero;
        public SimpleEvent<RocketProjectileView> ExplosionEvent { get; } = new SimpleEvent<RocketProjectileView>();

        private void OnEnable()
        {
            _isExploded = false;
            _contactCollider.enabled = true;
            _triggerCollider.enabled = false;
            _explosionParticles.Stop();
            _explosionParticles.Clear();
            _renderer.enabled = true;
        }

        public void SetMoveTarget(Vector3 target)
        {
            _moveTarget = target;
        }

        public void InitProjectileSpeed(float projectileSpeed)
        {
            _projectileSpeed = projectileSpeed;
        }

        public void EngineOff()
        {
            _engineOnParticles.Stop();
            _engineOnParticles.Clear();
            _isEngineOn = false;
        }

        public void EngineOn()
        {
            _engineOnParticles.Play();
            _isEngineOn = true;
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
            ExplosionEvent.Notify(this);
            _rigidbody.Sleep();
        }

        private void FixedUpdate()
        {
            if (!_isExploded && _isEngineOn)
            {
                ApplyForce();
            }
        }

        public void ApplyForce()
        {
            var direction = (_moveTarget - transform.position).normalized;
            var pos = transform.position + direction * _projectileSpeed * Time.fixedDeltaTime;
            var lookRotation = Quaternion.LookRotation(direction);
            var rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, Time.fixedDeltaTime * _projectileSpeed);
            transform.position = pos;
            transform.rotation = rotation;
        }
    }
}