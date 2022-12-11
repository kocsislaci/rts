using System;
using UnityEngine;
using UnityEngine.Events;

namespace MyRTS.Object.Resource.Dto
{
    [Serializable]
    public class Population
    {
        public Population(int maxPopulation = 0)
        {
            ActualPopulation = 0;
            PopulationLimit = 0;
            MaxPopulation = maxPopulation;
        }
    
        public UnityEvent<Tuple<int, int>> PopulationChanged { get; } = new();
        
        public int ActualPopulation
        {
            get => actualPopulation;
            set
            {
                actualPopulation = value;
                PopulationChanged.Invoke(new Tuple<int, int>(actualPopulation, populationLimit));
            }
        }
        [SerializeField] private int actualPopulation;
        
        public int PopulationLimit
        {
            get => populationLimit;
            set
            {
                populationLimit = value > MaxPopulation ? MaxPopulation : value;
                PopulationChanged.Invoke(new Tuple<int, int>(actualPopulation, populationLimit));
            }
        }
        [SerializeField] private int populationLimit;
        
        [field: SerializeField] public int MaxPopulation { get; private set; }

        public Tuple<int, int> GetActualValues() => new (actualPopulation, populationLimit);
    }
}
