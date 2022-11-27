using System.Collections.Generic;
using TerrainObject;
using Unit;
using Unit.Building;
using Unit.Character;
using Unit.ResourceObject;
using UnityEngine;

namespace GameManagers
{
    public class SceneLoader : MonoBehaviour
    {
        [Header("Map generation")]
        [SerializeField] private MapManager mapManager;

        [Header("Starter objects generation")]
        [SerializeField] private GameObject blueSpawnPoint;
        [SerializeField] private GameObject redSpawnPoint;

        [Header("Camera positioning")]
        [SerializeField] private CameraController cameraRig;

        void Start()
        {
            LoadMyStarterResources(
                new ResourceValue(ResourceType.Gold, 100),
                new ResourceValue(ResourceType.Stone, 90),
                new ResourceValue(ResourceType.Wood, 80)
            );

            // load map
            mapManager.GenerateMap();
            
            // set camera
            cameraRig.SetStartPosition(mapManager.SampleHeightFromWorldPosition(blueSpawnPoint.transform.position));

            // Put some units for the player and set owner color
            LoadMyStarterBuildingAndUnits();
        }

        private void LoadMyStarterResources(
            ResourceValue goldAmount,
            ResourceValue stoneAmount,
            ResourceValue woodAmount
        )
        {
            GameManager.MyResources[goldAmount.type].Amount = goldAmount.amount;
            GameManager.MyResources[stoneAmount.type].Amount = stoneAmount.amount;
            GameManager.MyResources[woodAmount.type].Amount = woodAmount.amount;
        }

        private void LoadMyStarterBuildingAndUnits()
        {
            /*
             * Set self color in GameManager
             */
            GameManager.MyTeam = Team.Blue;

            /*
             * Adding 1 MainBuilding
             */
            var blueSpawnPointPosition = blueSpawnPoint.transform.position;
            new Building(Team.Blue, mapManager.SampleHeightFromWorldPosition(blueSpawnPointPosition), true);
            
            /*
             * Adding 3 characters
             */
            var positions = new List<Vector3>
            {
                mapManager.SampleHeightFromWorldPosition(blueSpawnPointPosition + Vector3.back * 10f),
                mapManager.SampleHeightFromWorldPosition(blueSpawnPointPosition + Vector3.back * 12f),
                mapManager.SampleHeightFromWorldPosition(blueSpawnPointPosition + Vector3.back * 14f),
            };
            foreach (var position in positions)
            {
                new Character(Team.Blue, position);
            }
            
            
            // one other team unit
            var pos = mapManager.SampleHeightFromWorldPosition(blueSpawnPoint.transform.position + Vector3.back * 12f + Vector3.left * 4f);
            new Character(Team.Red, pos);
        }
    }
}
