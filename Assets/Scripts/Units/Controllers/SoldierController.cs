using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierController : UnitController
{
    [SerializeField]
    public SoldierSettings _settings;
    public SoldierDescription description;
    void Start()
    {
        _settings = Resources.Load<SoldierSettings>("Units/SoldierSettings");
        RtsGameManager.GameManager.population.ActualPopulation += _settings.population;
    }


    private void OnDestroy()
    {
        RtsGameManager.GameManager.population.ActualPopulation -= _settings.population;
    }
}
