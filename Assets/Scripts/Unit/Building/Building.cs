using GameManagers;
using UnityEngine;

namespace Unit.Building
{
    public class Building: Unit
    {
        public Building(Team owner, Vector3 startPosition, bool isAlreadyBuilt = false) : base()
        {
            GameManager.MY_BUILDINGS.Add(uuid, this);

            var prefab = Resources.Load<GameObject>(GameManager.PathToLoadUnitPrefab[UnitType.MainBuilding]);
            gameObject = Object.Instantiate(prefab, startPosition, Quaternion.identity);
            
            var controller = gameObject.GetComponent<BuildingController>();
            controller.InitialiseGameObject(owner, isAlreadyBuilt);
            controller.OnDying.AddListener(Destroy);
        }
        public override void Destroy()
        {
            base.Destroy();
            GameManager.MY_BUILDINGS.Remove(uuid);
        }
    }
}
