using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UnitSelectionController : MonoBehaviour
{
    public GameObject selectionCircle;
    
    public void Select()
    {
        Select(false);
    }
    public void Select(bool clearSelection)
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
    public void Deselect()
    {
        if (!RtsGameManager.GameManager.SELECTED_UNITS.Contains(this)) return;
        RtsGameManager.GameManager.SELECTED_UNITS.Remove(this);
        selectionCircle.SetActive(false);
    }
}
