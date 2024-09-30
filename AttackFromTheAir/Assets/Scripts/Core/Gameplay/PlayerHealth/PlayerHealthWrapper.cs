using Core.UI;
using Core.Utilities;
using UnityEngine;

namespace Core.GameLogic
{
    public class PlayerHealthWrapper : MonoBehaviour, IDamagableObject
    {
        private PlayerHealth _playerHealth = new PlayerHealth();
        public BaseDamagableObjectView View { get => _playerHealth.View; set => _playerHealth.View = value; }
        public SimpleEvent<IDamagableObject> ObjectDestroyed => _playerHealth.ObjectDestroyed;
        public SimpleEvent<IDamagableObject> ObjectDamaged => _playerHealth.ObjectDamaged;
        public IPropertyReadOnly<float> NormalizedHealth => _playerHealth.NormalizedHealth;
        public IPropertyReadOnly<int> Health => _playerHealth.Health;
        public bool IsDestroyed => _playerHealth.IsDestroyed;
        public SimpleEvent<ExplosionData> ObjectExploded => _playerHealth.ObjectExploded;

        public void HandleExplosion(ExplosionData data)
        {
            _playerHealth.HandleExplosion(data);
        }

        public void SetHealth(int health)
        {
            _playerHealth.SetHealth(health);    
        }

        public void SetMaxHealth(int maxHealth)
        {
            _playerHealth.SetMaxHealth(maxHealth);
        }

        public void TakeDamage(int damage)
        {
            _playerHealth.TakeDamage(damage);
        }
    }
}