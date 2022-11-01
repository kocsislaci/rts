using GameManagers;
using Unit.Building;
using UnityEngine;
using CharacterController = Unit.Character.CharacterController;

namespace Unit.Skill
{
    [CreateAssetMenu(fileName = "SkillData", menuName = "SkillData/SkillData", order = 4)]
    public class SkillData : ScriptableObject
    {
        public SkillType skillType;
        
        public float castTime;
        public float cooldown;

        
        public void Trigger(GameObject source, GameObject target = null)
        {
            switch (skillType)
            {
                case SkillType.CreateUnit:
                {
                    var buildingController = source.GetComponent<BuildingController>();
                    var position = buildingController.spawnPoint.transform.position;
                    var positionToCreate = GameManager.GameManagerGameObject.GetComponent<MapManager>()
                        .SampleHeightFromWorldPosition(position);
                    new Character.Character(/*source.GetComponent<Building.Building>().Owner*/TeamEnum.Blue, positionToCreate); // TODO
                }
                    break;
                case SkillType.BuildHouse:
                    // var characterController = source.GetComponent<CharacterController>();
                    // new Character.Character(/*source.GetComponent<Building.Building>().Owner*/TeamEnum.Blue, ); // TODO
                    
                    break;
                default:
                    break;
            }
        }
    }
}
