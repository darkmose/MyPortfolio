using DestroyIt;
using UnityEngine;

namespace Core.Buildings
{
    public class BuildingDestroySystem : MonoBehaviour
    {
        [SerializeField] private Destructible _destructible;

        public void Destroy()
        {
            if (_destructible == null)
            {
                return;
            }
            _destructible.canBeDestroyed = true;
            _destructible.canBeObliterated = true;
            _destructible.currentHitPoints = 0;
        }

        public void GetDamage(int damage)
        {
            if (_destructible != null)
            {
                _destructible.ApplyDamage(damage);
            }
        }
    }
}