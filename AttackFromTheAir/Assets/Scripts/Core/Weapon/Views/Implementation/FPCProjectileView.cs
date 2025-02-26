using Cinemachine;
using Core.Events;
using Core.GameLogic;
using Core.Utilities;
using DestroyIt;
using DG.Tweening;
using UnityEngine;

namespace Core.Weapon
{

    public class FPCProjectileView : ProjectileView
    {
        [SerializeField] private CinemachineVirtualCamera _projectileCamera;
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
        public CinemachineVirtualCamera ProjectileCamera => _projectileCamera;
        public SimpleEvent<FPCProjectileView> ExplosionEvent { get; } = new SimpleEvent<FPCProjectileView>();

        private void OnEnable()
        {
            _isExploded = false;
            _contactCollider.enabled = true;
            _triggerCollider.enabled = false;
            _explosionParticles.Stop();
            _explosionParticles.Clear();
            _renderer.enabled = true;
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
            var direction = transform.forward;
            var pos = transform.position + direction * _projectileSpeed * Time.fixedDeltaTime;
            transform.position = pos;
        }
    }
}