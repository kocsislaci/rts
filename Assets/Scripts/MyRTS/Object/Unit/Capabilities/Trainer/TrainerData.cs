using System.Collections.Generic;
using UnityEngine;

namespace MyRTS.Object.Unit.Capabilities.Trainer
{
    [CreateAssetMenu(fileName = "TrainerData", menuName = "UnitData/Capabilities/TrainerData", order = 0)]
    public class TrainerData : ScriptableObject
    {
        [SerializeField] public List<TrainableActionData> trainables;
    }
}
