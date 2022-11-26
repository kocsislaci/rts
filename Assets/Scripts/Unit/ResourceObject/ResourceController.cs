using GameManagers.Resources;
using UnityEngine;

namespace Unit.ResourceObject
{
    public class ResourceController : MonoBehaviour
    {
        public Resource selfClass;

        public void InitialiseGameObject(Resource selfClass)
        {
            this.selfClass = selfClass;
        }

        public ResourceValue GetHarvested(Character.Character harvester)
        {
            return null;
        }
    }
}
