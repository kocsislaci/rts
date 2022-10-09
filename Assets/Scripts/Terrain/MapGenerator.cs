
namespace Terrain
{
    using UnityEngine;
    using UnityEngine.UI;

    public class MapGenerator : MonoBehaviour
    {
        /// <summary>
        /// Terrain object
        /// </summary>
        /// Is loaded from inspector
        [SerializeField]
        private GameObject terrain;
        
        /// <summary>
        /// Map generational scripts
        /// </summary>
        private HeightMapGenerator _heightMapGenerator;
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
            // Saving references of generational functions
            if (terrain == null) return;
            _heightMapGenerator = terrain.GetComponent<HeightMapGenerator>();
            _resourceGenerator = terrain.GetComponent<ResourceGenerator>();
            _terrainMeshGenerator = terrain.GetComponent<TerrainMeshGenerator>();
            //
            
            // For debug purposes
            generateMapButton.onClick.AddListener(_heightMapGenerator.onMapGeneration);
            generateMapButton.onClick.AddListener(_resourceGenerator.ResourceGenerationAction);
            generateMapButton.onClick.AddListener(_terrainMeshGenerator.MapMeshGenerationAction);
            //
        }
        public void ExecuteMapGenerationFunctionalities()
        {
            if (terrain == null) return;
            _heightMapGenerator.onMapGeneration.Invoke();
            _resourceGenerator.ResourceGenerationAction.Invoke();
            _terrainMeshGenerator.MapMeshGenerationAction.Invoke();
        }
    }
}
