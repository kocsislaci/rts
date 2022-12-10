using UnityEngine;

namespace RTS.Object.Unit.Building
{
    [CreateAssetMenu(fileName = "BuildingData", menuName = "UnitData/BuildingData"/*, order = 0*/)]
    public class BuildingData : UnitData
    {
        [SerializeField] public int populationGain;
    }
}
