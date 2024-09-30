using Core.UI;
using Core.Utilities;

namespace Core.GameLogic
{
    public interface IDamagableObject 
    {
        BaseDamagableObjectView View { get; set; }
        SimpleEvent<IDamagableObject> ObjectDestroyed { get; }
        SimpleEvent<IDamagableObject> ObjectDamaged { get; }
        SimpleEvent<ExplosionData> ObjectExploded { get; }
        IPropertyReadOnly<float> NormalizedHealth { get; }
        IPropertyReadOnly<int> Health { get; }
        bool IsDestroyed { get; }
        void SetMaxHealth(int maxHealth);
        void SetHealth(int health);
        void TakeDamage(int damage);
        void HandleExplosion(ExplosionData data);
    }
}