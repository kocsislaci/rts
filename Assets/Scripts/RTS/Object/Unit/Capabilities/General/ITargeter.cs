using System.Collections.Generic;
using JetBrains.Annotations;
using RTS.Object.Unit.Behaviours;

namespace RTS.Object.Unit.Capabilities.General
{
    public interface ITargeter
    {
        [CanBeNull] public UnitController Target { get; set; }
        public List<UnitController> UnitsInFOV { get; set; }
        public FOV TargeterFOV { get; set; }
        public void SetTarget(UnitController targetable);
        public void OnUnitEntered(UnitController enterer);
        public void OnUnitExited(UnitController exiter);
        public void TargetDied();
    }
}
