using MyRTS.Object.Unit.Capabilities.General;

namespace MyRTS.Object.Unit.Capabilities.Attacker
{
    public interface IRangedAttacker: ITargeter
    {
        public AttackerData AttackerData { get; set; }
        public void RangedAttack(UnitController attackable);
        public bool IsTargetInAttackRange();

    }
}