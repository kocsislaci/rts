using System.Collections.Generic;
using MyRTS.Object.Resource;
using MyRTS.Object.Unit.Behaviours;
using MyRTS.Object.Unit.Capabilities.Attacker;
using MyRTS.Object.Unit.Capabilities.Builder;
using MyRTS.Object.Unit.Capabilities.Building;
using MyRTS.Object.Unit.Capabilities.General;
using MyRTS.Object.Unit.Capabilities.Harvester;
using MyRTS.Object.Unit.Capabilities.Movable;
using MyRTS.Player;
using MyRTS.Player.Commands;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

namespace MyRTS.Object.Unit.Character
{
    public class CharacterController : UnitController, IMovable, ITargeter, ICloseAttacker, IHarvester, IBuilder
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
        [field: Space(8f)][field: Header("Movable stuff")][field: SerializeField] public NavMeshAgent Agent { get; set; }

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
         * ITargetter
         */
        [field: Space(8f)][field: Header("ITargetter stuff")][field: SerializeField] public UnitController Target { get; set; }
        [field: SerializeField] public FOV TargeterFOV { get; set; }
        [field: SerializeField] public List<UnitController> UnitsInFOV { get; set; } = new();
        public void InitialiseTargetterFOVEvents()
        {
            TargeterFOV.UnitEnteredEvent.AddListener(OnUnitEnteredFOV);
            TargeterFOV.UnitExitedEvent.AddListener(OnUnitExitedFOV);
        }
        public void SetTarget(UnitController targetable)
        {
            Target = targetable;
            Target.OnDying.AddListener(OnTargetDied);
        }
        public void OnUnitEnteredFOV(UnitController enterer)
        {
            UnitsInFOV.Add(enterer);
            enterer.OnDying.AddListener(OnUnitInFOVDied);
        }
        public void OnUnitExitedFOV(UnitController exiter)
        {
            UnitsInFOV.Remove(exiter);
        }
        public UnitController TryPickEnemyFromFOV()
        {
            foreach (var unit in UnitsInFOV)
            {
                if (unit.Owner.Faction != Owner.Faction)
                {
                    return unit;
                }
            }
            return null;
        }
        public void ResetTarget()
        {
            if (Target != null)
            {
                Target.OnDying.RemoveListener(OnTargetDied);
                Target = null;
            }
        }
        public void OnTargetDied(GUID uuid)
        {
            ResetTarget();
        }
        public void OnUnitInFOVDied(GUID uuid)
        {
            foreach (var unit in UnitsInFOV)
            {
                if (unit.Uuid != uuid) continue;
                
                UnitsInFOV.Remove(unit);
                return;
            }
        }
        /*
         * Close attacker
         */
        [field: Space(8f)][field: Header("ICloseAttacker stuff")][field: SerializeField]public AttackerData AttackerData { get; set; }
        [field: SerializeField]public FOV AttackRange { get; set; }
        [field: SerializeField] public List<UnitController> UnitsInAttackRange { get; set; } = new();
        public void InitialiseAttackRangeEvents()
        {
            AttackRange.UnitEnteredEvent.AddListener(OnUnitEnteredAttackRange);
            AttackRange.UnitExitedEvent.AddListener(OnUnitExitedAttackRange);
        }
        public void Attack(UnitController attackable)
        {
            Debug.Log(Owner.Faction + " " + Data.type + " attacks " + attackable.Owner.Faction + " " + attackable.Data.type);
            attackable.TakeDamage(AttackerData.attackPower);
        }
        public bool IsTargetInAttackRange()
        {
            foreach (var unit in UnitsInAttackRange)
            {
                if (Target.Uuid == unit.Uuid)
                {
                    return true;
                }
            }
            return false;
        }
        public void OnUnitEnteredAttackRange(UnitController enterer)
        {
            UnitsInAttackRange.Add(enterer);
        }
        public void OnUnitExitedAttackRange(UnitController exiter)
        {
            UnitsInAttackRange.Remove(exiter);
        }
        


        /*
         * IHarvester
         */
        [field: Space(8f)][field: Header("Harvester stuff")][field: SerializeField] public HarvesterData HarvesterData { get; set; }
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

        public void CurrentHarvestTargetHasDied(GUID uuid)
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
        [field: Space(8f)][field: Header("Builder stuff")][field: SerializeField] public BuilderData BuilderData { get; set; }
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
