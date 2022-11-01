using System;

namespace GameManagers.Resources
{
    [Serializable]
    public class ResourceValue
    {
        public ResourceType type;
        public int amount;

        public ResourceValue(ResourceType type, int amount)
        {
            this.type = type;
            this.amount = amount;
        }
    }
}
