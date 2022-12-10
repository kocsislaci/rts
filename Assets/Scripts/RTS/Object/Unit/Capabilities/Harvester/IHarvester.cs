using System.Collections.Generic;
using JetBrains.Annotations;
using RTS.Object.Resource;
using RTS.Object.Unit.Behaviours;
using RTS.Object.Unit.Capabilities.Building;

namespace RTS.Object.Unit.Capabilities.Harvester
{
    public interface IHarvester
    {
        public HarvesterData HarvesterData { get; set; }
        public int CurrentHarvested { get; set; }
        public ResourceType? CurrentHarvestedType { get; set; }
        [CanBeNull] public ResourceController CurrentHarvestedTarget { get; set; }
        public List<ResourceController> ResourcesInFOV { get; set; }
        [CanBeNull] public IHarvestReceiver CurrentDropOffTarget { get; set; }
        public List<IHarvestReceiver> DropOffTargetsInFOV { get; set; }
        public FOV HarvesterFOV { get; set; }

        
        public bool Harvest(ResourceController harvestable);
        public void SetHarvestTarget(ResourceController harvestable);
        public void OnHarvestableEntered(ResourceController enterer);
        public void OnHarvestableExited(ResourceController exiter);
        public void CurrentHarvestTargetHasDied();
        public void OnDropOffTargetEntered(IHarvestReceiver enterer);
        public void OnDropOffTargetExited(IHarvestReceiver exiter);
        // public void CurrentDropOffTargetHasDied();
    }
}
