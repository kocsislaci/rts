using UnityEngine;

namespace MyRTS.Object.Resource
{
    [CreateAssetMenu(fileName = "ResourceData", menuName = "ResourceData/ResourceData", order = 0)]
    public class ResourceData : ScriptableObject
    {
        [SerializeField] public ResourceType type;
        [SerializeField] public int maxHarvestable;
    }
}
