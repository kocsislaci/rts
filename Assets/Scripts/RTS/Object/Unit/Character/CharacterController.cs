using System;
using System.Collections.Generic;
using RTS.Object.Resource;
using RTS.Object.Unit.Behaviours;
using RTS.Object.Unit.Capabilities.Builder;
using RTS.Object.Unit.Capabilities.Building;
using RTS.Object.Unit.Capabilities.Harvester;
using RTS.Object.Unit.Capabilities.Movable;
using RTS.Player;
using RTS.Player.Commands;
using UnityEngine;
using UnityEngine.AI;

namespace RTS.Object.Unit.Character
{
    public class CharacterController : UnitController, IMovable, IHarvester, IBuilder
    {
        /*
         * Unit Controller
         */
        public override void InitialiseUnit(PlayerManager pOwner, UnitType type, CommandDto initialCommandDto = null)
        {
            base.InitialiseUnit(pOwner, type);

            // Mood = CharacterMood.ATTACKER;
            // Stance = CharacterStance.AGGRESSIVE;
            Owner.MyPopulation.ActualPopulation += ((CharacterData)data).populationImpact;
            if (initialCommandDto != null)
            {
                // Destination = rallyPosition;
            }

            if (type == UnitType.Character)
            {
            }
        }

        /*    
         * IAttackable
         */
        public override float CurrentHealth
        {
            get => currentHealth;
            set
            {
                if (value <= 0)
                {
                    Owner.MyPopulation.ActualPopulation -= ((CharacterData)data).populationImpact;
                }

                base.CurrentHealth = value;
            }
        }
        
        /*
         * IMovable
         */
        [field: Header("Movable stuff")][field: SerializeField] public NavMeshAgent Agent { get; set; }

        public bool SetDestination(Vector3 targetPosition)
        {
            return Agent.SetDestination(targetPosition);
        }
        public bool HasPath()
        {
            return Agent.hasPath;
        }
        public bool HasReachedDestination()
        {
            return (Vector3.Distance(Agent.destination, transform.position) < 1f);
        }
        public bool ResetPath()
        {
            Agent.ResetPath();
            return true;
        }


        /*
         * IHarvester
         */
        [field: Header("Harvester stuff")][field: SerializeField] public HarvesterData HarvesterData { get; set; }
        [field: SerializeField] public int CurrentHarvested { get; set; }
        [field: SerializeField] public ResourceType? CurrentHarvestedType { get; set; }
        [field: SerializeField] public ResourceController CurrentHarvestedTarget { get; set; }
        [field: SerializeField]public List<ResourceController> ResourcesInFOV { get; set; } = new();
        [field: SerializeField] public IHarvestReceiver CurrentDropOffTarget { get; set; }
        [field: SerializeField]public List<IHarvestReceiver> DropOffTargetsInFOV { get; set; } = new();
        [field: SerializeField] public FOV HarvesterFOV { get; set; }

        public bool Harvest(ResourceController harvestable)
        {
            var harvested = harvestable.GetHarvested(this);
            CurrentHarvested += harvested;
            return harvested != 0;
        }

        public void SetHarvestTarget(ResourceController harvestable)
        {
            CurrentHarvestedType = harvestable.Data.type;
            CurrentHarvestedTarget = harvestable;
        }

        public void OnHarvestableEntered(ResourceController enterer)
        {
            ResourcesInFOV.Add(enterer);
        }

        public void OnHarvestableExited(ResourceController exiter)
        {
            ResourcesInFOV.Remove(exiter);
        }

        public void CurrentHarvestTargetHasDied()
        {
            if (ResourcesInFOV.Count > 0)
                SetHarvestTarget(ResourcesInFOV[0]);
        }

        public void OnDropOffTargetEntered(IHarvestReceiver enterer)
        {
            DropOffTargetsInFOV.Add(enterer);
        }

        public void OnDropOffTargetExited(IHarvestReceiver exiter)
        {
            DropOffTargetsInFOV.Remove(exiter);
        }

        

        /*
         * IBuilder
         */
        [field: Header("Builder stuff")][field: SerializeField] public BuilderData BuilderData { get; set; }
        [field: SerializeField] public IBuildable BuildTarget { get; set; }
        [field: SerializeField] public List<IHarvestReceiver> BuildTargetsInFOV { get; set; } = new();
        [field: SerializeField] public FOV BuilderFOV { get; set; }
        
        public void Build(IBuildable buildable)
        {
        }
        public void SetBuildTarget(IBuildable buildable)
        {
        }
        public void OnBuildableEntered(IBuildable enterer)
        {
        }
        public void OnBuildableExited(IBuildable exiter)
        {
        }
        public void CurrentBuildableTargetFinished()
        {
        }
    }
}
