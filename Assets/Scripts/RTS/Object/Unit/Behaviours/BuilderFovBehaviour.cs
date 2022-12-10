using RTS.Object.Unit.Building;
using UnityEngine;
using UnityEngine.Events;

namespace RTS.Object.Unit.Behaviours
{
    public class BuilderFovBehaviour : MonoBehaviour
    {
        private SphereCollider fovCollider;
        public UnityEvent<BuildingController> buildingEntered = new();
        public UnityEvent<BuildingController> buildingExited = new();
        
        private void Awake()
        {
            fovCollider = GetComponent<SphereCollider>();
        }
        public void SetFOV(float range)
        {
            fovCollider.radius = range;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<BuildingController>() != null)
                buildingEntered.Invoke(other.GetComponent<BuildingController>());
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<BuildingController>() != null)
                buildingExited.Invoke(other.GetComponent<BuildingController>());
        }
    }
}
