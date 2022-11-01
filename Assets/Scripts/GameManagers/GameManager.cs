using System.Collections.Generic;
using GameManagers.Pattern;
using GameManagers.Resources;
using Unit.Building;
using Unit.Character;
using UnityEngine;

namespace GameManagers
{
    public class GameManager : Singleton
    {
        private static GameObject gameManagerGameObject;
        public static GameObject GameManagerGameObject
        {
            get
            {
                if (gameManagerGameObject == null)
                {
                    gameManagerGameObject = GameObject.Find("GameManager");
                }

                return gameManagerGameObject;
            }
            private set {}
        }
        
        
        
        public static readonly List<Character> CHARACTERS = new(); 
        public static readonly List<Building> BUILDINGS = new();

        public static readonly List<CharacterSelectionController> SELECTED_CHARACTERS = new(); 
        public static readonly List<BuildingSelectionController> SELECTED_BUILDINGS = new();
        
        // public static ICommand SELECTED_COMMAND;
        // public static IMoveStrategies SELECTED_STRATEGY;
        
        public static readonly List<Resource> Resources = new()
        {
            new(ResourceType.Gold),
            new(ResourceType.Stone),
            new(ResourceType.Wood)
        };
        public static readonly Population Population = new(maxPopulation: 200);


        public static readonly Dictionary<WhatToLoadEnum, string> PathToLoadPrefab = new()
        {
            { WhatToLoadEnum.Unit, "" }, //
            
            { WhatToLoadEnum.Character, "Prefabs/Units/Characters/Character" },

            
            { WhatToLoadEnum.Building, "" }, // 
            { WhatToLoadEnum.MainBuilding, "Prefabs/Units/Buildings/MainBuilding" },
            { WhatToLoadEnum.House, "Prefabs/Units/Buildings/House" },
            { WhatToLoadEnum.Tower, "Prefabs/Units/Buildings/Tower" }, //TODO
            { WhatToLoadEnum.Wall, "Prefabs/Units/Buildings/Wall" }, //TODO
            
            { WhatToLoadEnum.Gold, "Prefabs/Resources/CollectableGold" },
            { WhatToLoadEnum.Stone, "Prefabs/Resources/CollectableStone" },
            { WhatToLoadEnum.Wood, "Prefabs/Resources/CollectableWood" },
        };
        
        public static readonly Dictionary<WhatToLoadEnum, string> PathToLoadData = new()
        {
            { WhatToLoadEnum.Unit, "" },
            
            { WhatToLoadEnum.Character, "Prefabs/Units/Characters/CharacterData" },

            { WhatToLoadEnum.Building, "" },
            { WhatToLoadEnum.MainBuilding, "Prefabs/Units/Buildings/MainBuildingData" },
            { WhatToLoadEnum.House, "Prefabs/Units/Buildings/HouseData" }, //TODO
            { WhatToLoadEnum.Tower, "Prefabs/Units/Buildings/TowerData" }, //TODO
            { WhatToLoadEnum.Wall, "Prefabs/Units/Buildings/WallData" }, //TODO
            
            { WhatToLoadEnum.Gold, "Prefabs/Resources/CollectableGoldData" },
            { WhatToLoadEnum.Stone, "Prefabs/Resources/CollectableStoneData" },
            { WhatToLoadEnum.Wood, "Prefabs/Resources/CollectableWoodData" },
        };
    }
}
