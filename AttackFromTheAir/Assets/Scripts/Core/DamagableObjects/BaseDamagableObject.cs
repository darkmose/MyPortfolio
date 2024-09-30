using Core.UI;
using Core.Utilities;

namespace Core.GameLogic
{
    public abstract class BaseDamagableObject : IDamagableObject
    {
        private IntProperty _health = new IntProperty(0);
        private FloatProperty _normalizedHealth = new FloatProperty();
        private SimpleEvent<IDamagableObject> _objectDestroyed = new SimpleEvent<IDamagableObject>();
        private SimpleEvent<IDamagableObject> _objectDamaged = new SimpleEvent<IDamagableObject>();
        private SimpleEvent<ExplosionData> _objectExploded = new SimpleEvent<ExplosionData>();
        private int _maxHealth;
        private bool _isDestroyed;
        public IPropertyReadOnly<int> Health { get; private set; }
        public SimpleEvent<IDamagableObject> ObjectDestroyed => _objectDestroyed;
        public SimpleEvent<IDamagableObject> ObjectDamaged => _objectDamaged;
        public bool IsDestroyed => _isDestroyed;
        public IPropertyReadOnly<float> NormalizedHealth => _normalizedHealth;
        public BaseDamagableObjectView View { get; set; }
        public SimpleEvent<ExplosionData> ObjectExploded => _objectExploded;

        public void SetHealth(int health)
        {
            _health.SetValue(health, true);
            MakeNormalizedHealth();
            if (_isDestroyed && health > 0)
            {
                _isDestroyed = false;
                return;
            }
            if (!_isDestroyed && health == 0)
            {
                Destroy();
            }
        }

        private void MakeNormalizedHealth()
        {
            if (_maxHealth != 0)
            {
                var normalizedHealth = (float)_health.Value / (float)_maxHealth;
                _normalizedHealth.SetValue(normalizedHealth, true);
            }
            else
            {
                _normalizedHealth.SetValue(0, true);
            }
        }

        public void SetMaxHealth(int maxHealth)
        {
            _maxHealth = maxHealth;
        }

        public void TakeDamage(int damage)
        {
            if (_isDestroyed) return;

            if (damage < _health.Value)
            {
                _health.SetValue(_health.Value - damage, true);
                MakeNormalizedHealth();
                _objectDamaged.Notify(this);
            }
            else
            {
                _health.SetValue(0, true);
                MakeNormalizedHealth();
                Destroy();
            }
        }

        public void HandleExplosion(ExplosionData data)
        {
            _objectExploded.Notify(data);
        }

        private void Destroy()
        {
            _isDestroyed = true;
            _objectDestroyed.Notify(this);
        }
    }
}