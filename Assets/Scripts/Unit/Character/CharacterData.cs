using System.Collections.Generic;
using GameManagers.Resources;
using UnityEngine;

namespace Unit.Character
{
    [CreateAssetMenu(fileName = "CharacterData", menuName = "CharacterData/CharacterData", order = 0)]
    public class CharacterData : UnitData
    {
        [Header("General info")]
        [SerializeField] public int speed;
        
        [Header("Gathering")]
        [SerializeField] public int gatherSpeed;
        [SerializeField] public int maxLoad;
        
        [Header("Building")]
        [SerializeField] public int buildSpeed;

        [Header("Fighting")]
        [SerializeField] public int attackDamage;
        [SerializeField] public int attackSpeed;
        [SerializeField] public int attackRange;

        [Header("Costs")]
        [SerializeField] public int populationCost;
        
        [Header("Skills")]
        [SerializeField] public int dummy;
    }
}
