using System.Collections.Generic;
using MyRTS.Object.Unit.Behaviours;
using MyRTS.Object.Unit.Capabilities.Attacker;
using MyRTS.Player;
using MyRTS.Player.Commands;
using UnityEditor;
using UnityEngine;

namespace MyRTS.Object.Unit.Building.Tower
{
    public class TowerController : BuildingController, IRangedAttacker
    {
        public override void InitialiseUnit(PlayerManager pOwner, UnitType type, CommandDto initialCommandDto = null)
        {
            base.InitialiseUnit(pOwner, type);
            InitialiseTargetterFOVEvents();
        }
        
        /*
         * ITargetter
         */
        [field: Space(8f)][field: Header("Targeter Stuff")] [field: SerializeField] public UnitController Target { get; set; }
        [field: SerializeField] public List<UnitController> UnitsInFOV { get; set; } = new();
        [field: SerializeField] public FOV TargeterFOV { get; set; }
        
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
            Debug.Log("added Unit to list");
            UnitsInFOV.Add(enterer);
            enterer.OnDying.AddListener(OnUnitInFOVDied);
        }
        public void OnUnitExitedFOV(UnitController exiter)
        {
            Debug.Log("removed Unit from list");
            UnitsInFOV.Remove(exiter);
            if (exiter == Target)
            {
                ResetTarget();
            }
        }
        public UnitController TryPickEnemyFromFOV()
        {
            foreach (var unit in UnitsInFOV)
            {
                if (unit.Owner.Faction != Owner.Faction)
                {
                    Debug.Log("found enemy " + unit.Uuid);
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
         * IRangedAttacker
         */
        [field: Space(8f)][field: Header("IRangedAttacker stuff")][field: SerializeField]public AttackerData AttackerData { get; set; }
        public void RangedAttack(UnitController attackable)
        {
            Debug.Log(Owner.Faction + " " + Data.type + " attacks " + attackable.Owner.Faction + " " + attackable.Data.type);
            attackable.TakeDamage(AttackerData.attackPower);
        }
        public bool IsTargetInAttackRange()
        {
            foreach (var unit in UnitsInFOV)
            {
                if (Target.Uuid == unit.Uuid)
                {
                    return true;
                }
            }
            return false;
        }
    }
}