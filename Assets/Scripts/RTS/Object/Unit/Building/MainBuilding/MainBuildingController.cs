using System.Collections.Generic;
using RTS.Object.Unit.Behaviours;
using RTS.Object.Unit.Capabilities.Attacker;
using RTS.Player;
using RTS.Player.Commands;
using UnityEngine;

namespace RTS.Object.Unit.Building.MainBuilding
{
    public class MainBuildingController: BuildingController, IRangedAttacker/*, ITrainer, IHarvestReceiver*/
    {
        public override void InitialiseUnit(PlayerManager pOwner, UnitType type, CommandDto initialCommandDto = null)
        {
            base.InitialiseUnit(pOwner, type, null);
            // SetFighterFovBehaviour();
        }

        /*
         * IRangedAttacker
         */
        [field: Header("Targeter Stuff")] [field: SerializeField] public UnitController Target { get; set; }
        [field: SerializeField] public List<UnitController> UnitsInFOV { get; set; } = new();
        [field: SerializeField] public FOV TargeterFOV { get; set; }

        // public void SetFighterFovBehaviour()
        // {
        //     // FighterFovBehaviour.SetFOV(Data.fieldOfView);
        //     FighterFovBehaviour.unitEntered.AddListener(OnUnitEntered);
        //     FighterFovBehaviour.unitExited.AddListener(OnUnitExited);
        // }
        public void SetTarget(UnitController targetable)
        {
            Target = targetable;
            targetable.OnDying.AddListener(TargetDied);
        }
        public void OnUnitEntered(UnitController enterer)
        {
            UnitsInFOV.Add(enterer);
            // Debug.Log(enterer.name);
        }
        public void OnUnitExited(UnitController exiter)
        {
            UnitsInFOV.Remove(exiter);
            // Debug.Log(exiter.name);
        }
        public void TargetDied()
        {
            Target = null;
            Debug.Log("our target died, try set from FOV");
            // if (unitsInFOV.Count > 0)
            // {
            //     foreach (var unitController in unitsInFOV)
            //     {
            //         if (unitController.Owner != Owner)
            //         {
            //             SetTarget(unitController);
            //             break;
            //         }
            //     }
            //     if (Target ==  null)
            //         Debug.Log("could not find enemy");
            // }
        }
        public void RangedAttack(UnitController attackable)
        {
            attackable.TakeDamage(Data.defense);
        }

        /*/*
         * IHarvestReceiver
         #1#
        public UnityEvent<IHarvestReceiver> BecomeUnavailable { get; set; }
        public void ReceiveHarvest()
        {
        }*/
    }
}
