// using System;
// using UnityEngine;
//
// namespace Units.MoveStrategy
// {
//     public class MoveBoxStrategy : IMoveStrategies
//     {
//         public Vector3 CalculateMovementStrategy(Vector3? targetPos, int posInForm)
//         {
//             int unitNumber = GameManagers.GameManager.MY_SELECTED_CHARACTERS.Count;
//             Vector3 targetBasePos = (Vector3)targetPos; // TODO
//             Vector3 averageStartPos = Vector3.zero;
//             foreach (var selectedUnit in GameManagers.GameManager.MY_SELECTED_CHARACTERS)
//             {
//                 averageStartPos += selectedUnit.transform.position;
//             }
//             averageStartPos /= unitNumber;
//             Vector3 firstUnitPoint = GameManagers.GameManager.MY_SELECTED_CHARACTERS[0].transform.position;
//             Vector3 direction = (targetBasePos - averageStartPos).normalized;
//             float boxSideSize = unitNumber / 4.0f;
//             boxSideSize = (float) (Math.Ceiling(boxSideSize));
//             Vector3 relPos = Vector3.zero;
//             float i = (float)Math.Truncate((float) (posInForm / 4));
//             switch (posInForm % 4)
//             {
//                 case 0:
//                     relPos += Vector3.zero + i * Vector3.Cross(direction, Vector3.up);
//                     break;
//                 case 1:
//                     relPos += boxSideSize * Vector3.Cross(direction, Vector3.up) + i * -direction;
//                     break;
//                 case 2:
//                     relPos += boxSideSize * ((Vector3.Cross(direction, Vector3.up)) + -direction) - i * Vector3.Cross(direction, Vector3.up);
//                     break;
//                 case 3:
//                     relPos += boxSideSize * -direction - i * -direction;
//                     break;
//             }
//             return targetBasePos + relPos;
//         }
//     }
// }
