using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RtsGameManager
{
    public class GameManager : RtsPattern.Singleton
    {
        public static List<UnitManager> SELECTED_UNITS = new List<UnitManager>();
    }
}

