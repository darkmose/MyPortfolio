using Core.Utilities;

namespace Core.GameLogic
{
    public interface IDamagableObjectView
    {
        IDamagableObject Model { get; set; }
        SimpleEvent<int> GetDamageEvent { get; }
        SimpleEvent<ExplosionData> ExplodedEvent { get; }
        void OnObjectDestroy(IDamagableObject damagableObject);
        void OnObjectDamaged(IDamagableObject damagableObject);
        void SetHealthNormalized(float health);
    }
}