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
                case SkillType.CreateCharacter:
                {
                    var buildingController = source.GetComponent<BuildingController>();
                    var position = buildingController.spawnPoint.transform.position;
                    var positionToCreate = GameManager.GameManagerGameObject.GetComponent<MapManager>().SampleHeightFromWorldPosition(position);
                    new Character.Character(buildingController.representingObject.Owner, positionToCreate); // TODO
                    break;
                }
                case SkillType.BuildMainBuilding:
                {
                    
                    break;
                }
                case SkillType.BuildHouse:
                {
                    
                    
                    break;
                }
                    // var characterController = source.GetComponent<CharacterController>();
                    // new Character.Character(/*source.GetComponent<Building.Building>().Owner*/TeamEnum.Blue, ); // TODO
                    
                

                default:
                    break;
            }
        }
    }
}
