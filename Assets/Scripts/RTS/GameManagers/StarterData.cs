using System.Collections.Generic;
using RTS.Object.Resource;
using RTS.Object.Unit;
using UnityEngine;

namespace RTS.GameManagers
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
