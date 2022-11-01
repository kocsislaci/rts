using GameManagers;
using GameManagers.Resources;
using UnityEngine;

namespace Unit.Building
{
    public class Building: Unit
    {
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
        public Building(TeamEnum owner, Vector3 startPosition): base(owner, startPosition)
        {
            // - initialize the fields
            data = Resources.Load<BuildingData>(GameManager.PathToLoadData[WhatToLoadEnum.MainBuilding]);
            Owner = owner;
            CurrentHealth = data.maxHealth;
            
            // - save reference to global scope
            GameManager.BUILDINGS.Add(this);
            
            //
            GameManager.Population.PopulationLimit += ((BuildingData)data).populationGain;

            // - instantiate the gameObject.
            itself = Object.Instantiate(data.prefab, startPosition, Quaternion.identity);
            controller = itself.GetComponent<BuildingController>();
            controller.InitialiseGameObject(owner, this);
            // take care of the controller initialization

            InitializeSkillControllers();
        }

        ~Building()
        {
            GameManager.Population.PopulationLimit -= ((BuildingData)data).populationGain;
        }
        // attack, gather, unload, build, move, receiveDamage, die
    }
}
