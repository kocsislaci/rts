using GameManagers;
using GameManagers.Resources;
using TerrainObject.ResourceObject;
using UnityEngine;
using Resource = TerrainObject.ResourceObject.Resource;

namespace Unit.ResourceObject
{
    public class GoldResource: Resource
    {
        public GoldResource(Vector3 startPosition)
        {
            // - initialize the fields
            resourceData = Resources.Load<ResourceData>(GameManager.PathToLoadData[WhatToLoadEnum.Gold]);
            //
            
            CurrentAmount = resourceData.amount;
            
            // // - instantiate the gameObject.
            itself = Object.Instantiate(resourceData.prefab, startPosition, Quaternion.identity);
            controller = itself.GetComponent<ResourceController>();
            controller.InitialiseGameObject();
        }
    }
}