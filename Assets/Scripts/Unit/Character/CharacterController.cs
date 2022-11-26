using System.Collections.Generic;
using GameManagers;
using UI;
using UnityEngine;

using UnityEngine.AI;

namespace Unit.Character
{
    public class CharacterController : UnitController
    {
        [SerializeField] public NavMeshAgent agent;
        
        
        
        public override void Select(bool clearSelection)
        {
            base.Select(clearSelection);
            if (GameManager.SELECTED_CHARACTERS.Contains(this)) return;
            if (clearSelection)
            {
                var selectedUnits = new List<CharacterController>(GameManager.SELECTED_CHARACTERS);
                foreach (var selectedUnit in selectedUnits)
                    selectedUnit.Deselect();
            }

            GameManager.SELECTED_CHARACTERS.Add(this);

            /*
         * Set healthBar
         */
            HealthBar = Instantiate(healthBarPrefab, healthBarCanvas, true);
            HealthBar healthBarComponent = HealthBar.GetComponent<HealthBar>();
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
            if (!GameManager.SELECTED_CHARACTERS.Contains(this)) return;
            GameManager.SELECTED_CHARACTERS.Remove(this);
            base.Deselect();

            /*
         * Off healthBar
         */
            Destroy(HealthBar);
            HealthBar = null;

            /*
         * Off selection circle
         */
            selectionCircle.SetActive(false);
        }
        
        
        
        
        public bool MoveTo(Vector3 targetPosition)
        {
            return agent.SetDestination(targetPosition);
        }
        public void Attack(Transform target)
        {
            UnitController uc = target.GetComponent<UnitController>();
            if (uc == null) return;
            uc.TakeHit(((CharacterData)representingObject.data).attackDamage);
        }
    }
    
}