using UnityEngine;

[CreateAssetMenu(fileName = "New Soldier Settings", menuName = "Units/SoldierDescription")]
public class SoldierSettings : UnitSettings
{
    public float speed;
    public int population;
    public float damage;
    public float attackSpeed;
    public float currentHealth;
    public float maxHealth;
    public float defense;

    
}