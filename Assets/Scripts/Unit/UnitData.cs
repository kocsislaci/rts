using System.Collections.Generic;
using Unit.ResourceObject;
using UnityEngine;

namespace Unit
{
    [CreateAssetMenu(fileName = "UnitData", menuName = "UnitData/UnitData", order = 0)]
    public class UnitData : ScriptableObject
    {
        [Header("General info")]
        [SerializeField] public UnitType type;
        [SerializeField] public float fieldOfView;
        
        [Header("Health")]
        [SerializeField] public float maxHealth;
        [SerializeField] public float defense;
        
        [Header("Costs")]
        [SerializeField] public List<ResourceValue> resourcesCost = new();
        
        /*[Header("Skills")]
        [SerializeField] public List<SkillData> skills = new();*/

        [Header("In game object")]
        [SerializeField] public GameObject prefab;
    }
}
