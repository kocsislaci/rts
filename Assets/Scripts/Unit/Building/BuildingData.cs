using System.Collections.Generic;
using GameManagers.Resources;
using UnityEngine;

namespace Unit.Building
{
    [CreateAssetMenu(fileName = "BuildingData", menuName = "BuildingData/BuildingData"/*, order = 0*/)]
    public class BuildingData : UnitData
    {
        [Header("Gain")]
        [SerializeField] public int populationGain;
    }
}
