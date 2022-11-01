using GameManagers;
using GameManagers.Resources;
using UnityEngine;
using Resource = TerrainObject.ResourceObject.Resource;

namespace Unit.ResourceObject
{
    public class StoneResource: Resource
    {
        public StoneResource(Vector3 startPosition)
        {
            // - initialize the fields
            resourceData = Resources.Load<ResourceData>(GameManager.PathToLoadData[WhatToLoadEnum.Stone]);
            //
            
            CurrentAmount = resourceData.amount;
            
            // // - instantiate the gameObject.
            itself = Object.Instantiate(resourceData.prefab, startPosition, Quaternion.identity);
            controller = itself.GetComponent<ResourceController>();
            controller.InitialiseGameObject();
        }
    }
}