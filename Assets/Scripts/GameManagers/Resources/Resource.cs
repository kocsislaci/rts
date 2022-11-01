using System;
using UnityEngine.Events;

namespace GameManagers.Resources
{
    public class Resource
    {
        public Resource(ResourceType type)
        {
            HasChanged = new UnityEvent<int>();
            this.type = type;
            Amount = 0;
        }
    
        public UnityEvent<int> HasChanged;

        public ResourceType type;
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
}
