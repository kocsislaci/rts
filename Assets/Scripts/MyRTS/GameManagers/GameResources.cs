using System.Collections.Generic;
using MyRTS.Object.Resource;
using MyRTS.Object.Unit;
using MyRTS.Object.Unit.Building;
using MyRTS.Object.Unit.Capabilities;
using MyRTS.Player;
using MyRTS.RTSCamera;
using MyRTS.UI;
using UnityEngine;

namespace MyRTS.GameManagers
{
    public static class GameResources
    {
        public static GameManager GameManager
        {
            get
            {
                if (_gameManager == null)
                {
                    _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
                }
                return _gameManager;
            }
        }
        private static GameManager _gameManager;
        public static MapManager MapManager
        {
            get
            {
                if (_mapManager == null)
                {
                    _mapManager = GameObject.Find("GameManager").GetComponent<MapManager>();
                }
                return _mapManager;
            }
        }
        private static MapManager _mapManager;
        public static NetworkManager NetworkManager
        {
            get
            {
                if (_networkManager == null)
                {
                    _networkManager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
                }
                return _networkManager;
            }
        }
        private static NetworkManager _networkManager;

        public static PlayerInterfaceController PlayerInterfaceController => GameObject.Find("PlayerInterfaceController").GetComponent<PlayerInterfaceController>();
        public static CameraController CameraController => GameObject.Find("CameraRig").GetComponent<CameraController>();
        
        public static Transform GoldResourcesParent
        {
            get
            {
                if (_goldResourcesParent == null)
                {
                    _goldResourcesParent = GameObject.Find("GoldResourcesParent").GetComponent<Transform>();
                }
                return _goldResourcesParent;
            }
        }
        private static Transform _goldResourcesParent;
        public static Transform StoneResourcesParent
        {
            get
            {
                if (_stoneResourcesParent == null)
                {
                    _stoneResourcesParent = GameObject.Find("StoneResourcesParent").GetComponent<Transform>();
                }
                return _stoneResourcesParent;
            }
        }
        private static Transform _stoneResourcesParent;
        public static Transform WoodResourcesParent
        {
            get
            {
                if (_woodResourcesParent == null)
                {
                    _woodResourcesParent = GameObject.Find("WoodResourcesParent").GetComponent<Transform>();
                }
                return _woodResourcesParent;
            }
        }
        private static Transform _woodResourcesParent;

        public static Transform PlayersParent
        {
            get
            {
                if (_playersParent == null)
                {
                    _playersParent = GameObject.Find("PlayersParent").GetComponent<Transform>();
                }
                return _playersParent;
            }
        }
        private static Transform _playersParent;
        public static Transform UnitsParent
        {
            get
            {
                if (_unitsParent == null)
                {
                    _unitsParent = GameObject.Find("UnitsParent").GetComponent<Transform>();
                }
                return _unitsParent;
            }
        }
        private static Transform _unitsParent;
        public static Transform ProjectilesParent
        {
            get
            {
                if (_projectilesParent == null)
                {
                    _projectilesParent = GameObject.Find("ProjectilesParent").GetComponent<Transform>();
                }
                return _projectilesParent;
            }
        }
        private static Transform _projectilesParent;
        
        

        public static StarterData playerStarterData = new StarterData(
            new Dictionary<ResourceType, int>()
            {
                { ResourceType.Gold, 100 },
                { ResourceType.Stone, 100 },
                { ResourceType.Wood, 100 },
            },
            new Dictionary<Vector3, UnitType>()
            {
                { Vector3.zero, UnitType.MainBuilding },
                { Vector3.forward * 10f, UnitType.House },
                { Vector3.right * 10f, UnitType.Tower },
                { Vector3.left * 10f, UnitType.Wall },
                
                { Vector3.back * 10f, UnitType.Character },
                { Vector3.back * 12f, UnitType.Character },
                { Vector3.back * 14f, UnitType.Character },
            }
        );


        public static readonly Dictionary<ResourceType, string> PathToLoadResourceData = new()
        {
            { ResourceType.Gold, "Prefabs/Resources/GoldData" },
            { ResourceType.Stone, "Prefabs/Resources/StoneData" },
            { ResourceType.Wood, "Prefabs/Resources/WoodData" },
        };
        public static readonly Dictionary<ResourceType, string> PathToLoadResourcePrefab = new()
        {
            { ResourceType.Gold, "Prefabs/Resources/Gold" },
            { ResourceType.Stone, "Prefabs/Resources/Stone" },
            { ResourceType.Wood, "Prefabs/Resources/Wood" },
        };

