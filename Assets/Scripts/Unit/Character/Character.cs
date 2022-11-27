using GameManagers;
using UnityEngine;

namespace Unit.Character
{
    public class Character: Unit
    {
        public Character(Team owner, Vector3 startPosition, Vector3? rallyPosition = null) : base()
        {
            GameManager.MY_CHARACTERS.Add(uuid ,this);

            var prefab = Resources.Load<GameObject>(GameManager.PathToLoadUnitPrefab[UnitType.Character]);
            gameObject = Object.Instantiate(prefab, startPosition, Quaternion.identity);
            
            var controller = gameObject.GetComponent<CharacterController>();
            controller.InitialiseGameObject(owner, rallyPosition);
            controller.OnDying.AddListener(Destroy);
        }
        public override void Destroy()
        {
            base.Destroy();
            GameManager.MY_CHARACTERS.Remove(uuid);
        }
    }
}
