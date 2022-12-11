using System;
using System.Collections.Generic;
using MyRTS.GameManagers;
using MyRTS.Object.Resource;
using MyRTS.Object.Resource.Dto;
using MyRTS.Object.Unit;
using MyRTS.Object.Unit.Building;
using MyRTS.Player.PlayerInputManager;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using CharacterController = MyRTS.Object.Unit.Character.CharacterController;

namespace MyRTS.Player
{
    public class PlayerManager: MonoBehaviour
    {
        [field: SerializeField] public Faction Faction { get; private set; } = Faction.Neutral;
        [field: SerializeField] public Dictionary<ResourceType, int> MyResources { get; private set; } = new()
        {
            { ResourceType.Gold, 0 },
            { ResourceType.Stone, 0 },
            { ResourceType.Wood, 0 },
        };
        [field: SerializeField] public Population MyPopulation { get; private set; }
        [field: SerializeField] public Dictionary<GUID ,UnitController> MyUnits { get; private set; } = new();
        [field: SerializeField] public Dictionary<GUID ,UnitController> MySelectedUnits { get; private set; } = new();
 
        
        public UnityEvent ResourcesChanged { get; } = new();
        public UnityEvent<PlayerInterfaceDto> PlayerStateChanged { get; } = new();
        
        public PlayerInputManager.PlayerInputManager PlayerInputManager => GetComponent<PlayerInputManager.PlayerInputManager>();
        public PlayerInterfaceManager PlayerInterfaceManager => GetComponent<PlayerInterfaceManager>();
        public PlayerCameraManager PlayerCameraManager => GetComponent<PlayerCameraManager>();
        
        // save here the available actions?


        public void InitialisePlayer(Faction faction, Vector3 starterPosition, StarterData starterData, bool isItLocal = false)
        {
            Faction = faction;
            foreach (var resource in starterData.starterResources)
            {
                MyResources[resource.Key] = resource.Value;
            }
            MyPopulation = new Population(200); 

            var starterPos = GameResources.MapManager.SampleHeightFromWorldPosition(starterPosition);

            foreach (var keyValuePair in starterData.starterUnitsOnRelativePosition)
            {
                var uc = Instantiate(
                    Resources.Load<GameObject>(GameResources.PathToLoadUnitPrefab[keyValuePair.Value]),
                    starterPos + keyValuePair.Key,
                    Quaternion.identity,
                    parent: GameResources.UnitsParent
                ).GetComponent<UnitController>();
                    uc.InitialiseUnit(this, keyValuePair.Value);
                    if (uc is BuildingController bc) bc.DebugBuilt();
            }
            
            PlayerInputManager.enabled = isItLocal;
            PlayerInterfaceManager.enabled = isItLocal;
            PlayerCameraManager.enabled = isItLocal;
            if (isItLocal)
            {            
                // PlayerCameraManager
                GameResources.CameraController.SetStartPosition(starterPos);
                
                // PlayerInterfaceManager 
                ResourcesChanged.AddListener(OnResourcesChanged);
                MyPopulation.PopulationChanged.AddListener(OnPopulationChanged);
                GameResources.PlayerInterfaceController.InitialisePlayerInterfaceController(this);
                PlayerStateChanged.Invoke(new PlayerInterfaceDto(MyResources, MyPopulation.GetActualValues()));
                
                // PlayerInputManager
                PlayerInputManager.InitialisePlayerInputManager(this);
                PlayerInputManager.UserInputStateChange.AddListener(OnUserInputStateChange);
            }
            GameResources.GameManager.Players.Add(faction, this);
        }
        private void OnPopulationChanged(Tuple<int, int> newPopulation)
        {
            PlayerStateChanged.Invoke(new PlayerInterfaceDto(null, newPopulation));
        }
        private void OnResourcesChanged()
        {
            PlayerStateChanged.Invoke(new PlayerInterfaceDto(MyResources));
        }

        private void OnUserInputStateChange(UserInputState newState, List<UnitType> typesOfSelected)
        {
            var actionButtons = new List<RectTransform>();
            var unitIcons = new List<RectTransform>();
            
            switch (newState)
            {
                case UserInputState.BuildActionSelected:
                    return; // sends null
                case UserInputState.NothingSelected:
                    // sends empty, but not null -> changes to empty
                    break;
                case UserInputState.OneUnitTypeSelected:
                    UnitController uc = null;
                    foreach (var mySelectedUnit in MySelectedUnits)
                    {
                        uc = mySelectedUnit.Value;
                        break;
                    }
                    if (uc != null)
                    {
                        switch (uc.Data.type)
                        {
                            case UnitType.Character:
                                foreach (var buildable in ((CharacterController)uc).BuilderData.buildables)
                                {
                                    actionButtons.Add(Instantiate(Resources.Load<GameObject>(GameResources.PathToLoadBuilderActionButton[buildable.buildingType])).GetComponent<RectTransform>());
                                    // create button, set callback to instantiate building and set InputState if possible
                                }
                                break;
                            case UnitType.MainBuilding:
                                // foreach (var buildable in ((MainBuildingController)uc).TrainerData.buildables)
                                // {
                                //     actionButtons.Add(Instantiate(Resources.Load<GameObject>(GameResources.PathToLoadBuilderActionButton[buildable.buildingType])).GetComponent<RectTransform>());
                                //     // create button, set callback to instantiate building and set InputState if possible
                                // }
                                break;
                        } 
                    }
                    goto case UserInputState.MultipleUnitTypesSelected;
                case UserInputState.MultipleUnitTypesSelected:
                    foreach (var mySelectedUnit in MySelectedUnits)
                    {
                        unitIcons.Add(Instantiate(Resources.Load<GameObject>(GameResources.PathToLoadUnitIconPrefab[mySelectedUnit.Value.Data.type]), Vector3.zero, Quaternion.identity).GetComponent<RectTransform>());
                    }
                    break;
            }
            var dto = new PlayerInterfaceDto(null, null, actionButtons, unitIcons);
            PlayerStateChanged.Invoke(dto);
        }
        
        

        public void AddResource(Dictionary<ResourceType, int> incomingResources)
        {
            foreach (var incomingResource in incomingResources)
            {
                MyResources[incomingResource.Key] += incomingResource.Value;
            }
            ResourcesChanged.Invoke();
        }
        
        public void SpendResources(Dictionary<ResourceType, int> costResources)
        {
            foreach (var incomingResource in costResources)
            {
                MyResources[incomingResource.Key] -= incomingResource.Value;
            }
            ResourcesChanged.Invoke();
        }
        
        
        private void Start()
        {
            // cameraRig.SetStartPosition(mapManager.SampleHeightFromWorldPosition(blueSpawnPoint.transform.position));
            //
            // // Put some units for the player and set owner color
            // LoadMyStarterBuildingAndUnits();
            //
        }
    }
}
