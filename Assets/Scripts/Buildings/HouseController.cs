using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HouseController : BuildingController
{
    public int populationCapacity = 10;
    
    private void Awake()
    {
        RtsGameManager.GameManager.BUILDINGS.Add(this);
    }

    private void Start()
    {
        RtsGameManager.GameManager.population.PopulationLimit += populationCapacity;
    }

    private void OnDestroy()
    {
        RtsGameManager.GameManager.population.PopulationLimit -= populationCapacity;
    }
}
