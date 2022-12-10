using System;
using RTS.GameManagers;
using RTS.Object.Unit.Capabilities.Harvester;
using UnityEngine;

namespace RTS.Object.Resource
{
    public class ResourceController : ObjectController
    {
        public ResourceData Data { get; private set; }
        public int CurrentHarvestable
        {
            get => currentHarvestable;
            set
            {
                currentHarvestable = value;
                if (value <= 0)
                {
                    Destroy();
                }
            }
        }
        private int currentHarvestable;

        public void InitialiseResource(ResourceType type)
        {
            InitialiseObject(ObjectType.Resource);
            Data = Resources.Load<ResourceData>(GameResources.PathToLoadResourceData[type]);
            CurrentHarvestable = Data.maxHarvestable;
            GameResources.GameManager.ResourcesOnMap[type].Add(Uuid, this);
        }
        public override void Destroy()
        {
            GameResources.GameManager.ResourcesOnMap[Data.type].Remove(Uuid);
            base.Destroy();
        }
        public int GetHarvested(IHarvester harvester)
        {
            var harvesterCanTakeAtOnce = harvester.HarvesterData.harvestingPower;
            var harvesterHasThisFreeSpace = harvester.HarvesterData.maxHarvested - harvester.CurrentHarvested;
            var harvesterTriesToTake = Math.Min(harvesterHasThisFreeSpace, harvesterCanTakeAtOnce);
            var harvesterTakesFromRemaining = Math.Min(harvesterTriesToTake, CurrentHarvestable);

            CurrentHarvestable -= harvesterTakesFromRemaining;
            return harvesterTakesFromRemaining;
        }
        public void SubscribeToDying(IHarvester harvester)
        {
            OnDying.AddListener(harvester.CurrentHarvestTargetHasDied);
        }
    }
}
