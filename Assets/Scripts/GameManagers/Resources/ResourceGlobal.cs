using System;
using Unit.ResourceObject;
using UnityEngine.Events;

namespace GameManagers.Resources
{
    [Serializable]
    public class ResourceGlobal
    {
        public ResourceGlobal(ResourceType type)
        {
            HasChanged = new UnityEvent<ResourceType, int>();
            this.type = type;
            Amount = 0;
        }
    
        public UnityEvent<ResourceType, int> HasChanged;

        public ResourceType type;
        private int _amount;
        public int Amount
        {
            get => _amount;
            set
            {
                _amount = value;
                HasChanged.Invoke(type ,_amount);
            }
        }
    }
}
