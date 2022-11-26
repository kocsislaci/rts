using UnityEngine;

namespace Unit.ResourceObject
{
    public abstract class Resource
    {
        /*
         * Reference to itself in the game scene
         */
        public GameObject itself;
        protected ResourceController controller;
        
        /*
         * Preloaded data
         */
        protected ResourceData resourceData;
        
        /*
         * Variable data
         */
        protected int currentAmount;
        protected int CurrentAmount
        {
            get
            {
                return currentAmount;
            }
            set
            {
                currentAmount = value;
            }
        }

        public Resource()
        {
        }

        public Resource(Vector3 startPosition)
        {
        }

        ~Resource()
        {
            Object.Destroy(itself);
        }
    }
}
