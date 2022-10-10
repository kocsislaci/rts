
namespace Terrain
{
    using UnityEngine;
    using UnityEngine.UI;

    public class MapManager : MonoBehaviour
    {
        [Header("Debug flags")]
        [SerializeField] private bool isDebugMode;
        public bool IsDebugMode
        {
            set
            {
                if (value)
                {
                    SetUpListeners();
                }
                else
                {
                    GetOffListeners();
                }
                isDebugMode = value;
            }
        }

        [Header("Terrain field")]
        [SerializeField]
        private GameObject terrain;
        
        /// <summary>
        /// Map generational scripts
        /// </summary>
        private HeightMapGenerator _heightMapGenerator;
        private NavMeshGenerator _navMeshGenerator;
        private ResourceGenerator _resourceGenerator;

        /// <summary>
        /// Debug button to regenerate the terrain, is public to be easy to set up.
        /// </summary>
        public Button generateMapButton;

        public void Start()
        {
            InitializeFields();
            if (isDebugMode)
                SetUpListeners();
            ExecuteMapGenerationForTheFirstTime();
        }
        
        private void InitializeFields()
        {
            // Saving references of generational functions
            if (terrain == null) return;
            _heightMapGenerator = terrain.GetComponent<HeightMapGenerator>();
            _resourceGenerator = terrain.GetComponent<ResourceGenerator>();
            _navMeshGenerator = terrain.GetComponent<NavMeshGenerator>();
            //
        }
        private void SetUpListeners()
        {
            // For debug purposes
            generateMapButton.onClick.AddListener(_heightMapGenerator.onMapGeneration);
            generateMapButton.onClick.AddListener(_resourceGenerator.onResourceGeneration);
            generateMapButton.onClick.AddListener(_navMeshGenerator.onNavMeshUpdate);
            //
        }
        private void GetOffListeners()
        {
            generateMapButton.onClick.RemoveAllListeners();
        }
        
        private void ExecuteMapGenerationForTheFirstTime()
        {
            if (terrain == null) return;
            _heightMapGenerator.onMapGeneration.Invoke();
            _resourceGenerator.onResourceGeneration.Invoke();
            _navMeshGenerator.onNavMeshBuild.Invoke();
        }
    }
}
