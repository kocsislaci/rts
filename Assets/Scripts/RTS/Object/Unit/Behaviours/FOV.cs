using RTS.Object.Resource;
using UnityEngine;
using UnityEngine.Events;

namespace RTS.Object.Unit.Behaviours
{
    public class FOV : MonoBehaviour
    {
        public UnityEvent<ObjectController> ObjectEnteredEvent { get; } = new();
        public UnityEvent<ObjectController> ObjectExitedEvent { get; } = new();
        public void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.GetMask("Resource"))
            {
                ObjectEnteredEvent.Invoke(other.gameObject.GetComponent<ResourceController>());
            }
            if (other.gameObject.layer == LayerMask.GetMask("Character") ||
                other.gameObject.layer == LayerMask.GetMask("Building"))
            {
                ObjectEnteredEvent.Invoke(other.gameObject.GetComponent<UnitController>());
            }
        }

        public void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer == LayerMask.GetMask("Resource"))
            {
                ObjectExitedEvent.Invoke(other.gameObject.GetComponent<ResourceController>());
            }
            if (other.gameObject.layer == LayerMask.GetMask("Character") ||
                other.gameObject.layer == LayerMask.GetMask("Building"))
            {
                ObjectExitedEvent.Invoke(other.gameObject.GetComponent<UnitController>());
            }
        }
    }
}
