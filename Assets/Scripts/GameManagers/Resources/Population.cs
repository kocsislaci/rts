using UnityEngine.Events;

namespace GameManagers.Resources
{
    public class Population
    {
        public Population(int maxPopulation)
        {
            OnPopulationHasChanged = new UnityEvent<int>();
            OnPopulationLimitHasChanged = new UnityEvent<int>();

            this.maxPopulation = maxPopulation;
            ActualPopulation = 0;
            PopulationLimit = 0;
        }
    
        public UnityEvent<int> OnPopulationHasChanged;
        public UnityEvent<int> OnPopulationLimitHasChanged;

        private readonly int maxPopulation;
    
        private int _actualPopulation;
        public int ActualPopulation
        {
            get => _actualPopulation;
            set
            {
                _actualPopulation = value;
                OnPopulationHasChanged.Invoke(value);
            }
        }
        private int _populationLimit;
        public int PopulationLimit
        {
            get => _populationLimit;
            set
            {
                if (value > maxPopulation)
                    _populationLimit = maxPopulation;
                else
                    _populationLimit = value;
                OnPopulationLimitHasChanged.Invoke(value);
            }
        }
    }
}
