using GameManagers;
using GameManagers.Resources;
using Unit.Character;
using UnityEngine;

namespace Unit.Building
{
    public class Building: Unit
    {
        protected BuildingStatus buildingStatus;
        public BuildingStatus BuildingStatus
        {
            get => buildingStatus;
            set => buildingStatus = value;
        }
        protected BuildingPlacementStatus buildingPlacementStatus;
        public BuildingPlacementStatus BuildingPlacementStatus
        {
            get => buildingPlacementStatus;
            set
            {
                // TODO set materials and params?
                buildingPlacementStatus = value;
            }
        }
        public override int CurrentHealth
        {
            get => currentHealth;
            set
            {
                currentHealth = value;
                if (currentHealth < 1)
                {
                    GameManager.BUILDINGS.Remove(this);
                }
                if (value > data.maxHealth)
                {
                    currentHealth = data.maxHealth;
                }
            }
        }

        public Building(Team owner, Vector3 startPosition, bool isAlreadyBuilt = false) : base(owner, startPosition)
        {
            unitType = UnitType.MainBuilding;

            // - initialize the fields
            data = Resources.Load<BuildingData>(GameManager.PathToLoadData[UnitType.MainBuilding]);
            
            // - save reference to global scope
            GameManager.BUILDINGS.Add(this);
            
            BuildingPlacementStatus = BuildingPlacementStatus.VALID; // by default reckon that its valid
            if (isAlreadyBuilt)
            {
                BuildingStatus = BuildingStatus.BUILT;
                CurrentHealth = data.maxHealth;
            }
            else
            {
                BuildingStatus = BuildingStatus.PENDING; // Has to be built before be able to use Actions
                CurrentHealth = 1;
            }
            
            // Population
            GameManager.Population.PopulationLimit += ((BuildingData)data).populationGain;

            // instantiate the gameObject.
            sceneGameObject = Object.Instantiate(data.prefab, startPosition, Quaternion.identity);
            controller = sceneGameObject.GetComponent<BuildingController>();
            controller.InitialiseGameObject(owner, this); // Has to set phantomMaterial and a callback to later events
            InitializeSkillControllers();
        }
        ~Building()
        {
            GameManager.Population.PopulationLimit -= ((BuildingData)data).populationGain;
        }
    }
}
