using System;
using RTS.Object.Unit.Building;
using RTS.Object.Unit.Capabilities.Builder;
using UnityEngine.Events;

namespace RTS.Object.Unit.Capabilities.Building
{
    public interface IBuildable
    {
        public float CurrentHealth { get; set; }
        public BuildingStatus BuildingStatus { get; set; }
        public UnityEvent BuildingFinishedEvent { get; set; }
        public void GetBuilt(IBuilder builder);
        public void SubscribeToBeingFinished(IBuilder builder)
        {
            BuildingFinishedEvent.AddListener(builder.CurrentBuildableTargetFinished);
        }
    }
}
