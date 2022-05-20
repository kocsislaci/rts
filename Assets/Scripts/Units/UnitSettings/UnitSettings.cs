using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Unit Settings", menuName = "Units/UnitDescription")]
public class UnitSettings : ScriptableObject
{
    public string unitName;
    public Color color;
    public int team;
}