using RTS.Object.Resource;
using RTS.Object.Unit.Building;
using RTS.Object.Unit.Capabilities.Building;
using UnityEngine;
using UnityEngine.Events;

namespace RTS.Object.Unit.Behaviours
{
    public class HarvestFovBehaviour : MonoBehaviour
    {
        private SphereCollider fovCollider;
        public UnityEvent<ResourceController> harvestableEntered = new();
        public UnityEvent<ResourceController> harvestableExited = new();
        public UnityEvent<IHarvestReceiver> harvestReceiverEntered = new();
        public UnityEvent<IHarvestReceiver> harvestReceiverExited = new();
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
            if (other.GetComponent<ResourceController>() != null)
                harvestableEntered.Invoke(other.GetComponent<ResourceController>());
            var building = other.GetComponent<BuildingController>();
            if (building is IHarvestReceiver receiver)
                harvestReceiverEntered.Invoke(receiver);
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<ResourceController>() != null)
                harvestableExited.Invoke(other.GetComponent<ResourceController>());
            var building = other.GetComponent<BuildingController>();
            if (building is IHarvestReceiver receiver)
                harvestReceiverEntered.Invoke(receiver);
        }
    }
}