        public static readonly string PathToLoadPlayerPrefab = "Prefabs/Players/PlayerManager";

        public static readonly Dictionary<UnitType, string> PathToLoadUnitData = new()
        {
            { UnitType.Character, "Prefabs/Units/Characters/CharacterData" }, // shall be generic
            { UnitType.MainBuilding, "Prefabs/Units/Buildings/MainBuildingData" },
            { UnitType.House, "Prefabs/Units/Buildings/HouseData" },
            { UnitType.Tower, "Prefabs/Units/Buildings/TowerData" },
            { UnitType.Wall, "Prefabs/Units/Buildings/WallData" },
        };
        public static readonly Dictionary<UnitType, string> PathToLoadUnitPrefab = new()
        {
            { UnitType.Character, "Prefabs/Units/Characters/Character" }, // shall be generic
            { UnitType.MainBuilding, "Prefabs/Units/Buildings/MainBuilding" },
            { UnitType.House, "Prefabs/Units/Buildings/House" },
            { UnitType.Tower, "Prefabs/Units/Buildings/Tower" },
            { UnitType.Wall, "Prefabs/Units/Buildings/Wall" },
        };
        public static readonly Dictionary<UnitType, string> PathToLoadUnitIconPrefab = new()
        {
            { UnitType.Character, "Prefabs/UI/UnitIcons/Characters/CharacterIcon" }, // shall be generic
            { UnitType.MainBuilding, "Prefabs/UI/UnitIcons/Buildings/MainBuildingIcon" },
            { UnitType.House, "Prefabs/UI/UnitIcons/Buildings/HouseIcon" },
            { UnitType.Tower, "Prefabs/UI/UnitIcons/Buildings/TowerIcon" },
            { UnitType.Wall, "Prefabs/UI/UnitIcons/Buildings/WallIcon" },
        };
        public static readonly Dictionary<UnitType, string> PathToLoadTrainerActionButton = new()
        {
            { UnitType.Character, "Prefabs/UI/ActionButtons/CreateCharacterButton" },
        };
        public static readonly Dictionary<UnitType, string> PathToLoadBuilderActionButton = new()
        {
            { UnitType.MainBuilding, "Prefabs/UI/ActionButtons/BuildMainBuildingButton" },
            { UnitType.House, "Prefabs/UI/ActionButtons/BuildHouseButton" },
            { UnitType.Wall, "Prefabs/UI/ActionButtons/BuildWallButton" },
            { UnitType.Tower, "Prefabs/UI/ActionButtons/BuildTowerButton" },
        };
        public static readonly Dictionary<CapabilityType, string> PathToLoadCapabilityData = new()
        {
            { CapabilityType.Attacker,  ""},
            { CapabilityType.Harvester,  ""},
            { CapabilityType.Builder,  ""}
        };

        
        
        public static readonly Dictionary<Faction, string> PathToLoadTeamMaterial = new()
        {
            { Faction.Blue, "Materials/Unit/Team/BlueTeamMaterial" },
            { Faction.Cyan, "Materials/Unit/Team/CyanTeamMaterial" },
            { Faction.Green, "Materials/Unit/Team/GreenTeamMaterial" },
            { Faction.Magenta, "Materials/Unit/Team/MagentaTeamMaterial" },
            { Faction.Neutral, "Materials/Unit/Team/NeutralTeamMaterial" },
            { Faction.Red, "Materials/Unit/Team/RedTeamMaterial" },
            { Faction.Yellow, "Materials/Unit/Team/YellowTeamMaterial" },
        };
        public static readonly string PathToLoadCharacterMaterial = "Materials/Unit/Unit/CharacterMaterial";
        public static readonly string PathToLoadBuildingMaterial = "Materials/Unit/Building/BuildingMaterial";
        public static readonly Dictionary<BuildingPlacementStatus, string> PathToLoadBuildingPlacementStatusMaterial = new()
        {
            { BuildingPlacementStatus.INVALID, "Materials/Unit/Building/InvalidMaterial" },
            { BuildingPlacementStatus.VALID, "Materials/Unit/Building/ValidMaterial" },
        };
    }
}
