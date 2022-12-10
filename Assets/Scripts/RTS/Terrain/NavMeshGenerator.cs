using System.Collections;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace RTS.Terrain
{
    [RequireComponent(typeof(UnityEngine.Terrain))]
    [RequireComponent(typeof(TerrainCollider))]
    public class NavMeshGenerator : MonoBehaviour
    {
        [SerializeField]
        private NavMeshSurface navMeshSurface;

        [SerializeField] [Range(.5f, 10f)]
        private float intervalToUpdateMesh;
        
        /// <summary>
        /// Action which can be invoked from the outside to trigger callbacks
        /// </summary>
        public UnityAction OnNavMeshUpdate;
        private void SetCallbackToOnMapMeshGenerationEvent()
        {
            OnNavMeshUpdate += UpdateNavMeshCallback;
        }

        public UnityAction OnNavMeshBuild;
        private void SetCallbackToOnNavMeshBuildEvent()
        {
            OnNavMeshBuild += InitNavMesh;
        }
        
        private void Awake()
        {
            SetCallbackToOnMapMeshGenerationEvent();
            SetCallbackToOnNavMeshBuildEvent();
        }
        
        [ContextMenu("InitNavMesh")]
        private void InitNavMesh()
        {
            navMeshSurface.navMeshData = new NavMeshData();
            navMeshSurface.BuildNavMesh();
            UpdateNavMeshCallback();
        }
        [ContextMenu("UpdateNavMesh")]
        private void UpdateNavMeshCallback()
        {
            navMeshSurface.UpdateNavMesh(navMeshSurface.navMeshData);
        }

        // Starts Coroutine scope to update nav mesh
        //
        [ContextMenu("StartNavMeshCoroutine")]
        private void StartUpdateNavMeshCoroutine()
        {
            StartCoroutine(RepeatablyUpdateMesh());
        }
        [ContextMenu("StopNavMeshCoroutine")]
        private void StopUpdateNavMeshCoroutine()
        {
            StopCoroutine(RepeatablyUpdateMesh());
        }
        private IEnumerator RepeatablyUpdateMesh()
        {
            while (true)
            {
                yield return new WaitForSeconds(intervalToUpdateMesh);
                UpdateNavMeshCallback();
            }
        }
        //
        // Ends Coroutine scope to update nav mesh
    }
}
