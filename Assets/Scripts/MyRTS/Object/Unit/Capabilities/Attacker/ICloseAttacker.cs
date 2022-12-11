using System.Collections.Generic;
using MyRTS.Object.Unit.Behaviours;

namespace MyRTS.Object.Unit.Capabilities.Attacker
{
    public interface ICloseAttacker
    {
        public AttackerData AttackerData { get; set; }
        public FOV AttackRange { get; set; }
        public List<UnitController> UnitsInAttackRange { get; set; }
        public void InitialiseAttackRangeEvents();
        public void Attack(UnitController attackable);
        public bool IsTargetInAttackRange();
        public void OnUnitEnteredAttackRange(UnitController enterer);
        public void OnUnitExitedAttackRange(UnitController exiter);
    }
}
