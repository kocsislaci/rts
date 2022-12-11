using UnityEngine;

namespace MyRTS.Object.Unit.Capabilities.Attacker
{
    [CreateAssetMenu(fileName = "AttackerData", menuName = "UnitData/Capabilities/AttackerData", order = 0)]
    public class AttackerData : ScriptableObject
    {
        [SerializeField] public int attackPower;
        [SerializeField] public float attackSpeed;
    }
}