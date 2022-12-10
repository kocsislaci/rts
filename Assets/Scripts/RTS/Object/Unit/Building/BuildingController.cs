using System;
using System.Collections.Generic;
using RTS.GameManagers;
using RTS.Object.Unit.Capabilities.Builder;
using RTS.Object.Unit.Capabilities.Building;
using RTS.Player;
using RTS.Player.Commands;
using UnityEngine;
using UnityEngine.Events;

namespace RTS.Object.Unit.Building
{
    public abstract class BuildingController : UnitController, IPlaceable, IBuildable
    {
        /*
         * UnitController
         */
        public override void InitialiseUnit(PlayerManager pOwner, UnitType type, CommandDto initialCommandDto = null)
        {
            base.InitialiseUnit(pOwner, type);
            
            BuildingPlacementStatus = BuildingPlacementStatus.VALID;
            BuildingStatus = BuildingStatus.PENDING;
            CurrentHealth = 1;
            transform.localScale = new Vector3(1, CurrentHealth / data.maxHealth, 1);
        }

        public void DebugBuilt()
        {
            BuildingStatus = BuildingStatus.BUILT;
            CurrentHealth = Data.maxHealth;
        }
        
        
        /*
         * IAttackable / IBuildable
         */
        public override float CurrentHealth
        {
            get => currentHealth;
            set
            {
                if (BuildingStatus == BuildingStatus.PLACED)
                {
                    transform.localScale = new Vector3(1, CurrentHealth / data.maxHealth, 1);
                }
                if (value >= data.maxHealth)
                {
                    if (BuildingStatus == BuildingStatus.PLACED)
                    {
                        BuildingStatus = BuildingStatus.BUILT;
                    }
                    base.CurrentHealth = data.maxHealth;
                }
                else
                {
                    if (value <= 0)
                    {
                        Owner.MyPopulation.PopulationLimit -= ((BuildingData)data).populationGain;
                    }
                    base.CurrentHealth = value;
                }
            }
        }

        /*
         * IPlaceable
         */
        public BuildingPlacementStatus BuildingPlacementStatus
        {
            get => buildingPlacementStatus;
            set
            {
                if (value != buildingPlacementStatus)
                {
                    var material = Resources.Load<Material>(GameResources.PathToLoadBuildingPlacementStatusMaterial[value]);
                    foreach (var buildingPart in ((IPlaceable)this).BuildingParts)
                    {
                        buildingPart.GetComponent<MeshRenderer>().material = material;
                    }
                    buildingPlacementStatus = value;
                }
            }
        }
        protected BuildingPlacementStatus buildingPlacementStatus;

        public int Collosions { get => _collosions; set => _collosions = value; }
        private int _collosions;

        BoxCollider IPlaceable.Collider => GetComponent<BoxCollider>();
        
        List<GameObject> IPlaceable.BuildingParts => buildingParts;
        [SerializeField] private List<GameObject> buildingParts;

        public void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Terrain")) return;
            if (BuildingStatus != BuildingStatus.PENDING) return;
            
            _collosions++;
            if (_collosions > 1 && BuildingPlacementStatus != BuildingPlacementStatus.INVALID)
            {
                BuildingPlacementStatus = BuildingPlacementStatus.INVALID;
            }
        }
        public void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Terrain")) return;
            if (BuildingStatus != BuildingStatus.PENDING) return;

            _collosions--;
            if (_collosions == 0)
            {
                BuildingPlacementStatus = BuildingPlacementStatus.VALID;
            }
        }
        public void Placement()
        {
            if (BuildingPlacementStatus == BuildingPlacementStatus.INVALID) return;
            if (BuildingStatus != BuildingStatus.PENDING) return;
            // TODO
            BuildingStatus = BuildingStatus.PLACED;
        }
        
        /*
         * IBuildable
         */
        public BuildingStatus BuildingStatus
        {
            get => buildingStatus;
            set
            {
                if (value == BuildingStatus.PENDING)
                {
                    ((IPlaceable)this).Collider.isTrigger = true;
                }
                if (value is BuildingStatus.PLACED or BuildingStatus.BUILT)
                {
                    ((IPlaceable)this).Collider.isTrigger = false;
                    var material = Resources.Load<Material>(GameResources.PathToLoadBuildingMaterial);
                    foreach (var buildingPart in ((IPlaceable)this).BuildingParts)
                    {
                        buildingPart.GetComponent<MeshRenderer>().material = material;
                    }
                    SetTeamIndicatorMaterial(Resources.Load<Material>(GameResources.PathToLoadTeamMaterial[Owner.Faction]));
                }
                if (value == BuildingStatus.BUILT)
                {
                    transform.localScale = new Vector3(1, 1, 1);
                    Owner.MyPopulation.PopulationLimit += ((BuildingData)data).populationGain;
                    BuildingFinishedEvent.Invoke();
                }
                buildingStatus = value;
            }
        }
        protected BuildingStatus buildingStatus;
        
        public UnityEvent BuildingFinishedEvent { get; set; } = new();
        public void GetBuilt(IBuilder builder)
        {
            if (BuildingStatus == BuildingStatus.PLACED)
            {
                var maximumBuilding = Math.Min(builder.BuilderData.buildingPower, Data.maxHealth - CurrentHealth);
                CurrentHealth += maximumBuilding;
            }
        }
    }
}