using GameManagers.Resources;
using UnityEngine;

namespace Unit.ResourceObject
{
    [CreateAssetMenu(fileName = "ResourceData", menuName = "ResourceData/ResourceData", order = 0)]
    public class ResourceData : ScriptableObject
    {
        [SerializeField] public ResourceType type;
        [SerializeField] public int maxAmount;
        
        [Header("In game object")]
        [SerializeField] public GameObject prefab;
    }
}
