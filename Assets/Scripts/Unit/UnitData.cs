using System.Collections.Generic;
using GameManagers.Resources;
using Unit.ResourceObject;
using Unit.Skill;
using UnityEngine;

namespace Unit
{
    [CreateAssetMenu(fileName = "UnitData", menuName = "UnitData/UnitData", order = 0)]
    public class UnitData : ScriptableObject
    {
        [Header("General info")]
        [SerializeField] public UnitType type;
        [SerializeField] public int fieldOfView;
        
        [Header("Health")]
        [SerializeField] public int maxHealth;
        [SerializeField] public int defense;
        
        [Header("Costs")]
        [SerializeField] public List<ResourceValue> resourcesCost = new List<ResourceValue>();
        
        [Header("Skills")]
        [SerializeField] public List<SkillData> skills = new List<SkillData>();

        [Header("In game object")]
        [SerializeField] public GameObject prefab;
    }
}
