using System.Collections.Generic;
using MyRTS.Object.Resource;
using UnityEngine;

namespace MyRTS.Object.Unit.Capabilities.Trainer
{
    [CreateAssetMenu(fileName = "TrainableActionData", menuName = "ActionData/TrainableActionData", order = 0)]
    public class TrainableActionData : ScriptableObject
    {
        [SerializeField] public UnitType characterType;
        [Header("1: gold, 2: stone, 3: wood")] [SerializeField] public List<int> costsInList = new();
        public Dictionary<ResourceType, int> Costs =>
            new() 
            {
                { ResourceType.Gold, costsInList[0] },
                { ResourceType.Stone, costsInList[1] },
                { ResourceType.Wood, costsInList[2] },
            };

        [SerializeField] public int populationCost;
        [SerializeField] public float timeToTrain;
    }
}
