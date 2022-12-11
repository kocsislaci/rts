using System.Collections.Generic;
using UnityEngine;

namespace MyRTS.Object.Unit.Capabilities.Builder
{
    [CreateAssetMenu(fileName = "BuilderData", menuName = "UnitData/Capabilities/BuilderData", order = 0)]
    public class BuilderData : ScriptableObject
    {
        [SerializeField] public int buildingPower;
        [SerializeField] public float buildingSpeed;
        [SerializeField] public List<BuildableActionData> buildables;
    }
}
