using System.Collections.Generic;
using JetBrains.Annotations;
using MyRTS.Object.Unit.Behaviours;
using UnityEditor;

namespace MyRTS.Object.Unit.Capabilities.General
{
    public interface ITargeter
    {
        [CanBeNull] public UnitController Target { get; set; }
        public List<UnitController> UnitsInFOV { get; set; }
        public FOV TargeterFOV { get; set; }
        public void InitialiseTargetterFOVEvents();
        public void SetTarget(UnitController targetable);
        public void ResetTarget();
        public void OnUnitEnteredFOV(UnitController enterer);
        public void OnUnitExitedFOV(UnitController exiter);
        [CanBeNull] public UnitController TryPickEnemyFromFOV();
        public void OnTargetDied(GUID uuid);
        public void OnUnitInFOVDied(GUID uuid);
    }
}
