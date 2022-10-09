using System.Collections;
using UnityEngine.Events;
using Unity.AI.Navigation;

namespace Terrain
{
    using UnityEngine;

    [RequireComponent(typeof(Terrain))]
    [RequireComponent(typeof(TerrainCollider))]
    public class TerrainMeshGenerator : MonoBehaviour
    {
        private NavMeshSurface _navMeshSurface;
        public UnityAction MapMeshGenerationAction;

        private void Awake()
        {
            _navMeshSurface = GetComponent<NavMeshSurface>();
            _navMeshSurface.BuildNavMesh();
            
            // MapMeshGenerationAction += UpdateMesh;
        }

        public void UpdateMesh()
        {
            _navMeshSurface.UpdateNavMesh(_navMeshSurface.navMeshData);
        }

        public void StartRepetablyUpdateMesh()
        {
            StartCoroutine(RepeatablyUpdateMesh());
        }
        
        private IEnumerator RepeatablyUpdateMesh()
        {
            while (true)
            {
                yield return new WaitForSeconds(1f);
            }
        }
    }
}
