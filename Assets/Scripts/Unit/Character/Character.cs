using System.Collections.Generic;
using GameManagers;
using GameManagers.Resources;
using Unit.ResourceObject;
using UnityEngine;

namespace Unit.Character
{
    public class Character: Unit
    {
        protected ResourceValue currentLoad;
        public ResourceValue CurrentLoad
        {
            get
            {
                return currentLoad;
            }
            set
            {
                currentLoad = value;
            }
        }
        public override int CurrentHealth
        {
            get { return currentHealth; }
            set
            {
                currentHealth = value;
                if (currentHealth < 1)
                {
                    GameManager.CHARACTERS.Remove(this);
                }
            }
        }
        
        
        public Character(Team owner, Vector3 startPosition) : base(owner, startPosition)
        {
            unitType = UnitType.Character;
            
            // - initialize the fields
            data = Resources.Load<CharacterData>(GameManager.PathToLoadData[UnitType.Character]);
            CurrentHealth = data.maxHealth;
            CurrentLoad = new ResourceValue(ResourceType.Gold, 0);
            
            // - save reference to global scope
            GameManager.CHARACTERS.Add(this);
            
            // Population
            GameManager.Population.ActualPopulation += ((CharacterData)data).populationCost;

            // - instantiate the gameObject.
            sceneGameObject = Object.Instantiate(data.prefab, startPosition, Quaternion.identity);
            controller = sceneGameObject.GetComponent<CharacterController>();
            controller.InitialiseGameObject(owner, this);
            InitializeSkillControllers();
        }
        ~Character()
        {
            GameManager.Population.ActualPopulation -= ((CharacterData)data).populationCost;
        }
    }
}
