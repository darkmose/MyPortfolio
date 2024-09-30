using Core.GameLogic;
using UnityEngine;

namespace Core.Weapon
{
    public class BulletProjectileView : ProjectileView
    {
        [SerializeField] private ParticleSystem _mainParticle;
        [SerializeField] private Collider _triggerCollider;
        private int _groundLayer;
        

        private void Awake()
        {
            _groundLayer = LayerMask.NameToLayer("Ground");
        }

        private void DisableBullet()
        {
            _mainParticle.Stop();
            _mainParticle.Clear();
            _triggerCollider.enabled = false;
        }

        private void OnEnable()
        {
            _mainParticle.Play();
            _triggerCollider.enabled = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out BaseDamagableObjectView damagableObjectView))
            {
                if (!other.gameObject.CompareTag(WeaponFraction.ToString()))
                {
                    damagableObjectView.RaiseGetDamageEvent(Damage);
                    DisableBullet();
                }
            }
            if (other.gameObject.layer == _groundLayer)
            {
                DisableBullet();
            }
        }
    }
}