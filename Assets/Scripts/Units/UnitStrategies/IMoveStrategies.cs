using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMoveStrategies
{
    public Vector3 CalculateMovementStrategy(Vector3? targetPos, int posInForm);
}
