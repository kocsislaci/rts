using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRowStrategy : IMoveStrategies
{
    public Vector3 CalculateMovementStrategy(Vector3? targetPos, int posInForm)
    {
        int unitNumber = RtsGameManager.GameManager.SELECTED_UNITS.Count;
        int rowLengt = 5;
        Vector3 targetBasePos = (Vector3)targetPos; // TODO
        Vector3 averageStartPos = Vector3.zero;
        foreach (var selectedUnit in RtsGameManager.GameManager.SELECTED_UNITS)
        {
            averageStartPos += selectedUnit.transform.position;
        }
        averageStartPos /= unitNumber;
        Vector3 firstUnitPoint = RtsGameManager.GameManager.SELECTED_UNITS[0].transform.position;
        Vector3 direction = (targetBasePos - averageStartPos).normalized;
        return targetBasePos + (posInForm % rowLengt) * Vector3.Cross(direction, Vector3.up)
            + -direction * (float) Math.Truncate((double) (posInForm / rowLengt));
    }
}
