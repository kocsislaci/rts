using System.Collections.Generic;
using GameManagers.Resources;
using Unit;
using Unit.Building;
using Unit.Character;
using Unit.ResourceObject;
using Unit.ResourceObject.ImplementedResources;
// using Unit.Skill;
using UnityEditor;
using UnityEngine;

namespace GameManagers
{
    public static class GameManager
    {
        /*
         * Initialized for neutral values
         */
        static GameManager()
        {
            MyTeam = Team.Neutral;
            InputState = UserInputState.NON_SELECTED;
        }
        
        /*
         * GameManager gameObject in the scene
         */
        private static GameObject _gameManagerGameObject;
        public static GameObject GameManagerGameObject
        {
            get
            {
                if (_gameManagerGameObject == null)
                {
                    _gameManagerGameObject = GameObject.Find("GameManager");
                }

                return _gameManagerGameObject;
            }
            private set {}
        }

        /*
         * Local player information
         */
        public static Team MyTeam { get; set; }
        /*
         * Player resources and population data
         */
        public static readonly Dictionary<ResourceType, ResourceGlobal> MyResources = new()
        {
            { ResourceType.Gold, new(ResourceType.Gold) },
            { ResourceType.Stone, new(ResourceType.Stone) },
            { ResourceType.Wood, new(ResourceType.Wood) },
        };
        public static readonly Population MyPopulation = new(maxPopulation: 200);
        /*
         * Local player mutable state
         */
        public static UserInputState InputState { get; set; }
        /*
         * Player objects in the gameSpace
         */
        public static readonly Dictionary<GUID ,Unit.Unit> MY_UNITS = new();
        public static readonly Dictionary<GUID, Character> MY_CHARACTERS = new();
        public static readonly Dictionary<GUID, Building> MY_BUILDINGS = new();
        public static readonly List<UnitController> MY_SELECTED_UNITS = new();
        public static readonly List<Unit.Character.CharacterController> MY_SELECTED_CHARACTERS = new(); 
        public static readonly List<BuildingController> MY_SELECTED_BUILDINGS = new();
        
        
        
        
        
        
        
        
        
        
        /*
         * Neutral objects in the gameSpace
         */
        public static readonly List<WoodResource> WOOD_RESOURCES = new();
        public static readonly List<StoneResource> STONE_RESOURCES = new();
        public static readonly List<GoldResource> GOLD_RESOURCES = new();
        
        
        /*
         * Paths to load prefabs and data from Resources
         */
        public static readonly Dictionary<UnitType, string> PathToLoadData = new()
        {
            { UnitType.Unit, "" }, // Generic
            
            { UnitType.Character, "Prefabs/Units/Characters/CharacterData" },

            { UnitType.Building, "" }, // Generic
            { UnitType.MainBuilding, "Prefabs/Units/Buildings/MainBuildingData" },
            { UnitType.House, "Prefabs/Units/Buildings/HouseData" }, //TODO
            { UnitType.Tower, "Prefabs/Units/Buildings/TowerData" }, //TODO
            { UnitType.Wall, "Prefabs/Units/Buildings/WallData" }, //TODO
            
            { UnitType.Gold, "Prefabs/Resources/CollectableGoldData" },
            { UnitType.Stone, "Prefabs/Resources/CollectableStoneData" },
            { UnitType.Wood, "Prefabs/Resources/CollectableWoodData" },
        };
        public static readonly Dictionary<UnitType, string> PathToLoadUnitPrefab = new()
        {
            { UnitType.Unit, "" }, // generic
            
            { UnitType.Character, "Prefabs/Units/Characters/Character" },
            
            { UnitType.Building, "" }, // generic
            { UnitType.MainBuilding, "Prefabs/Units/Buildings/MainBuilding" },
            { UnitType.House, "Prefabs/Units/Buildings/House" }, // TODO
            { UnitType.Tower, "Prefabs/Units/Buildings/Tower" }, //TODO
            { UnitType.Wall, "Prefabs/Units/Buildings/Wall" }, //TODO
            
            { UnitType.Gold, "Prefabs/Resources/CollectableGold" },
            { UnitType.Stone, "Prefabs/Resources/CollectableStone" },
            { UnitType.Wood, "Prefabs/Resources/CollectableWood" },
        };
        public static readonly Dictionary<UnitType, string> PathToLoadIconPrefab = new()
        {
            { UnitType.Unit, "" }, // generic

            { UnitType.Character, "Prefabs/UI/UnitIcon/CharacterIcon" },

            { UnitType.Building, "" }, // generic
            { UnitType.MainBuilding, "Prefabs/UI/UnitIcon/MainBuildingIcon" },
            { UnitType.House, "Prefabs/UI/UnitIcon/HouseIcon" },
            
            { UnitType.Gold, "Prefabs/UI/" }, // TODO
            { UnitType.Stone, "Prefabs/UI/" }, // TODO
            { UnitType.Wood, "Prefabs/UI/" }, // TODO
        };
        // public static readonly Dictionary<SkillType, string> PathToLoadButton = new()
        // {
        //     { SkillType.CreateCharacter, "Prefabs/UI/Button/CreateCharacterButton" },
        //     
        //     { SkillType.BuildMainBuilding, "Prefabs/UI/Button/BuildMainBuildingButton" },
        //     { SkillType.BuildHouse, "Prefabs/UI/Button/BuildHouseButton" },
        //     { SkillType.BuildWall, "Prefabs/UI/Button/BuildWallButton" }, // TODO
        //     { SkillType.BuildTower, "Prefabs/UI/Button/BuildTowerButton" }, // TODO
        // };
        public static readonly Dictionary<Team, string> PathToLoadTeamMaterial = new()
        {
            { Team.Blue, "Materials/Unit/BlueTeamMaterial" },
            { Team.Red, "Materials/Unit/RedTeamMaterial" },
        };
        public static readonly Dictionary<BuildingPlacementStatus, string> PathToLoadBuildingPlacementStatusMaterial = new()
        {
            { BuildingPlacementStatus.VALID, "Materials/Unit/Building/ValidMaterial" },
            { BuildingPlacementStatus.INVALID, "Materials/Unit/Building/InvalidMaterial" },
        };
        public static readonly string PathToLoadBuildingMaterial = "Materials/Unit/Building/BuildingMaterial";

        public static readonly Dictionary<ResourceType, string> PathToLoadResourceIcon = new()
        {
            { ResourceType.Gold, "Materials/UI/Resource/gold" },
            { ResourceType.Stone, "Materials/UI/Resource/stone" },
            { ResourceType.Wood, "Materials/UI/Resource/wood" },
        };
        public static readonly string PathToLoadPopulationIcon = "Materials/UI/Population/population";

    }
}
