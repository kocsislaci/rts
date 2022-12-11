using System.Collections.Generic;
using MyRTS.Object.Resource;
using MyRTS.Object.Unit;
using UnityEngine;

namespace MyRTS.GameManagers
{
    public struct StarterData
    {
        public Dictionary<ResourceType, int> starterResources;
        public Dictionary<Vector3, UnitType> starterUnitsOnRelativePosition;

        public StarterData(Dictionary<ResourceType, int> starterResources, Dictionary<Vector3, UnitType> starterUnitsOnRelativePosition)
        {
            this.starterResources = starterResources;
            this.starterUnitsOnRelativePosition = starterUnitsOnRelativePosition;
        }
    }
}
