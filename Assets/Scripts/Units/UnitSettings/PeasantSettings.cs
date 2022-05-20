using UnityEngine;

[CreateAssetMenu(fileName = "New Peasant Settings", menuName = "Units/PeasantDescription")]
public class PeasantSettings : UnitSettings
{
    public float speed;
    public float currentHealth;
    public float maxHealth;
    public float defense;
    public float gatherSpeed;
    public float currentLoad;
    public float maxLoad;
}