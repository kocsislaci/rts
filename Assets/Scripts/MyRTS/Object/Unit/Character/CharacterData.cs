using UnityEngine;

namespace MyRTS.Object.Unit.Character
{
    [CreateAssetMenu(fileName = "CharacterData", menuName = "UnitData/CharacterData", order = 0)]
    public class CharacterData : UnitData
    {
        [SerializeField] public float speed;
        [SerializeField] public int populationImpact;
    }
}
