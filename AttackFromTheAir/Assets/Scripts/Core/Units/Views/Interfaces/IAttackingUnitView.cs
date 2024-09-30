using Core.GameLogic;
using Core.Weapon;

namespace Core.Units
{
    public interface IAttackingUnitView : IUnitView 
    {
        void InitWeapon(BaseWeaponView weapon);
        void OnStartAttacking(IDamagableObject unit);
        void OnStopAttacking();
    }
}