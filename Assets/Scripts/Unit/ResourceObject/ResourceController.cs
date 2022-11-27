using GameManagers.Resources;
using UnityEngine;
using UnityEngine.Events;

namespace Unit.ResourceObject
{
    public class ResourceController : MonoBehaviour
    {
        public ResourceData data;

        protected int currentAmount;
        protected int CurrentAmount
        {
            get => currentAmount;
            set
            {
                currentAmount = value;
            }
        }

        public UnityEvent OnCollapsing = new();

        public void InitialiseGameObject()
        {
        }

        public ResourceValue GetHarvested(Character.Character harvester)
        {
            return null;
        }
    }
}
