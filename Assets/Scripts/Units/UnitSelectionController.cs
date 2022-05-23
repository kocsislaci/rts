using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UnitSelectionController : SelectionController
{
    public override void Select(bool clearSelection)
    {
        if (RtsGameManager.GameManager.SELECTED_UNITS.Contains(this)) return;
        if (clearSelection)
        {
            List<UnitSelectionController> selectedUnits = new List<UnitSelectionController>(RtsGameManager.GameManager.SELECTED_UNITS);
            foreach (UnitSelectionController um in selectedUnits)
                um.Deselect();
        }
        RtsGameManager.GameManager.SELECTED_UNITS.Add(this);
        selectionCircle.SetActive(true);
    }
    public override void Deselect()
    {
        if (!RtsGameManager.GameManager.SELECTED_UNITS.Contains(this)) return;
        RtsGameManager.GameManager.SELECTED_UNITS.Remove(this);
        selectionCircle.SetActive(false);
    }
}
