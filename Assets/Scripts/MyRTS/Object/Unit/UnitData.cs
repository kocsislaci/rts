using UnityEngine;

namespace MyRTS.Object.Unit
{
    public class UnitData : ScriptableObject
    {
        [Header("General info")]
        [SerializeField] public UnitType type;
        
        [Header("Unit stats")]
        [SerializeField] public int maxHealth;
        [SerializeField] public int defense;
    }
}
