using System.Collections.Generic;
using JetBrains.Annotations;
using MyRTS.Object.Unit.Behaviours;
using MyRTS.Object.Unit.Capabilities.Building;

namespace MyRTS.Object.Unit.Capabilities.Builder
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
