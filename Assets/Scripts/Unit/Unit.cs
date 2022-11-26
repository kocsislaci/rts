using System.Collections.Generic;
using Unit.Skill;
using UnityEditor;
using UnityEngine;

namespace Unit
{
    public abstract class Unit
    {
        public GUID uuid;
        public GameObject sceneGameObject;
        public UnitController controller;
        public UnitData data;
        public UnitType unitType;
        protected Team owner;
        public Team Owner
        {
            get => owner;
            set { owner = value; }
        }
        protected int currentHealth;
        public virtual int CurrentHealth
        {
            get { return currentHealth; }
            set
            {
                currentHealth = value;
            }
        }
        

        protected List<SkillController> skillControllers;
        public List<SkillController> SkillControllers { get => skillControllers; }

        
        protected Unit(Team owner, Vector3 startPosition)
        {
            Owner = owner;
            uuid = GUID.Generate();
        }
        ~Unit()
        {
            Object.Destroy(sceneGameObject);
        }
        
        
        protected void InitializeSkillControllers()
        {
            skillControllers = new List<SkillController>();
            SkillController skillController;
            foreach (SkillData skill in data.skills)
            {
                skillController = sceneGameObject.AddComponent<SkillController>();
                skillController.Initialize(skill, sceneGameObject);
                skillControllers.Add(skillController);
            }
        }
        public void TriggerSkill(int index, GameObject target = null)
        {
            skillControllers[index].Trigger(target);
        }
    }
}
