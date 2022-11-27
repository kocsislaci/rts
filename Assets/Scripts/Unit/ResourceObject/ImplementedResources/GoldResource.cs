using GameManagers;
using GameManagers.Resources;
using UnityEngine;

namespace Unit.ResourceObject.ImplementedResources
{
    public class GoldResource: Resource
    {
        public GoldResource(Vector3 startPosition)
        {
            var prefab = Resources.Load<GameObject>(GameManager.PathToLoadUnitPrefab[UnitType.Gold]);
            gameObject = Object.Instantiate(prefab, startPosition, Quaternion.identity);

            var controller = gameObject.GetComponent<ResourceController>();
            controller.InitialiseGameObject();
            controller.OnCollapsing.AddListener(Destroy);
        }
    }
}