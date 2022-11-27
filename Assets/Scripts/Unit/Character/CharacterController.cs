using System.Collections.Generic;
using GameManagers;
using UI;
using Unit.ResourceObject;
using UnityEngine;

using UnityEngine.AI;

namespace Unit.Character
{
    public class CharacterController : UnitController
    {
        protected ResourceValue currentLoad;
        public ResourceValue CurrentLoad
        {
            get => currentLoad;
            set
            {
                currentLoad = value;
            }
        }
        public override float CurrentHealth
        {
            get => currentHealth;
            set
            {
                if (value <= 0)
                {
                    GameManager.MyPopulation.ActualPopulation -= ((CharacterData)data).populationCost;
                }
                base.CurrentHealth = value;
            }
        }
        [SerializeField] public NavMeshAgent agent;
        public CharacterStance stance;
        public CharacterMood mood;
        public Vector3? destination = null;
        public GameObject target;
        public GameObject resource;
        public GameObject dropOffBuilding;
        
        
        public override void InitialiseGameObject(Team owner)
        {
            InitialiseGameObject(owner);
        }
        public virtual void InitialiseGameObject(Team owner, Vector3? rallyPosition = null)
        {
            base.InitialiseGameObject(owner);
            CurrentLoad = null;
            GameManager.MyPopulation.ActualPopulation += ((CharacterData)data).populationCost;
            if (rallyPosition != null)
            {
                destination = (Vector3)rallyPosition;
            }
        }

        
        
        public override void Select(bool clearSelection)
        {
            base.Select(clearSelection);
            if (GameManager.MY_SELECTED_CHARACTERS.Contains(this)) return;
            if (clearSelection)
            {
                var selectedUnits = new List<CharacterController>(GameManager.MY_SELECTED_CHARACTERS);
                foreach (var selectedUnit in selectedUnits)
                    selectedUnit.Deselect();
            }

            GameManager.MY_SELECTED_CHARACTERS.Add(this);

            /*
         * Set healthBar
         */
            healthBar = Instantiate(healthBarPrefab, healthBarCanvas, true);
            HealthBar healthBarComponent = healthBar.GetComponent<HealthBar>();
            healthBarComponent.Initialize(transform);
            healthBarComponent.SetPosition();

            /*
         * Set selection circle
         */
            selectionCircle.SetActive(true);

            /*
             * Set on UI
             */
        }

        public override void Deselect()
        {
            if (!GameManager.MY_SELECTED_CHARACTERS.Contains(this)) return;
            GameManager.MY_SELECTED_CHARACTERS.Remove(this);
            base.Deselect();

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
        
        
        public void SetDestination(Vector3 targetPosition)
        {
            destination = targetPosition;
        }

        public bool HasDestination()
        {
            return destination != null;
        }

        public bool MoveToDestination()
        {
            if (destination == null) return false;
            return agent.SetDestination((Vector3)destination);
        }
        
        public void Attack(Transform target)
        {
            UnitController uc = target.GetComponent<UnitController>();
            if (uc == null) return;
            uc.TakeDamage(((CharacterData)data).attackDamage);
        }
    }
}
