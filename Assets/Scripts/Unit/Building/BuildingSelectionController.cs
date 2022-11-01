using System.Collections.Generic;
using GameManagers;
using UI;
using Unit.Character;

namespace Unit.Building
{
    public class BuildingSelectionController : UnitSelectionController
    {
        public override void Select(bool clearSelection)
        {
            if (GameManager.SELECTED_BUILDINGS.Contains(this)) return;
            if (clearSelection)
            {
                var selectedUnits = new List<CharacterSelectionController>(GameManager.SELECTED_CHARACTERS);
                foreach (var selectedUnit in selectedUnits)
                    selectedUnit.Deselect();
            }
            GameManager.SELECTED_BUILDINGS.Add(this);
            
            /*
         * Set healthBar
         */
            HealthBar = Instantiate(healthBarPrefab, healthBarCanvas, true);
            HealthBar healthBarComponent = HealthBar.GetComponent<HealthBar>();
            healthBarComponent.Initialize(this.transform);
            healthBarComponent.SetPosition();
            
            /*
             * Set circle
             */
            selectionCircle.SetActive(true);
            
            Icon = Instantiate(iconPrefab, unitListPanel, true);
        }
        public override void Deselect()
        {
            if (!GameManager.SELECTED_BUILDINGS.Contains(this)) return;
                GameManager.SELECTED_BUILDINGS.Remove(this);
                
                /*
            * Off healthBar
            */
                Destroy(HealthBar);
                HealthBar = null;

                /*
             * Off selection circle
             */
                selectionCircle.SetActive(false);

                /*
                 * Set UI
                 */
                Destroy(Icon);
                Icon = null;
        }
    }
}
