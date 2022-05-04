using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RtsGameManager
{
    public class GameManager : RtsPattern.Singleton
    {
        public static List<UnitSelectionController> SELECTED_UNITS = new List<UnitSelectionController>();
    }
}

