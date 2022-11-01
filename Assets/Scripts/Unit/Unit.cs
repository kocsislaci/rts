using System.Collections.Generic;
using Unit.Skill;
using UnityEngine;
using UnityEngine.Events;

namespace Unit
{
    public abstract class Unit
    {
        /*
         * Reference to itself in the game scene
         */
        protected GameObject itself;
        protected UnitController controller;

        /*
         * Preloaded data
         */
        public UnitData data;

        /*
         * Static data
         */
        protected TeamEnum owner;
        public TeamEnum Owner
        {
            get => owner;
            set { owner = value; }
        }

        /*
         * Variable data
         */
        protected int currentHealth;
        public int CurrentHealth
        {
            get { return currentHealth; }
            set { currentHealth = value; }
        }

        protected List<SkillController> skillControllers;
        public List<SkillController> SkillControllers { get => skillControllers; }

        
        protected Unit(TeamEnum owner, Vector3 startPosition)
        {
            
        }
        
        protected void InitializeSkillControllers()
        {
            skillControllers = new List<SkillController>();
            SkillController skillController;
            foreach (SkillData skill in data.skills)
            {
                skillController = itself.AddComponent<SkillController>();
                skillController.Initialize(skill, itself);
                skillControllers.Add(skillController);
            }
        }
        
        public void TriggerSkill(int index, GameObject target = null)
        {
            skillControllers[index].Trigger(target);
        }
        ~Unit()
        {
            Object.Destroy(itself);
        }
    }
}
