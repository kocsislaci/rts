using System.Collections.Generic;
using GameManagers;
using UI;
using UnityEngine;
using CharacterController = Unit.Character.CharacterController;

namespace Unit.Building
{
    public class BuildingController : UnitController
    {
        
        public override float CurrentHealth
        {
            get => currentHealth;
            set
            {
                if (BuildingStatus == BuildingStatus.PLACED)
                {
                    transform.localScale = new Vector3(1, CurrentHealth / data.maxHealth, 1);
                }
                if (value > data.maxHealth)
                {
                    base.CurrentHealth = data.maxHealth;
                }
                else
                {
                    if (value <= 0)
                    {
                        GameManager.MyPopulation.PopulationLimit -= ((BuildingData)data).populationGain;
                    }
                    base.CurrentHealth = value;
                }
            }
        }
        
        protected BuildingStatus buildingStatus;
        public BuildingStatus BuildingStatus
        {
            get => buildingStatus;
            set
            {
                if (value == BuildingStatus.PENDING)
                {
                    collider.isTrigger = true;
                }
                if (value is BuildingStatus.PLACED or BuildingStatus.BUILT)
                {
                    collider.isTrigger = false;
                    var material = Resources.Load<Material>(GameManager.PathToLoadBuildingMaterial);
                    foreach (var buildingPart in buildingParts)
                    {
                        buildingPart.GetComponent<MeshRenderer>().material = material;
                    }
                    teamIndicator.GetComponent<MeshRenderer>().material =
                        Resources.Load<Material>(GameManager.PathToLoadTeamMaterial[Owner]);
                }
                if (value == BuildingStatus.BUILT)
                {
                    GameManager.MyPopulation.PopulationLimit += ((BuildingData)data).populationGain;
                }
                buildingStatus = value;
            }
        }
        
        protected BuildingPlacementStatus buildingPlacementStatus;
        public BuildingPlacementStatus BuildingPlacementStatus
        {
            get => buildingPlacementStatus;
            set
            {
                var material = Resources.Load<Material>(GameManager.PathToLoadBuildingPlacementStatusMaterial[value]);
                foreach (var buildingPart in buildingParts)
                {
                    buildingPart.GetComponent<MeshRenderer>().material = material;
                }
                buildingPlacementStatus = value;
            }
        }
        
        [SerializeField] public GameObject spawnPoint;
        [SerializeField] public GameObject rallyPoint;
        
        private int collosions = 0;
        [SerializeField] private BoxCollider collider;
        [SerializeField] private List<GameObject> buildingParts;


        public override void InitialiseGameObject(Team owner)
        {
            InitialiseGameObject(owner);
        }
        public virtual void InitialiseGameObject(Team owner, bool isAlreadyBuilt = false)
        {
            base.InitialiseGameObject(owner);

            BuildingPlacementStatus = BuildingPlacementStatus.VALID;
            
            if (isAlreadyBuilt)
            {
                BuildingStatus = BuildingStatus.BUILT;
                CurrentHealth = data.maxHealth;
            }
            else
            {
                BuildingStatus = BuildingStatus.PENDING;
                CurrentHealth = 1;
            }
            transform.localScale = new Vector3(1, CurrentHealth / data.maxHealth, 1);
        }
        

        
        
        
        public override void Select(bool clearSelection)
        {
            if (GameManager.MY_SELECTED_BUILDINGS.Contains(this)) return;
            if (clearSelection)
            {
                var selectedUnits = new List<CharacterController>(GameManager.MY_SELECTED_CHARACTERS);
                foreach (var selectedUnit in selectedUnits)
                    selectedUnit.Deselect();
            }
            GameManager.MY_SELECTED_BUILDINGS.Add(this);
            
            /*
         * Set healthBar
         */
            healthBar = Instantiate(healthBarPrefab, healthBarCanvas, true);
            HealthBar healthBarComponent = healthBar.GetComponent<HealthBar>();
            healthBarComponent.Initialize(transform);
            healthBarComponent.SetPosition();
            
            /*
             * Set circle
             */
            selectionCircle.SetActive(true);
        }
        public override void Deselect()
        {
            if (!GameManager.MY_SELECTED_BUILDINGS.Contains(this)) return;
            GameManager.MY_SELECTED_BUILDINGS.Remove(this);
                
            /*
        * Off healthBar
        */
            Destroy(healthBar);
            healthBar = null;

            /*
         * Off selection circle
         */
            selectionCircle.SetActive(false);
        }
        
        
        
        
        
        
        protected void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Terrain")) return;
            if (BuildingStatus != BuildingStatus.PENDING) return;
            
            collosions++;
            if (collosions > 1 && BuildingPlacementStatus != BuildingPlacementStatus.INVALID)
            {
                BuildingPlacementStatus = BuildingPlacementStatus.INVALID;
            }
        }
        protected void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Terrain")) return;
            if (BuildingStatus != BuildingStatus.PENDING) return;

            collosions--;
            if (collosions == 0)
            {
                BuildingPlacementStatus = BuildingPlacementStatus.VALID;
            }
        }
        public void Placement()
        {
            if (BuildingPlacementStatus == BuildingPlacementStatus.INVALID) return;
            if (BuildingStatus != BuildingStatus.PENDING) return;

            BuildingStatus = BuildingStatus.PLACED;
        }
        
        
        
        
        public void GetBuilt(CharacterController builder)
        {
            // if (BuildingStatus == BuildingStatus.PLACED)
            // {
            //     CurrentHealth += ((CharacterData)builder.data).buildingUnit;
            //     transform.localScale = new Vector3(1, CurrentHealth / ((BuildingData)data).maxHealth, 1);
            //     if (CurrentHealth == ((BuildingData)data).maxHealth)
            //     {
            //         ((Building)representingObject).BuildingStatus = BuildingStatus.BUILT;
            //     }
            // }
            // else
            // {
            //     Debug.Log("not placed or already built");
            // }
        }
    }
}
