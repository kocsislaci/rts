using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public GameObject selectionCircle;
    private void OnMouseDown()
    {
        Select(true);
    }
    public void Select() { Select(false); }
    public void Select(bool clearSelection)
    {
        if (RtsGameManager.GameManager.SELECTED_UNITS.Contains(this)) return;
        if (clearSelection)
        {
            List<UnitManager> selectedUnits = new List<UnitManager>(RtsGameManager.GameManager.SELECTED_UNITS);
            foreach (UnitManager um in selectedUnits)
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
