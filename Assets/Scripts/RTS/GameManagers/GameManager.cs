using System.Collections.Generic;
using RTS.Object.Resource;
using RTS.Object.Unit;
using RTS.Player;
using UnityEditor;
using UnityEngine;

namespace RTS.GameManagers
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
            foreach (var faction in playersToLoad)
            {
                LoadPlayer(faction, spawnPoints[faction == Faction.Blue ? 0 : 1], faction == Faction.Blue);
            }
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
    }
}