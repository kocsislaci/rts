using GameManagers;
using GameManagers.Resources;
using UnityEngine;

namespace Unit.Character
{
    public class Character: Unit
    {
        protected int currentLoad;
        public int CurrentLoad
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
        
        /// <summary>
        /// Constructor
        /// 
        /// Responsible to
        /// - initialize the fields,
        /// - save reference to global scope,
        /// - instantiate the gameObject.
        /// </summary>
        /// <param name="owner"> Sets the owner of the object </param>
        /// <param name="startPosition"> Sets the start position of the object</param>
        public Character(TeamEnum owner, Vector3 startPosition) : base(owner, startPosition)
        {
            // - initialize the fields
            data = Resources.Load<CharacterData>(GameManager.PathToLoadData[WhatToLoadEnum.Character]);
            Owner = owner;
            CurrentHealth = data.maxHealth;
            CurrentLoad = 0;
            
            // - save reference to global scope
            GameManager.CHARACTERS.Add(this);
            
            //
            GameManager.Population.ActualPopulation += ((CharacterData)data).populationCost;
            
            
            // - instantiate the gameObject.
            itself = Object.Instantiate(data.prefab, startPosition, Quaternion.identity);
            controller = itself.GetComponent<CharacterController>();
            controller.InitialiseGameObject(owner, this);
            // take care of the controller initialization

        }

        ~Character()
        {
            GameManager.Population.ActualPopulation -= ((CharacterData)data).populationCost;
        }

        // attack, gather, unload, build, move, receiveDamage, die
    }
}
