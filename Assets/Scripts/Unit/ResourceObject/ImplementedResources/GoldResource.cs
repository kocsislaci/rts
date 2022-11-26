using GameManagers;
using GameManagers.Resources;
using UnityEngine;

namespace Unit.ResourceObject.ImplementedResources
{
    public class GoldResource: Resource
    {
        public GoldResource(Vector3 startPosition)
        {
            // - initialize the fields
            resourceData = Resources.Load<ResourceData>(GameManager.PathToLoadData[UnitType.Gold]);
            //
            
            CurrentAmount = resourceData.maxAmount;
            
            // // - instantiate the gameObject.
            itself = Object.Instantiate(resourceData.prefab, startPosition, Quaternion.identity);
            controller = itself.GetComponent<ResourceController>();
            controller.InitialiseGameObject(this);
        }
    }
}