using UnityEngine;

namespace RTS.Object.Unit.Capabilities.Harvester
{
    [CreateAssetMenu(fileName = "HarvesterData", menuName = "UnitData/Capabilities/HarvesterData", order = 0)]
    public class HarvesterData : ScriptableObject
    {
        [SerializeField] public int maxHarvested;
        [SerializeField] public int harvestingPower;
        [SerializeField] public float harvestingSpeed;
    }
}
