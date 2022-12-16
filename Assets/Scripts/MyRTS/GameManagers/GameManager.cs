using System.Collections.Generic;
using MyRTS.Object.Resource;
using MyRTS.Object.Unit;
using MyRTS.Player;
using UnityEditor;
using UnityEngine;

namespace MyRTS.GameManagers
{
    public class GameManager : MonoBehaviour
    {
        public Dictionary<Faction, PlayerManager> Players { get; } = new();

        public Dictionary<GUID, UnitController> UnitsOnMap { get; } = new();
        public Dictionary<ResourceType, Dictionary<GUID, ResourceController>> ResourcesOnMap { get; } = new()
        {
            { ResourceType.Gold, new Dictionary<GUID, ResourceController>()},
            { ResourceType.Stone, new Dictionary<GUID, ResourceController>()},
            { ResourceType.Wood, new Dictionary<GUID, ResourceController>()},
        };
        [SerializeField] private MapManager mapManager;
        [SerializeField] private List<Transform> spawnPoints;
        [SerializeField] private bool debugMode;
        
        private void Start()
        {
            LoadScene(new List<Faction>()
            {
                Faction.Blue,
                Faction.Red
            });
        }

        public void LoadScene(List<Faction> playersToLoad)
        {
            LoadMap();
            // foreach (var faction in playersToLoad)
            // {
            //     LoadPlayer(faction, spawnPoints[faction == Faction.Blue ? 0 : 1], faction == Faction.Blue);
            // }
            // if (debugMode)
            // {
            //     DebugUnitLoad();
            // }
        }


        private void LoadMap()
        {
            mapManager.GenerateMap();
        }

        private void LoadPlayer(Faction faction, Transform spawnPoint, bool isItLocal = false)
        {
            Instantiate(
                    Resources.Load<GameObject>(GameResources.PathToLoadPlayerPrefab),
                    Vector3.zero,
                    Quaternion.identity,
                    GameResources.PlayersParent
                )
                .GetComponent<PlayerManager>()
                .InitialisePlayer(
                    faction,
                    spawnPoint.position,
                    GameResources.playerStarterData,
                    isItLocal
                );
        }


        private void DebugUnitLoad()
        {
            var starterPos = GameResources.MapManager.SampleHeightFromWorldPosition(spawnPoints[0].position + Vector3.right * 24f);

            List<Vector3> relPositions = new()
            {
                Vector3.zero,
                Vector3.back * 2f,
                Vector3.back * 4f,
                Vector3.back * 6f,
            };

            foreach (var relPos in relPositions)
            {
                var uc = Instantiate(
                    Resources.Load<GameObject>(GameResources.PathToLoadUnitPrefab[UnitType.Character]),
                    starterPos + relPos,
                    Quaternion.identity,
                    parent: GameResources.UnitsParent
                ).GetComponent<UnitController>();
                uc.InitialiseUnit(Players[Faction.Red], UnitType.Character);
            }
        }
    }
}