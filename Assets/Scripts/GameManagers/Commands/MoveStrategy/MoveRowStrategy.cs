using System;
using UnityEngine;

namespace Units.MoveStrategy
{
    public class MoveRowStrategy : IMoveStrategies
    {    
        public Vector3 CalculateMovementStrategy(Vector3? targetPos, int posInForm)
        {
            int unitNumber = GameManagers.GameManager.MY_SELECTED_CHARACTERS.Count;
            const int rowLenght = 5;
            Vector3 targetBasePos = (Vector3)targetPos; // TODO
            Vector3 averageStartPos = Vector3.zero;
            foreach (var selectedUnit in GameManagers.GameManager.MY_SELECTED_CHARACTERS)
            {
                averageStartPos += selectedUnit.transform.position;
            }
            averageStartPos /= unitNumber;
            Vector3 firstUnitPoint = GameManagers.GameManager.MY_SELECTED_CHARACTERS[0].transform.position;
            Vector3 direction = (targetBasePos - averageStartPos).normalized;
            return targetBasePos + (posInForm % rowLenght) * Vector3.Cross(direction, Vector3.up)
                                 + -direction * (float) Math.Truncate((double) (posInForm / rowLenght));        
        }
    }
}
