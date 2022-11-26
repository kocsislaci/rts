using System.Collections.Generic;
using TerrainObject;
using Unit;
using Unit.Building;
using Unit.Character;
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
            // mock start
            LoadStarterResources();

            // load map
            mapManager.GenerateMap();
            
            // set camera
            cameraRig.SetStartPosition(mapManager.SampleHeightFromWorldPosition(blueSpawnPoint.transform.position));

            // Put some units for the player and set owner color
            LoadStarterBuildingAndUnits();
        }

        private void LoadStarterResources()
        {
            GameManager.Resources[0].Amount = 100;
            GameManager.Resources[1].Amount = 90;
            GameManager.Resources[2].Amount = 80;
        }

        private void LoadStarterBuildingAndUnits()
        {
            /*
             * Set self color in GameManager
             */
            GameManager.MyTeam = Team.Blue;

            /*
             * Adding 1 MainBuilding
             */
            new Building(Team.Blue, mapManager.SampleHeightFromWorldPosition(blueSpawnPoint.transform.position), true);
            
            /*
             * Adding 3 characters
             */
            var positions = new List<Vector3>
            {
                mapManager.SampleHeightFromWorldPosition(blueSpawnPoint.transform.position + Vector3.back * 10f),
                mapManager.SampleHeightFromWorldPosition(blueSpawnPoint.transform.position + Vector3.back * 12f),
                mapManager.SampleHeightFromWorldPosition(blueSpawnPoint.transform.position + Vector3.back * 14f),
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
