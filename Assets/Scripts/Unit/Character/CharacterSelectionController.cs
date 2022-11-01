using System.Collections.Generic;
using GameManagers;
using UI;

namespace Unit.Character
{
    public class CharacterSelectionController : UnitSelectionController
    {
        public override void Select(bool clearSelection)
        {
            if (GameManager.SELECTED_CHARACTERS.Contains(this)) return;
            if (clearSelection)
            {
                var selectedUnits = new List<CharacterSelectionController>(GameManager.SELECTED_CHARACTERS);
                foreach (var selectedUnit in selectedUnits)
                    selectedUnit.Deselect();
            }

            GameManager.SELECTED_CHARACTERS.Add(this);

            /*
         * Set healthBar
         */
            HealthBar = Instantiate(healthBarPrefab, healthBarCanvas, true);
            HealthBar healthBarComponent = HealthBar.GetComponent<HealthBar>();
            healthBarComponent.Initialize(this.transform);
            healthBarComponent.SetPosition();

            /*
         * Set selection circle
         */
            selectionCircle.SetActive(true);

            /*
             * Set on UI
             */
            Icon = Instantiate(iconPrefab, unitListPanel, true);
        }

        public override void Deselect()
        {
            if (!GameManager.SELECTED_CHARACTERS.Contains(this)) return;
            GameManager.SELECTED_CHARACTERS.Remove(this);

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
