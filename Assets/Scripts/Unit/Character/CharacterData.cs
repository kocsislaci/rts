using System.Collections.Generic;
using GameManagers.Resources;
using UnityEngine;

namespace Unit.Character
{
    [CreateAssetMenu(fileName = "CharacterData", menuName = "CharacterData/CharacterData", order = 0)]
    public class CharacterData : UnitData
    {
        [Header("General info")]
        [SerializeField] public float speed;
        [SerializeField] public float reach;

        [Header("Gathering")]
        [SerializeField] public int maxLoad;
        [SerializeField] public int gatherUnit;
        [SerializeField] public float gatherSpeed;

        [Header("Building")]
        [SerializeField] public int buildingUnit;
        [SerializeField] public float buildingSpeed;

        [Header("Fighting")]
        [SerializeField] public float attackDamage;
        [SerializeField] public float attackSpeed;
        
        [Header("Costs")]
        [SerializeField] public int populationCost;
        [SerializeField] public int timeToTrain;
    }
}
