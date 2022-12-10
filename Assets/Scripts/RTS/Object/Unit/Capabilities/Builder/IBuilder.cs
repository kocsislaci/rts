using System.Collections.Generic;
using JetBrains.Annotations;
using RTS.Object.Unit.Behaviours;
using RTS.Object.Unit.Capabilities.Building;
using UnityEngine;

namespace RTS.Object.Unit.Capabilities.Builder
{
    public interface IBuilder
    {
        public BuilderData BuilderData { get; set; }
        [CanBeNull] public IBuildable BuildTarget { get; set; }
        public List<IHarvestReceiver> BuildTargetsInFOV { get; set; }
        public FOV BuilderFOV { get; set; }


        public void Build(IBuildable buildable);
        public void SetBuildTarget(IBuildable buildable);
        public void OnBuildableEntered(IBuildable enterer);
        public void OnBuildableExited(IBuildable exiter);
        public void CurrentBuildableTargetFinished();
    }
}
