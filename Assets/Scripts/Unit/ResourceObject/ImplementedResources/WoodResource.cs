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
                var prefab = Resources.Load<GameObject>(GameManager.PathToLoadUnitPrefab[UnitType.Wood]);
                gameObject = Object.Instantiate(prefab, startPosition, Quaternion.identity);

                var controller = gameObject.GetComponent<ResourceController>();
                controller.InitialiseGameObject();
                controller.OnCollapsing.AddListener(Destroy);
            }
        }
}
