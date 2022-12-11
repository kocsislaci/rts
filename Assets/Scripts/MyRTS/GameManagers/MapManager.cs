using MyRTS.Terrain;
using UnityEngine;

namespace MyRTS.GameManagers
{
    public class MapManager : MonoBehaviour
    {
        [Header("Terrain field")]
        [SerializeField]
        private GameObject terrainGameObject;
        
        /// <summary>
        /// Map generational scripts
        /// </summary>
        private HeightMapGenerator _heightMapGenerator;
        private NavMeshGenerator _navMeshGenerator;
        private ResourceGenerator _resourceGenerator;
        private UnityEngine.Terrain terrain;

        public Vector3 SampleHeightFromWorldPosition(Vector3 position)
        {
            return new Vector3(position.x, terrain.SampleHeight(position), position.z);
        }
        
        public void GenerateMap()
        {
            InitializeFields();
            ExecuteMapGenerationForTheFirstTime();
        }

        private void InitializeFields()
        {
            if (terrainGameObject == null) return;
            terrain = terrainGameObject.GetComponent<UnityEngine.Terrain>();
            _heightMapGenerator = terrainGameObject.GetComponent<HeightMapGenerator>();
            _resourceGenerator = terrainGameObject.GetComponent<ResourceGenerator>();
            _navMeshGenerator = terrainGameObject.GetComponent<NavMeshGenerator>();
        }
        
        private void ExecuteMapGenerationForTheFirstTime()
        {
            if (terrainGameObject == null) return;
            _heightMapGenerator.OnMapGeneration.Invoke();
            _resourceGenerator.onResourceGeneration.Invoke();
            _navMeshGenerator.OnNavMeshBuild.Invoke();
        }
    }
}
