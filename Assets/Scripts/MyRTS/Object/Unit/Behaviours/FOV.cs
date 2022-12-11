using MyRTS.Object.Resource;
using UnityEngine;
using UnityEngine.Events;

namespace MyRTS.Object.Unit.Behaviours
{
    public class FOV : MonoBehaviour
    {
        public UnityEvent<ResourceController> ResourceEnteredEvent { get; } = new();
        public UnityEvent<ResourceController> ResourceExitedEvent { get; } = new();
        public UnityEvent<UnitController> UnitEnteredEvent { get; } = new();
        public UnityEvent<UnitController> UnitExitedEvent { get; } = new();

        public void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<ResourceController>() != null)
            {
                ResourceEnteredEvent.Invoke(other.gameObject.GetComponent<ResourceController>());
            }
            if (other.GetComponent<UnitController>() != null)
            {
                UnitEnteredEvent.Invoke(other.gameObject.GetComponent<UnitController>());
            }
        }
        public void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<ResourceController>() != null)
            {
                ResourceExitedEvent.Invoke(other.gameObject.GetComponent<ResourceController>());
            }
            if (other.GetComponent<UnitController>() != null)
            {
                UnitExitedEvent.Invoke(other.gameObject.GetComponent<UnitController>());
            }
        }
    }
}
