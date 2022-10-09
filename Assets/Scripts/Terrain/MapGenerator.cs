using UnityEngine.Serialization;

namespace Terrain
{
    using UnityEngine;
    using UnityEngine.UI;

    public class MapGenerator : MonoBehaviour
    {
        /// <summary>
        /// Terrain object
        /// </summary>
        private GameObject _terrain;
        
        /// <summary>
        /// Map generational scripts
        /// </summary>
        private CreateHeightMap _createHeightMap;
        private TerrainMeshGenerator _terrainMeshGenerator;
        private ResourceGenerator _resourceGenerator;

        /// <summary>
        /// Debug button to regenerate the terrain, is public to be easy to set up.
        /// </summary>
        public Button generateMapButton;

        public void Start()
        {
            Init();
            ExecuteMapGenerationFunctionalities();
        }
        
        private void Init()
        {
            // Terrain object instantiated
            _terrain = Instantiate(Resources.Load($"Terrain/Terrain")) as GameObject;
            // Saving references of generational functions
            if (_terrain == null) return;
            _createHeightMap = _terrain.GetComponent<CreateHeightMap>();
            _resourceGenerator = _terrain.GetComponent<ResourceGenerator>();
            _terrainMeshGenerator = _terrain.GetComponent<TerrainMeshGenerator>();
            //
            
            // For debug purposes
            generateMapButton.onClick.AddListener(_createHeightMap.onMapGeneration);
            generateMapButton.onClick.AddListener(_resourceGenerator.ResourceGenerationAction);
            generateMapButton.onClick.AddListener(_terrainMeshGenerator.MapMeshGenerationAction);
            //
        }
        public void ExecuteMapGenerationFunctionalities()
        {
            if (_terrain == null) return;
            _createHeightMap.onMapGeneration.Invoke();
            _resourceGenerator.ResourceGenerationAction.Invoke();
            _terrainMeshGenerator.MapMeshGenerationAction.Invoke();
        }
    }
}