using System.Collections.Generic;
using MyRTS.Object.Resource;
using UnityEngine;

namespace MyRTS.Object.Unit.Capabilities.Builder
{
    [CreateAssetMenu(fileName = "BuildableActionData", menuName = "ActionData/BuildableActionData", order = 0)]
    public class BuildableActionData : ScriptableObject
    {
        [SerializeField] public UnitType buildingType;
        [Header("1: gold, 2: stone, 3: wood")] [SerializeField] public List<int> costsInList = new();
        public Dictionary<ResourceType, int> Costs =>
            new() 
            {
                { ResourceType.Gold, costsInList[0] },
                { ResourceType.Stone, costsInList[1] },
                { ResourceType.Wood, costsInList[2] },
            };    
    }
}
