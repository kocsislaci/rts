using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RtsGameManager
{
    public class GameManager : RtsPattern.Singleton
    {
        
        public static List<UnitSelectionController> SELECTED_UNITS = new List<UnitSelectionController>();
        public static ICommand SELECTED_COMMAND;
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
        HasChanged = new UnityEvent();
        this.name = name;
        Amount = 0;
    }
    
    public UnityEvent HasChanged;

    public string name;
    private int _amount;
    public int Amount
    {
        get => _amount;
        set
        {
            _amount = value;
            HasChanged.Invoke();
        }
    }
}

public class Population
{
    public Population(int maxPopulation)
    {
        PopulationHasChanged = new UnityEvent();
        PopulationLimitHasChanged = new UnityEvent();

        MaxPopulation = maxPopulation;
        ActualPopulation = 0;
        PopulationLimit = 0;
    }
    
    public UnityEvent PopulationHasChanged;
    public UnityEvent PopulationLimitHasChanged;

    private int MaxPopulation;
    
    private int _actualPopulation;
    public int ActualPopulation
    {
        get => _actualPopulation;
        set
        {
            _actualPopulation = value;
            PopulationHasChanged.Invoke();
        }
    }
    private int _populationLimit;
    public int PopulationLimit
    {
        get => _populationLimit;
        set
        {
            _populationLimit = value;
            PopulationLimitHasChanged.Invoke();
        }
    }
}

