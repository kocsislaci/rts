using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSelectionController : SelectionController
{
    public override void Select(bool clearSelection)
    {
        if (RtsGameManager.GameManager.SELECTED_BUILDINGS.Contains(this)) return;
        if (clearSelection)
        {
            List<UnitSelectionController> selectedUnits = new List<UnitSelectionController>(RtsGameManager.GameManager.SELECTED_UNITS);
            foreach (UnitSelectionController um in selectedUnits)
                um.Deselect();
        }
        RtsGameManager.GameManager.SELECTED_BUILDINGS.Add(this);
        selectionCircle.SetActive(true);
    }
    public override void Deselect()
    {
        if (!RtsGameManager.GameManager.SELECTED_BUILDINGS.Contains(this)) return;
        RtsGameManager.GameManager.SELECTED_BUILDINGS.Remove(this);
        selectionCircle.SetActive(false);
    }
}
