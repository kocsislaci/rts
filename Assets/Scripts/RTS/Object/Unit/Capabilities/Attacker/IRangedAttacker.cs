using RTS.Object.Unit.Capabilities.General;
using UnityEngine;

namespace RTS.Object.Unit.Capabilities.Attacker
{
    public interface IRangedAttacker: ITargeter
    {
        public void RangedAttack(UnitController attackable);
    }
}