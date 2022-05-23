using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RtsGameManager
{
    public class GameManager : RtsPattern.Singleton
    {
        public static List<UnitController> UNITS = new List<UnitController>(); 
        public static List<BuildingController> BUILDINGS = new List<BuildingController>();

        public static List<UnitSelectionController> SELECTED_UNITS = new List<UnitSelectionController>(); 
        public static List<BuildingSelectionController> SELECTED_BUILDINGS = new List<BuildingSelectionController>();
        public static ICommand SELECTED_COMMAND;
        public static IMoveStrategies SELECTED_STRATEGY;
        public static List<Resource> Resources = new List<Resource>()
        {
            new Resource("gold"),
            new Resource("stone"),
            new Resource("wood")
        };
        public static Population population = new Population(maxPopulation: 200);
    }
}
public class Resource
{
    public Resource(String name)
    {
        HasChanged = new UnityEvent<int>();
        this.name = name;
        Amount = 0;
    }
    
    public UnityEvent<int> HasChanged;

    public string name;
    private int _amount;
    public int Amount
    {
        get => _amount;
        set
        {
            _amount = value;
            HasChanged.Invoke(_amount);
        }
    }
}

public class Population
{
    public Population(int maxPopulation)
    {
        PopulationHasChanged = new UnityEvent<int>();
        PopulationLimitHasChanged = new UnityEvent<int>();

        MaxPopulation = maxPopulation;
        ActualPopulation = 0;
        PopulationLimit = 0;
    }
    
    public UnityEvent<int> PopulationHasChanged;
    public UnityEvent<int> PopulationLimitHasChanged;

    private int MaxPopulation;
    
    private int _actualPopulation;
    public int ActualPopulation
    {
        get => _actualPopulation;
        set
        {
            _actualPopulation = value;
            PopulationHasChanged.Invoke(value);
        }
    }
    private int _populationLimit;
    public int PopulationLimit
    {
        get => _populationLimit;
        set
        {
            if (value > MaxPopulation)
                _populationLimit = MaxPopulation;
            else
                _populationLimit = value;
            PopulationLimitHasChanged.Invoke(value);
        }
    }
}

