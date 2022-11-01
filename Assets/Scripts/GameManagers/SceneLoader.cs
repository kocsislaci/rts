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

            // TODO put starter building and some units
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

            new Building(TeamEnum.Blue, mapManager.SampleHeightFromWorldPosition(blueSpawnPoint.transform.position));
            
            var positions = new List<Vector3>
            {
                mapManager.SampleHeightFromWorldPosition(blueSpawnPoint.transform.position + Vector3.back * 10f),
                mapManager.SampleHeightFromWorldPosition(blueSpawnPoint.transform.position + Vector3.back * 12f),
                mapManager.SampleHeightFromWorldPosition(blueSpawnPoint.transform.position + Vector3.back * 14f),
            };
            foreach (var position in positions)
            {
                new Character(TeamEnum.Blue, position);
            }
        }
    }
}
