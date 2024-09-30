using Core.Utilities;
using UnityEngine;

namespace Core.GameLogic
{
    public abstract class BaseDamagableObjectView : MonoBehaviour, IDamagableObjectView
    {
        public SimpleEvent<int> GetDamageEvent { get; } = new SimpleEvent<int>();
        public SimpleEvent<ExplosionData> ExplodedEvent { get; } = new SimpleEvent<ExplosionData>();
        public IDamagableObject Model { get; set; }

        public void RaiseGetDamageEvent(int damage)
        {
            GetDamageEvent.Notify(damage);
        }

        public void HandleExplosion(ExplosionData explosionData)
        {
            ExplodedEvent.Notify(explosionData);
        }

        public abstract void OnObjectDamaged(IDamagableObject damagableObject);
        public abstract void OnObjectDestroy(IDamagableObject damagableObject);
        public abstract void SetHealthNormalized(float health);
    }

    public struct ExplosionData
    {
        public bool IsPlayerExplosion;
        public Vector3 Center;
        public float Radius;
        public float Force;
        public float UpwardModificator;
    }
}