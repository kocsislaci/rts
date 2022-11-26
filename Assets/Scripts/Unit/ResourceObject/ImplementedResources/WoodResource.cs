using GameManagers;
using GameManagers.Resources;
using UnityEngine;
using Resource = Unit.ResourceObject.Resource;

namespace Unit.ResourceObject.ImplementedResources
{
    public class WoodResource: Resource
    {
        public WoodResource(Vector3 startPosition)
        {
            // - initialize the fields
            resourceData = Resources.Load<ResourceData>(GameManager.PathToLoadData[UnitType.Wood]);
            //
            
            CurrentAmount = resourceData.maxAmount;
            
            // // - instantiate the gameObject.
            itself = Object.Instantiate(resourceData.prefab, startPosition, Quaternion.identity);
            controller = itself.GetComponent<ResourceController>();
            controller.InitialiseGameObject(this);
        }
    }
}